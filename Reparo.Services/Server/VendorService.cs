using Microsoft.EntityFrameworkCore;

public interface IVendorService
{
    Task<VendorModel?> GetVendorByPlaceAsync(string placeId, CancellationToken cancellationToken = default);
    Task<VendorModel?> GetVendorAsync(int vendorId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<VendorModel>> GetVendorListAsync(CancellationToken cancellationToken = default);
    Task AddVendorsAsync(PlaceDto[] places, CancellationToken cancellationToken = default);

    // not used
    //Task<long> AddVendorAsync(VendorModel vendor, CancellationToken cancellationToken = default);
    //Task<bool> VendorExistsAsync(string placeId, CancellationToken cancellationToken = default);
}

public sealed class VendorService : IVendorService
{
    private readonly AppDbContext _context;

    public VendorService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<VendorModel?> GetVendorByPlaceAsync(string placeId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<VendorModel>().AsNoTracking().Where(e => e.PlaceId == placeId).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<VendorModel?> GetVendorAsync(int vendorId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<VendorModel>().AsNoTracking().Where(e => e.Id == vendorId).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<VendorModel>> GetVendorListAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Set<VendorModel>().AsNoTracking().Where(v => v.IsActive).OrderBy(v => v.UpdatedAt).ToListAsync(cancellationToken);
    }

    public async Task AddVendorsAsync(PlaceDto[] places, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(places);

        if (places.Length == 0) return;

        var placeIds = places
            .Where(x => !string.IsNullOrWhiteSpace(x.PlaceId))
            .Select(x => x.PlaceId!)
            .Distinct()
            .ToList();

        var existingPlaceIds = await _context.Vendors
            .Where(x => x.PlaceId != null && placeIds.Contains(x.PlaceId))
            .Select(x => x.PlaceId!)
            .ToListAsync(cancellationToken);

        var existingSet = existingPlaceIds.ToHashSet();
        var now = DateTimeOffset.UtcNow;
        var vendors = places
            .Where(x =>
                !string.IsNullOrWhiteSpace(x.PlaceId) &&
                !existingSet.Contains(x.PlaceId) &&
                !string.IsNullOrWhiteSpace(x.Name))
            .Select(place => new VendorModel
            {
                // Business identity
                Name = place.Name!,
                PlaceId = place.PlaceId,
                GoogleMaps = place.GoogleMaps,

                // Address
                Address = place.Address,
                AddressLine1 = place.AddressLine1,
                City = place.City,
                State = place.State,
                PostalCode = place.PostalCode,
                Country = place.Country ?? "US",

                // Geo
                Latitude = place.Latitude,
                Longitude = place.Longitude,

                Status = place.Status,
                TravelToYou = place.TravelToYou,

                PrimaryType = place.PrimaryType,
                PrimaryTypeDisplayName = place.PrimaryTypeDisplayName,
                Types = place.Types,

                // Operational
                IsActive = place.Status == "OPEN",
                IsVerified = false,
                IsPreferred = false,

                // Audit
                CreatedAt = now,
                UpdatedAt = now
            })
            .ToList();

        if (vendors.Count == 0) return;

        _context.Vendors.AddRange(vendors);
        await _context.SaveChangesAsync(cancellationToken);
    }

    //    vendor.CreatedAt = DateTimeOffset.UtcNow;
    //    vendor.UpdatedAt = DateTimeOffset.UtcNow;
    //    _context.Set<VendorModel>().Add(vendor);
    //    await _context.SaveChangesAsync(cancellationToken);
    //    return vendor.Id;
    //}

    //public async Task UpdateVendorAsync(VendorModel vendor, CancellationToken cancellationToken = default)
    //{
    //    ArgumentNullException.ThrowIfNull(vendor);
    //    vendor.UpdatedAt = DateTimeOffset.UtcNow;
    //    _context.Set<VendorModel>().Update(vendor);
    //    await _context.SaveChangesAsync(cancellationToken);
    //}

    //public async Task<bool> VendorExistsAsync(string placeId, CancellationToken cancellationToken = default)
    //{
    //    if (string.IsNullOrWhiteSpace(placeId)) return false;
    //    return await _context.Set<VendorModel>().AsNoTracking().AnyAsync(v => v.PlaceId == placeId, cancellationToken);
    //}
}