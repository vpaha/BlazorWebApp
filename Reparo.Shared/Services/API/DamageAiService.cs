using Microsoft.Extensions.AI;
using System.Text.Json;

public interface IDamageAiService
{
    Task ProcessEntryAsync(
        DamageEntry entry,
        CancellationToken cancellationToken = default);
}

public sealed class DamageAiService : IDamageAiService
{
    private readonly IChatClient _chatClient;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private const string ExtractionPrompt =
    """
    Extract structured property damage claim information from the user text.

    Return JSON only.

    Schema:
    {
      "street": "",
      "city": "",
      "state": "",
      "zip": "",
      "fullName": "",
      "phone": "",
      "email": "",
      "insuranceCarrier": "",
      "policyNumber": "",
      "claimNumber": ""
    }

    Rules:
    - Do not invent values.
    - Use null when information is missing.
    - Return valid JSON only.
    """;

    public DamageAiService(IChatClient chatClient)
    {
        _chatClient = chatClient;
    }

    public async Task ProcessEntryAsync(
        DamageEntry entry,
        CancellationToken cancellationToken = default)
    {
        var input = entry.BuildCombinedDescription();

        if (string.IsNullOrWhiteSpace(input))
            return;

        ChatOptions options = new()
        {
            MaxOutputTokens = 400,
            Temperature = 0.1f
        };

        List<ChatMessage> messages =
        [
            new(ChatRole.System, ExtractionPrompt),
            new(ChatRole.User, $"Text:\n{input}")
        ];

        ChatResponse response = await _chatClient.GetResponseAsync(
            messages,
            options,
            cancellationToken);

        if (string.IsNullOrWhiteSpace(response.Text))
            return;

        var result = JsonSerializer.Deserialize<DamageExtractionResult>(
            response.Text,
            JsonOptions);

        if (result is null)
            return;

        entry.Street = result.Street;
        entry.City = result.City;
        entry.State = result.State;
        entry.Zip = result.Zip;

        entry.FullName = result.FullName;
        entry.Phone = result.Phone;
        entry.Email = result.Email;

        entry.InsuranceCarrier = result.InsuranceCarrier;
        entry.PolicyNumber = result.PolicyNumber;
        entry.ClaimNumber = result.ClaimNumber;

        entry.UpdatedAt = DateTimeOffset.UtcNow;
        entry.StatusId = DamageStatus.AIReviewCompleted;
    }

    private sealed class DamageExtractionResult
    {
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }

        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }

        public string? InsuranceCarrier { get; set; }
        public string? PolicyNumber { get; set; }
        public string? ClaimNumber { get; set; }
    }
}