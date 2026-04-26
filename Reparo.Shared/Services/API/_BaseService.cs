using Microsoft.AspNetCore.Components.Authorization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

public class BaseService
{
    protected readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly AuthenticationStateProvider _authProvider;

    public BaseService(AuthenticationStateProvider authProvider)
    {
        _authProvider = authProvider;
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

    protected async Task<int?> ResolveUserIdAsync()
    {
        var authState = await _authProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        if (user.Identity?.IsAuthenticated == true)
        {
            var appUserId = user.FindFirst("app_user_id")?.Value;
            if (!string.IsNullOrWhiteSpace(appUserId) && int.TryParse(appUserId, out var parsedUserId)) return parsedUserId;
        }
        return null;
    }

    protected async Task<int?> ResolveVendorIdAsync()
    {
        var authState = await _authProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        if (user.Identity?.IsAuthenticated == true)
        {
            var vendorId = user.FindFirst("vendor_id")?.Value;
            if (!string.IsNullOrWhiteSpace(vendorId) && int.TryParse(vendorId, out var parsedVendorId)) return parsedVendorId;
        }
        return null;
    }
}