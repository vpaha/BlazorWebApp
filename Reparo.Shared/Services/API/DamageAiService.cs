using Microsoft.Extensions.AI;
using System.Text.Json;

public interface IDamageAiService
{
    Task ProcessEntryAsync(DamageEntry entry, CancellationToken cancellationToken = default);
}

public sealed class DamageAiService : IDamageAiService
{
    private readonly IChatClient _chatClient;

    public DamageAiService(IChatClient chatClient)
    {
        _chatClient = chatClient;
    }

    public async Task ProcessEntryAsync(DamageEntry entry, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(entry.AddressEntry)) return;

        ChatOptions options = new()
        {
            MaxOutputTokens = 200,
            Temperature = 0.1f
        };

        List<ChatMessage> messages =
        [
            new(ChatRole.System,
                """
                Extract the address information from the user text.

                Return JSON only using this schema:
                {
                  "street": "",
                  "city": "",
                  "state": "",
                  "zip": ""
                }
                """),

            new(ChatRole.User, $"Text:\n{entry.AddressEntry}")
        ];

        ChatResponse response = await _chatClient.GetResponseAsync(
            messages,
            options,
            cancellationToken);

        var result = JsonSerializer.Deserialize<AddressExtractionResult>(
            response.Text,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        if (result is null) return;

        entry.Street = result.Street;
        entry.City = result.City;
        entry.State = result.State;
        entry.Zip = result.Zip;
    }

    private sealed class AddressExtractionResult
    {
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
    }
}