public class ClaimStatusResponse
{
    public string? ClaimId { get; set; }

    public string? OrgClaimId { get; set; }

    public string? IncidentId { get; set; }

    public int? LockedByUser { get; set; }
    public string? LockedByUserName { get; set; }

    public string? RoutingStatus { get; set; }
    public string? AuditStatus { get; set; }

    public string? AuditInstruction { get; set; }

    public string? CreateId { get; set; }

    public string? UpdateId { get; set; }
}