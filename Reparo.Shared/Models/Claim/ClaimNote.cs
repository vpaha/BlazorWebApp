public class ClaimNote
{
    public required string ClaimId { get; set; }
    public required string NoteTypeId { get; set; }
    public required string NoteType { get; set; }
    public required string NoteText { get; set; }
    public int? NoteLine { get; set; }
    public int? Line { get; set; }
    public int? StepNumber { get; set; }
    public required string UserName { get; set; }
    public DateTime? CreatedDate { get; set; }
    public bool Expand { get; set; }
}