using Microsoft.EntityFrameworkCore;

public interface IVendorService
{
    Task<VendorModel?> GetVendorByPlaceAsync(string placeId, CancellationToken cancellationToken = default);
    Task<VendorModel?> GetVendorAsync(int vendorId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<VendorModel>> GetVendorListAsync(CancellationToken cancellationToken = default);
    Task AddVendorAsync(PlaceDto vendor, CancellationToken cancellationToken = default);

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

    public async Task AddVendorAsync(PlaceDto place, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(place);
        if (string.IsNullOrWhiteSpace(place.PlaceId)) throw new ArgumentException("Place Id is required.", nameof(place));

        var existingVendor = await _context.Set<VendorModel>().FirstOrDefaultAsync(v => v.PlaceId == place.PlaceId, cancellationToken);
        if (existingVendor is not null) return;

        var now = DateTimeOffset.UtcNow;
        var vendor = new VendorModel
        {
            Name = place.Title ?? place.Name ?? "Unknown Vendor",
            //legalName
            //Description
            //email
            //addresslane1

            PlaceId = place.PlaceId,

            Phone = place.Phone,
            WebsiteUrl = place.Website,

            AddressLine1 = place.Address,
            State = place.Region,

            Latitude = place.Latitude,
            Longitude = place.Longitude,

            Rating = place.Rating.HasValue ? Convert.ToDecimal(place.Rating.Value) : null,

            ReviewCount = place.ReviewCount,

            IsActive = true,
            IsVerified = false,
            IsPreferred = false,

            CreatedAt = now,
            UpdatedAt = now
        };

        _context.Vendors.Add(vendor);
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