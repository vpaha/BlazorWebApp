using System.Security.Claims;

public interface IImageService
{
    Task UploadAsync(int damageId, string fileName, string contentType, Stream stream);
    Task DeleteAsync(int damageId, string fileName);
    Task<List<ImageItem>> ListAsync(int damageId);
}

public interface IClaimService
{
    Task<ModelCollection<ClaimSearchResponse>> SearchAsync(ClaimSearchFilter request, CancellationToken ct = default);
}

public interface IUserService
{
    Task ProvisionAsync(ClaimsPrincipal principal, CancellationToken ct = default);

    Task<IReadOnlyList<AppUser>> GetUsersAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AppRole>> GetRolesAsync(CancellationToken ct = default);
    Task UpdateUserAsync(AppUser user, CancellationToken ct = default);
}

public interface IDamageAiService
{
    Task ProcessEntryAsync(DamageEntry entry, CancellationToken cancellationToken = default);
    Task<string> ProcessTextAsync(string entry, CancellationToken cancellationToken = default);
}
