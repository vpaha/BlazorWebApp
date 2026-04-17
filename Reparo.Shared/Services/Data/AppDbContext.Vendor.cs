using Microsoft.EntityFrameworkCore;

internal static class VendorModelBuilderExtensions
{
    internal static ModelBuilder ConfigureVendorDomain(this ModelBuilder modelBuilder, string schema)
    {
        modelBuilder.Entity<VendorModel>(e =>
        {
            e.ToTable("vendors", schema);

            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();

            // Business identity
            e.Property(x => x.Name).HasColumnName("name").IsRequired();
            e.Property(x => x.LegalName).HasColumnName("legal_name");
            e.Property(x => x.Description).HasColumnName("description");

            // External reference
            e.Property(x => x.PlaceId).HasColumnName("place_id");

            // Contact info
            e.Property(x => x.Email).HasColumnName("email");
            e.Property(x => x.Phone).HasColumnName("phone");
            e.Property(x => x.WebsiteUrl).HasColumnName("website_url");

            // Address
            e.Property(x => x.AddressLine1).HasColumnName("address_line1");
            e.Property(x => x.AddressLine2).HasColumnName("address_line2");
            e.Property(x => x.City).HasColumnName("city");
            e.Property(x => x.State).HasColumnName("state");
            e.Property(x => x.PostalCode).HasColumnName("postal_code");
            e.Property(x => x.Country).HasColumnName("country");

            // Geo
            e.Property(x => x.Latitude).HasColumnName("latitude");
            e.Property(x => x.Longitude).HasColumnName("longitude");

            // Classification
            e.Property(x => x.LicenseNumber).HasColumnName("license_number");

            // Operational status
            e.Property(x => x.IsActive).HasColumnName("is_active");
            e.Property(x => x.IsVerified).HasColumnName("is_verified");
            e.Property(x => x.IsPreferred).HasColumnName("is_preferred");

            // Ratings
            e.Property(x => x.Rating).HasColumnName("rating");
            e.Property(x => x.ReviewCount).HasColumnName("review_count");

            // Audit
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
            e.Property(x => x.UpdatedAt).HasColumnName("updated_at");

            // Recommended index mapping (optional but consistent with DB)
            e.HasIndex(x => x.PlaceId).IsUnique();
        });

        return modelBuilder;
    }
}