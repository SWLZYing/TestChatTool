using Autofac.Integration.WebApi;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Owin;
using System.Web.Http;
using TestChatTool.Service;
using TestChatTool.Service.Applibs;

[assembly: Microsoft.Owin.OwinStartup(typeof(Startup))]

namespace TestChatTool.Service
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = WebApiConfig();

            app.UseWebApi(config);
            app.UseCors(CorsOptions.AllowAll);

            GlobalHost.Configuration.MaxIncomingWebSocketMessageSize = 500;
            GlobalHost.Configuration.DefaultMessageBufferSize = 32;

            app.MapSignalR();
        }

        private HttpConfiguration WebApiConfig()
        {
            var config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional });

            // API DI設定
            config.DependencyResolver = new AutofacWebApiDependencyResolver(AutofacConfig.Container);

            return config;
        }
    }
}