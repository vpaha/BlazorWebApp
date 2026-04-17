using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;

public class BaseService
{
    protected readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public BaseService()
    {
    }

    public void InvalidateCache()
    {
        // Placeholder for future caching strategy
    }

    protected async Task HandleErrorAsync(HttpResponseMessage response, CancellationToken ct)
    {
        if (response.StatusCode == HttpStatusCode.Conflict)
        {
            var conflicts = await response.Content.ReadFromJsonAsync<ProcessResult>(JsonOptions, ct);
            throw new ConflictException(conflicts);
        }
        var body = await response.Content.ReadAsStringAsync(ct);
        throw new HttpRequestException($"Request failed: {(int)response.StatusCode} ({response.ReasonPhrase}). Body: {body}");
    }

    protected HttpRequestMessage CreateAuthorizedRequest(HttpMethod method, string url, string accessToken)
    {
        var request = new HttpRequestMessage(method, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        return request;
    }

    protected void ReplaceClaim(ClaimsIdentity identity, string claimType, string value)
    {
        var existingClaims = identity.FindAll(claimType).ToList();
        foreach (var existingClaim in existingClaims)
        {
            identity.RemoveClaim(existingClaim);
        }
        identity.AddClaim(new Claim(claimType, value));
    }
}