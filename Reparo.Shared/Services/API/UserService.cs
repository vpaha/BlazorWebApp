using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

public interface IUserService
{
    Task ProvisionAsync(ClaimsPrincipal principal, CancellationToken ct = default);
    Task<IReadOnlyList<AppUser>> GetUsersAsync(CancellationToken cancellationToken = default);
}

public sealed class UserService : BaseService, IUserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context) : base()
    {
        _context = context;
    }

    public async Task ProvisionAsync(ClaimsPrincipal principal, CancellationToken ct = default)
    {
        if (principal?.Identity?.IsAuthenticated != true) return;

        var providerKey = principal.FindFirst("sub")?.Value ?? principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException("Missing provider user id claim.");
        var loginProvider = principal.Identity?.AuthenticationType ?? principal.FindFirst("iss")?.Value ?? "external";

        var userId = await _context.UserLogins.AsNoTracking().Where(x => x.LoginProvider == loginProvider &&
                        x.ProviderKey == providerKey).Select(x => (int?)x.UserId)
            .SingleOrDefaultAsync(ct) ?? throw new InvalidOperationException("Authenticated user could not be resolved.");

        var roleNames = await _context.UserRoles.AsNoTracking().Where(ur => ur.UserId == userId)
            .Join(_context.Roles.AsNoTracking(), ur => ur.RoleId, r => r.Id, (_, r) => r.Name!)
            .ToListAsync(ct);

        if (principal.Identity is ClaimsIdentity identity)
        {
            foreach (var claim in identity.FindAll(identity.RoleClaimType).ToList()) identity.RemoveClaim(claim);
            foreach (var role in roleNames) identity.AddClaim(new Claim(identity.RoleClaimType, role));
        }
    }

    public async Task<IReadOnlyList<AppUser>> GetUsersAsync(CancellationToken ct = default)
    {
        return await _context.Users.AsNoTracking().ToListAsync(ct);
    }
}