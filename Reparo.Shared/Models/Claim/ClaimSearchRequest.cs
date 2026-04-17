using Reparo.Shared.Models.Resource;
using System.ComponentModel.DataAnnotations;

[AtLeastOneRequiredAttribute(["ClaimId", "MemId", "ProvId"], ErrorMessageResourceName = "DataForm_AtLeastOneProperty_Error", ErrorMessageResourceType = typeof(ModelResource))]
public class ClaimSearchRequest
{
    public string ClaimId { get; set; }
    public string FormType { get; set; }
    public string MemId { get; set; }
    [RegularExpression(@"^.{2,}$", ErrorMessageResourceName = "DataForm_Name_Error", ErrorMessageResourceType = typeof(ModelResource))]
    public string MemberName { get; set; }
    public string EnrollId { get; set; }
    public string Status { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public string AccessGroupControl { get; set; }
    public string ProvId { get; set; }
    public string ProviderName { get; set; }
    public string ProvType { get; set; }
    public string RecordType { get; set; }
    public string IsProcessRestricted { get; set; }
    public string ProcessRestrictionReasonId { get; set; }

    public int? Skip { get; set; }
    public int? Take { get; set; }
    public ClaimSearchRequest()
    {
        ClaimId = string.Empty;
        FormType = string.Empty;
        MemId = string.Empty;
        MemberName = string.Empty;
        EnrollId = string.Empty;
        Status = string.Empty;
        AccessGroupControl = string.Empty;
        ProvId = string.Empty;
        ProviderName = string.Empty;
        ProvType = string.Empty;
        RecordType = string.Empty;
        IsProcessRestricted = string.Empty;
        ProcessRestrictionReasonId = string.Empty;
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!string.IsNullOrWhiteSpace(ClaimId) && ClaimId.Length < 10)
        {
            yield return new ValidationResult("ID must be at least 10 characters when provided", [nameof(ClaimId)]);
        }
    }
}