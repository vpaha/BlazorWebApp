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
            e.Property(x => x.Name).HasColumnName("name").HasMaxLength(200).IsRequired();
            e.Property(x => x.PlaceId).HasColumnName("place_id").HasMaxLength(255);

            e.Property(x => x.GoogleMaps).HasColumnName("googlemaps_url").HasMaxLength(500);
            e.Property(x => x.Address).HasColumnName("address");
            e.Property(x => x.AddressLine1).HasColumnName("address_line1").HasMaxLength(200);
            e.Property(x => x.City).HasColumnName("city").HasMaxLength(100);
            e.Property(x => x.State).HasColumnName("state").HasMaxLength(100);
            e.Property(x => x.PostalCode).HasColumnName("postal_code").HasMaxLength(20);
            e.Property(x => x.Country).HasColumnName("country").HasMaxLength(100).HasDefaultValue("US");
            e.Property(x => x.Latitude).HasColumnName("latitude").HasColumnType("double precision");
            e.Property(x => x.Longitude).HasColumnName("longitude").HasColumnType("double precision");
            e.Property(x => x.LicenseNumber).HasColumnName("license_number").HasMaxLength(100);
            e.Property(x => x.IsActive).HasColumnName("is_active").HasDefaultValue(true);
            e.Property(x => x.IsVerified).HasColumnName("is_verified").HasDefaultValue(false);
            e.Property(x => x.IsPreferred).HasColumnName("is_preferred").HasDefaultValue(false);
            e.Property(x => x.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("now()");
            e.Property(x => x.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("now()");
            e.Property(x => x.Status).HasColumnName("status");
            e.Property(x => x.PrimaryType).HasColumnName("primary_type");
            e.Property(x => x.PrimaryTypeDisplayName).HasColumnName("primary_type_display_name");
            e.Property(x => x.TravelToYou).HasColumnName("travel_to_you");
            e.Property(x => x.Types).HasColumnName("types");

            e.HasIndex(x => x.PlaceId).IsUnique().HasDatabaseName("uq_vendors_place_id");
        });

        return modelBuilder;
    }
}