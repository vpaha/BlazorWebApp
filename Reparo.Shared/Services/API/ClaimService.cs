using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Json;

public interface IClaimService
{
    Task<ModelCollection<ClaimSearchResponse>> SearchAsync(ClaimSearchFilter request, CancellationToken ct = default);
}

public sealed class ClaimService : BaseService, IClaimService
{
    private readonly HttpClient _httpClient;
    public ClaimService(HttpClient http) : base()
    {
        _httpClient = http;
    }

    public async Task<ModelCollection<ClaimSearchResponse>> SearchAsync(ClaimSearchFilter request, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        var url = BuildSearchUrl(request);
        using var response = await _httpClient.GetAsync(url, ct);
        if (response.IsSuccessStatusCode)
        {
            return await ReadResultAsync(response, ct);
        }
        await HandleErrorAsync(response, ct);
        // unreachable, but compiler requires return
        throw new InvalidOperationException("Unexpected execution path.");
    }

    private string BuildSearchUrl(ClaimSearchFilter request)
    {
        var query = new Dictionary<string, string?>
        {
            ["skip"] = request.Skip.ToString(),
            ["take"] = request.Take.ToString()
        };

        if (!string.IsNullOrWhiteSpace(request.ClaimId)) query["claimId"] = request.ClaimId;
        if (!string.IsNullOrWhiteSpace(request.ProvId)) query["provId"] = request.ProvId;
//        if (!string.IsNullOrWhiteSpace(request.MemId)) query["memId"] = request.MemId;

        return QueryHelpers.AddQueryString("claims/search", query);
    }

    private async Task<ModelCollection<ClaimSearchResponse>> ReadResultAsync(HttpResponseMessage response, CancellationToken ct)
    {
        var result = await response.Content.ReadFromJsonAsync<ModelCollection<ClaimSearchResponse>>(JsonOptions, ct);
        return result ?? new ModelCollection<ClaimSearchResponse>
        {
            Results = Array.Empty<ClaimSearchResponse>()
        };
    }
}