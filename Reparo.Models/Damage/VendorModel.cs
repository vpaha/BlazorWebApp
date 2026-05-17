using System.ComponentModel.DataAnnotations;

public sealed class VendorModel
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = default!;

    public string? PlaceId { get; set; }
    public string? GoogleMaps { get; set; }

    // Address
    public string? Address { get; set; }
    public string? AddressLine1 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }

    // Geo
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    public string? Status { get; set; }

    public string? PrimaryType { get; set; }
    public string? PrimaryTypeDisplayName { get; set; }
    public string[]? Types { get; set; }

    // Operational status
    public bool IsActive { get; set; } = true;
    public bool IsVerified { get; set; } = false;
    public bool IsPreferred { get; set; } = false;

    // Classification
    public string? LicenseNumber { get; set; }

    // Audit
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}