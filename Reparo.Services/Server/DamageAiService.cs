using Microsoft.Extensions.AI;
using System.Text.Json;

public sealed class DamageAiService : IDamageAiService
{
    private readonly IChatClient _chatClient;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private const string ReviewPrompt = "Review and correct the grammar of this text. Keep the original logic and meaning unchanged.";
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
    private const string ReviewPromptJson =
    """
    Review and correct grammar for each item.
    Keep the original logic and meaning unchanged.
    Return JSON only using this schema:
    [
        { "index": 0, "text": "" }
    ]
    """;

    private ChatOptions options { get; set; } = default!;

    public DamageAiService(IChatClient chatClient)
    {
        _chatClient = chatClient;
        options = new()
        {
            MaxOutputTokens = 800,
            Temperature = 0.1f
        };
    }

    public async Task<string> ProcessTextAsync(string entry, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entry);
        List<ChatMessage> messages = [new(ChatRole.User, ReviewPrompt), new(ChatRole.User, $"Text:\n{entry}")];
        ChatResponse response = await _chatClient.GetResponseAsync(messages, options, cancellationToken);
        return response.Text;
    }

    public async Task ProcessEntryAsync(DamageEntry entry, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entry);

        var input = entry.BuildCustomerInfo();

        if (!string.IsNullOrWhiteSpace(input))
        {
            List<ChatMessage> messages = [new(ChatRole.User, ExtractionPrompt), new(ChatRole.User, $"Text:\n{input}")];
            ChatResponse response = await _chatClient.GetResponseAsync(messages, options, cancellationToken);
            var result = JsonSerializer.Deserialize<DamageExtractionResult>(response.Text, JsonOptions);

            if (result is not null)
            {
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
            }
        }

        var sections = entry.Sections.Where(x => !string.IsNullOrWhiteSpace(x.Entry)).ToList();
        if (sections.Count > 0)
        {
            var sectionInput = JsonSerializer.Serialize(sections.Select((section, index) => new { index, text = section.Entry }));
            List<ChatMessage> messages = [new(ChatRole.User, ReviewPromptJson), new(ChatRole.User, sectionInput)
            ];

            ChatResponse response = await _chatClient.GetResponseAsync(messages, options, cancellationToken);
            var reviewedSections = JsonSerializer.Deserialize<List<ReviewedSectionResult>>(response.Text, JsonOptions);
            if (reviewedSections is not null)
            {
                foreach (var reviewed in reviewedSections)
                {
                    if (reviewed.Index >= 0 && reviewed.Index < sections.Count)
                    {
                        sections[reviewed.Index].Entry = reviewed.Text?.Trim();
                    }
                }
            }
        }

        entry.UpdatedAt = DateTime.UtcNow;
        entry.AIReviewCompleted = true;
    }

    private sealed class ReviewedSectionResult
    {
        public int Index { get; set; }
        public string? Text { get; set; }
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