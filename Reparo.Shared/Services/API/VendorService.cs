using Microsoft.EntityFrameworkCore;

public interface IVendorService
{
    Task<VendorModel?> GetVendorByPlaceIdAsync(string placeId, CancellationToken cancellationToken = default);
    Task<List<VendorModel>> ListActiveVendorsAsync(CancellationToken cancellationToken = default);
    Task<long> AddVendorAsync(VendorModel vendor, CancellationToken cancellationToken = default);
    Task UpdateVendorAsync(VendorModel vendor, CancellationToken cancellationToken = default);
    Task<bool> VendorExistsAsync(string placeId, CancellationToken cancellationToken = default);
}

public sealed class VendorService : IVendorService
{
    private readonly AppDbContext _context;

    public VendorService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<VendorModel?> GetVendorByPlaceIdAsync(string placeId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(placeId)) return null;
        var vendor = await _context.Set<VendorModel>().AsNoTracking().Where(v => v.IsActive).FirstOrDefaultAsync(v => v.PlaceId == placeId, cancellationToken);

        return vendor;
    }

    public async Task<List<VendorModel>> ListActiveVendorsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Set<VendorModel>().AsNoTracking().Where(v => v.IsActive).OrderBy(v => v.Name).ToListAsync(cancellationToken);
    }

    public async Task<long> AddVendorAsync(VendorModel vendor, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(vendor);

        vendor.CreatedAt = DateTimeOffset.UtcNow;
        vendor.UpdatedAt = DateTimeOffset.UtcNow;
        _context.Set<VendorModel>().Add(vendor);
        await _context.SaveChangesAsync(cancellationToken);
        return vendor.Id;
    }

    public async Task UpdateVendorAsync(VendorModel vendor, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(vendor);
        vendor.UpdatedAt = DateTimeOffset.UtcNow;
        _context.Set<VendorModel>().Update(vendor);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> VendorExistsAsync(string placeId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(placeId)) return false;
        return await _context.Set<VendorModel>().AsNoTracking().AnyAsync(v => v.PlaceId == placeId, cancellationToken);
    }
}