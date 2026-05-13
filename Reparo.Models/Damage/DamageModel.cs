using System.ComponentModel.DataAnnotations;

public enum MessageSeverity
{
    Normal = 0,
    Success = 1,
    Info = 2,
    Warning = 3,
    Error = 4
}

public sealed class DamageEntrySection
{
    public int Id { get; set; }
    // FK → damage_entries.id
    public int DamageEntryId { get; set; }
    // FK → damage_section_types.id
    public int DamageSectionId { get; set; }

    public string? Entry { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public DamageSectionType? DamageSectionType { get; set; }
}

public sealed class DamageSectionType
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public bool IsEmergency { get; set; }
}

public class DamageEntry : IValidatableObject
{
    public int Id { get; set; }
    public DamageStatus StatusId { get; set; }
    public bool AIReviewCompleted { get; set; }
    
    [Required(ErrorMessage = "What's the property address? (Street, city, state, or ZIP code)")]
    public string? AddressEntry { get; set; }

    public string? Street { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Zip { get; set; }

    public string? Placename { get; set; }
    public string? Region { get; set; }

    public string? GoogleId { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    public string? InsuranceEntry { get; set; }

    public string? InsuranceCarrier { get; set; }
    public string? PolicyNumber { get; set; }
    public string? ClaimNumber { get; set; }

    [Required(ErrorMessage = "What's the best way to contact you?")]
    public string? ContactEntry { get; set; }

    public string? FullName { get; set; }
    public string? Phone { get; set; }
    [EmailAddress]
    public string? Email { get; set; }

    [DateGreaterThan(2000, 1, 1, ErrorMessage = "Date must be after 01/01/2000")]
    public DateOnly DateOfLoss { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public int? UserId { get; set; }
    public AppUser? User { get; set; }

    public int? VendorId { get; set; }
    public VendorModel? Vendor { get; set; }

    public List<DamageEntrySection> Sections { get; set; } = [];

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Sections.Count == 0 || Sections.All(x => string.IsNullOrWhiteSpace(x.Entry)))
        {
            yield return new ValidationResult(
                "Click a button to describe damage",
                [nameof(Sections)]);
        }
    }

    public string? BuildCustomerInfo()
    {
        var parts = new List<string>();

        if (!string.IsNullOrWhiteSpace(AddressEntry)) parts.Add(AddressEntry);
        if (!string.IsNullOrWhiteSpace(ContactEntry)) parts.Add(ContactEntry);
        if (!string.IsNullOrWhiteSpace(InsuranceEntry)) parts.Add(InsuranceEntry);

        if (parts.Any() == true) return string.Join(". ", parts);
        return null;
    }

    public string? BuildDamageInfo()
    {
        var parts = new List<string>();
        if (Sections.Count > 0 && Sections.Any(x => !string.IsNullOrWhiteSpace(x.Entry)))
        {
            parts.AddRange(
                Sections.Where(d => !string.IsNullOrWhiteSpace(d.Entry))
                    .Select(d => string.Join(".", new[] { d.Entry, d.DamageSectionType?.Name }
                    .Where(v => !string.IsNullOrWhiteSpace(v)))));
        }
        if (parts.Any() == true) return string.Join(". ", parts);
        return null;
    }

    public string GetStatusDescription()
    {
        return (DamageStatus)StatusId switch
        {
            DamageStatus.Reported => "Status: Reported",
            DamageStatus.VendorAssigned => "Status: Vendor assigned",
            DamageStatus.ServiceScheduled => "Status: Service scheduled",
            DamageStatus.WorkCompleted => "Status: Work completed",
            DamageStatus.Closed => "Status: Closed",
            DamageStatus.Cancelled => "Status: Canceled",
            _ => "Status: Unknown"
        };
    }

    public string GetStatusStyle()
    {
        return (DamageStatus)StatusId switch
        {
            DamageStatus.Reported => "e-primary",
            DamageStatus.VendorAssigned => "e-outline e-primary",
            DamageStatus.ServiceScheduled => "e-outline e-success",
            DamageStatus.WorkCompleted => "",
            DamageStatus.Closed => "e-outline e-info",
            DamageStatus.Cancelled => "e-outline e-warning",
            _ => "e-danger"
        };
    }
}

public sealed class DateGreaterThanAttribute : ValidationAttribute
{
    private readonly DateTimeOffset _minDate;

    public DateGreaterThanAttribute(int year, int month, int day)
    {
        _minDate = new DateTimeOffset(year, month, day, 0, 0, 0, TimeSpan.Zero);
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext context)
    {
        if (value is DateTimeOffset date && date <= _minDate)
        {
            return new ValidationResult(
                ErrorMessage ?? $"Date must be greater than {_minDate:MM/dd/yyyy}.");
        }

        return ValidationResult.Success;
    }
}