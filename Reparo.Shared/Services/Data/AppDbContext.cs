using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public sealed partial class AppDbContext
    : IdentityDbContext<AppUser, AppRole, int>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Domain tables only
    public DbSet<DamageEntry> DamageEntries => Set<DamageEntry>();
    public DbSet<DamageEntrySection> DamageEntrySections => Set<DamageEntrySection>();
    public DbSet<DamageSectionType> DamageSectionTypes => Set<DamageSectionType>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .ConfigureIdentityDomain("public")
            .ConfigureDamageDomain("public")
            .ConfigureVendorDomain("public");
    }
}