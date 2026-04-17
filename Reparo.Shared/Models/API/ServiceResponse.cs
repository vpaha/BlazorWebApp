using System.Net;

public class ProcessMessage
{
    public string MessageId { get; set; }
    public string Message { get; set; }
    public MessageSeverity Severity { get; set; }
    public MessageCategory Category { get; set; }
    public MessageSource Source { get; set; }
    public Dictionary<string, object> AdditionalInfo { get; set; }
}

public class MultiStatusResponse
{
    public HttpStatusCode Status { get; set; }
    public Dictionary<string, object> PrimaryIdentifier { get; set; }
    public List<ProcessMessage> ProcessMessages { get; set; }
}

public class ProcessMetadata
{
    public string ConversationId { get; set; }
    public string TransactionId { get; set; }
    public List<ProcessMessage> ProcessMessages { get; set; }
    public List<MultiStatusResponse> MultiStatusResponses { get; set; }
}


public class ProcessResult
{
    public Exception Exception { get; set; }
    public ProcessMetadata ProcessMetadata { get; set; }
}

public sealed class ConflictException : System.Exception
{
    public IReadOnlyList<ProcessMessage> Conflicts { get; }
    public ConflictException(ProcessResult? conflicts, string? message = null) : base(message ?? "One or more conflicts occurred.")
    {
        Conflicts = conflicts?.ProcessMetadata.ProcessMessages ?? [];
    }
}