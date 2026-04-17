using Microsoft.AspNetCore.Authentication;
using System.Net;
using System.Net.Http.Headers;

public sealed class TokenHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string _envId;

    public TokenHandler(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        _envId = configuration["UserEnvironment:EnvId"]
                 ?? throw new InvalidOperationException("UserEnvironment:EnvId is not configured.");
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        var httpContext = _httpContextAccessor.HttpContext
                          ?? throw new InvalidOperationException("HttpContext is not available.");

        if (httpContext.User?.Identity?.IsAuthenticated != true)
            return new HttpResponseMessage(HttpStatusCode.Unauthorized);

        var accessToken = await httpContext.GetTokenAsync("access_token");
        if (!string.IsNullOrWhiteSpace(accessToken))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        request.Headers.TryAddWithoutValidation("X-TZ-EnvId", _envId);

        return await base.SendAsync(request, ct);
    }
}