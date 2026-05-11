using System.Text.Json.Serialization;

public class ClaimSearchResponse : ClaimResponseBase
{
    public string ClaimId { get; set; }
    public string ClaimType { get; set; }
    public string FormType { get; set; }
    public string ProvId { get; set; }
    public string ProvName { get; set; }
    public string MemId { get; set; }
    public string MemberName { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? TotalAmt { get; set; }
    public int? EditCount { get; set; }
    public string PayToProvid { get; set; }
    public string PayToProvName { get; set; }
    public DateTime? DOB { get; set; }
    public string Sex { get; set; }
    public string SubscriberId { get; set; }
    public DateTime? CleanDate { get; set; }
    public string ControlNmb { get; set; }
    public string BillType { get; set; }
    public string IsLtc { get; set; }
    public DateTime? LogDate { get; set; }
    public DateTime? PaidDate { get; set; }
    public string DCN { get; set; }
    public decimal? TotalPaid { get; set; }
    public decimal? MemberOwes { get; set; }
    public string EnrollId { get; set; }
    public string Status { get; set; }
    public string HasAccess { get; set; }
    public string RestrictedAccessGroups { get; set; }
    public string IsVip { get; set; }
    public string HealthPlanId { get; set; }
    public string RecordType { get; set; }
    public string IsReserved { get; set; }
    public string IsProcessRestricted { get; set; }
    public string IsItsClaim { get; set; }
}

public class ClaimResponseBase : ModelBase
{
    public List<ClaimNote> Notes { get; set; }
    public int? Line { get; set; }
    public bool IsAdded { get; set; } = false;
    public bool IsRouted { get; set; } = false;
}

public abstract class ModelBase
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EntityState EntityState { get; set; }
}

public enum EntityState
{
    ByDefault,
    NewlyAdded,
    Modified,
    Deleted
}
