using Microsoft.Extensions.AI;

public interface IDamageAiService
{
    Task<string?> ProcessEntryAsync(DamageEntry entry, CancellationToken cancellationToken = default);
}

public sealed class DamageAiService : IDamageAiService
{
    private readonly IChatClient _chatClient;

    public DamageAiService(IChatClient chatClient)
    {
        _chatClient = chatClient;
    }

    public async Task<string?> ProcessEntryAsync(
        DamageEntry entry,
        CancellationToken cancellationToken = default)
    {
        var text = entry.BuildCombinedDescription();

        if (string.IsNullOrWhiteSpace(text))
            return null;

        var response = await _chatClient.GetResponseAsync(
            $"""
            Extract structured property damage information from this text.

            Text:
            {text}
            """,
            cancellationToken: cancellationToken);

        return response.Text;
    }
}