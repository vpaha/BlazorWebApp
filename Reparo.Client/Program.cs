using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Syncfusion.Blazor;
using Syncfusion.Licensing;

internal class Program
{
    static async Task Main(string[] args)
    {
        SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1JFaF5cXGRCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWH5cd3RQRmhZVUJ/XEtWYEA=");

        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        builder.Services.AddSyncfusionBlazor(options => { options.Animation = GlobalAnimationMode.Enable; options.EnableRippleEffect = true; });

        builder.Services.AddAuthorizationCore();
        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddAuthenticationStateDeserialization();

        await builder.Build().RunAsync();
    }
}