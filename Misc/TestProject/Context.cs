using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Syncfusion.Blazor;

namespace Reparo.TestProject
{
    public class Context
    {
        protected BunitContext ctx { get; set; }

        protected MockNavigationManager mockNavigationManager { get; set; } = new MockNavigationManager();
        protected Mock<ToastService> toastMock { get; set; } = new Mock<ToastService>();

        public Context()
        {
            ctx = new BunitContext();
            ctx.Services.AddSyncfusionBlazor();
            ctx.AddAuthorization();

            ctx.Services.AddSingleton<NavigationManager>(mockNavigationManager);

            ctx.Services.AddSingleton(new Mock<ToastService>().Object);
            ctx.Services.AddSingleton(toastMock.Object);

            ctx.JSInterop.Mode = JSRuntimeMode.Loose;

            //authContext.SetAuthenticationType(OpenIdConnectDefaults.AuthenticationScheme);
            //authContext.SetAuthorized("automation1");
            //authContext.SetRoles("superuser");
        }
        ~Context()
        {
            ctx.Dispose();
        }
    }

    public class MockNavigationManager : NavigationManager
    {
        public string? NavigatedTo { get; private set; }
        public bool ForceLoad { get; private set; }

        public MockNavigationManager(string baseUri = "https://localhost/", string uri = "https://localhost/")
        {
            Initialize(baseUri, uri);
        }

        protected override void NavigateToCore(string uri, bool forceLoad)
        {
            NavigatedTo = uri;
            ForceLoad = forceLoad;
            Uri = ToAbsoluteUri(uri).ToString();
        }
    }
}