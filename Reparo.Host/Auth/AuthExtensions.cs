using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

public static class AuthExtensions
{
    public static IServiceCollection AddHostAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var appBasePath = configuration["AppBasePath"] ?? "/";

        services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
            {
                o.Cookie.Path = appBasePath;
                o.Cookie.Name = ".Pavel.Auth";
                o.AccessDeniedPath = "/accessdenied";
            })
            .AddGoogle(options =>
            {
                var oauth = configuration.GetSection("UserEnvironment:Google");
                options.ClientId = oauth["Client_Id"] ?? string.Empty;

                var clientSecret = oauth["ClientSecret"];
                if (!string.IsNullOrWhiteSpace(clientSecret)) options.ClientSecret = clientSecret;

                options.Scope.Add("email");
                options.Scope.Add("profile");

                options.SaveTokens = true;
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddTwitter(options =>
            {
                var oauth = configuration.GetSection("UserEnvironment:Twitter");

                options.ConsumerKey = oauth["ConsumerAPIKey"];
                options.ConsumerSecret = oauth["ConsumerSecret"];
                options.CallbackPath = "/signin-twitter";

                options.SaveTokens = true;
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddFacebook(options =>
            {
                var oauth = configuration.GetSection("UserEnvironment:Facebook");
                options.AppId = oauth["AppId"] ?? string.Empty;

                var appSecret = oauth["AppSecret"];
                if (!string.IsNullOrWhiteSpace(appSecret)) options.AppSecret = appSecret;

                options.AccessDeniedPath = "/accessdenied";
                options.CallbackPath = "/signin-facebook";

                options.SaveTokens = true;
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                var oauth = configuration.GetSection("UserEnvironment:OidcClientOptions");
                var authority = oauth["Authority"] ?? string.Empty;
                var clientId = oauth["Client_Id"] ?? string.Empty;
                var clientSecret = oauth["ClientSecret"];
                var extraScope = oauth["Scope"];

                options.PushedAuthorizationBehavior = PushedAuthorizationBehavior.Disable;

                options.Authority = authority;
                options.ClientId = clientId;
                if (!string.IsNullOrWhiteSpace(clientSecret))
                    options.ClientSecret = clientSecret;

                options.ResponseType = "code";
                options.AccessDeniedPath = "/accessdenied";
                options.CallbackPath = "/signin-oidc";

                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                // recommended defaults for OIDC
                AddScopes(options, extraScope);

                options.SaveTokens = true;
                options.MapInboundClaims = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = "role"
                };

                options.Events = new OpenIdConnectEvents
                {
                    OnTokenValidated = async ctx =>
                    {
                        var principal = ctx.Principal;
                        //var accessToken = ctx.TokenEndpointResponse?.AccessToken;

                        if (principal is null) throw new InvalidOperationException("Principal is not available.");
                        //if (string.IsNullOrWhiteSpace(accessToken)) throw new InvalidOperationException("Access token is not available.");

                        var scopeFactory = ctx.HttpContext.RequestServices.GetRequiredService<IServiceScopeFactory>();
                        await using var scope = scopeFactory.CreateAsyncScope();

                        var provisioner = scope.ServiceProvider.GetRequiredService<IUserService>();
                        await provisioner.ProvisionAsync(principal, ctx.HttpContext.RequestAborted);
                    }
                };
            });
        return services;
    }

    private static void AddScopes(OpenIdConnectOptions options, string? extraScopes, params string[] defaults)
    {
        var scopes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        if (defaults is { Length: > 0 })
        {
            foreach (var scope in defaults)
            {
                if (!string.IsNullOrWhiteSpace(scope)) scopes.Add(scope);
            }
        }
        if (!string.IsNullOrWhiteSpace(extraScopes))
        {
            foreach (var scope in extraScopes.Split([' ', ','], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                scopes.Add(scope);
            }
        }
        foreach (var scope in scopes)
        {
            options.Scope.Add(scope);
        }
    }
}