using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using SmartHouseWeb.App_Start;

[assembly: OwinStartupAttribute(typeof(SmartHouseWeb.Startup))]
namespace SmartHouseWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            ConfigureOAuth(app);
            app.MapSignalR();
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            var OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = System.TimeSpan.FromDays(1),
                Provider = new SimpleAuthorizationServerProvider(),
                RefreshTokenProvider = new ApplicationRefreshTokenProvider()
            };            

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

        }
    }
}
