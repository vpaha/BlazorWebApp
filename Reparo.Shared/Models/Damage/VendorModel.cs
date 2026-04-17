using System.ComponentModel.DataAnnotations;

public sealed class VendorModel
{
    public int Id { get; set; }

    // Business identity
    [Required]
    public string Name { get; set; } = default!;

    public string? LegalName { get; set; }

    public string? Description { get; set; }

    // External reference (Google Places)
    public string? PlaceId { get; set; }

    // Contact info
    [EmailAddress]
    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? WebsiteUrl { get; set; }

    // Address
    public string? AddressLine1 { get; set; }

    public string? AddressLine2 { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? PostalCode { get; set; }

    public string? Country { get; set; } = "US";

    // Geo
    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    // Classification
    public string? LicenseNumber { get; set; }

    // Operational status
    public bool IsActive { get; set; } = true;

    public bool IsVerified { get; set; } = false;

    public bool IsPreferred { get; set; } = false;

    // Ratings / metadata
    [Range(0, 5)]
    public decimal? Rating { get; set; }

    public int? ReviewCount { get; set; }

    // Audit
    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }
}