using System;
using System.IO;
using System.Reflection;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using SmartHouse.Lib;
using Swashbuckle.Application;
using System.Web.Http.Routing;

namespace SmartHouse.WebApiMono
{
    public class WebApiConfig
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();

            EnableCors(appBuilder);            

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );            

            SetupDI(config);

            //config.MessageHandlers.Add(new MessageLoggingHandler());
            config.Filters.Add(new ExceptionFilter());

            var formatters = config.Formatters;
            var jsonFormatter = formatters.JsonFormatter;
            var settings = jsonFormatter.SerializerSettings;
            settings.Formatting = Formatting.Indented;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();                        

            SetupSwagger(config);

            appBuilder.MapSignalR();

            config.EnsureInitialized();
            appBuilder.UseWebApi(config);
        }

        private void SetupDI(HttpConfiguration config)
        {
            var unity = new UnityContainer();

            unity.RegisterType<ISettingsService, SettingService>(new HierarchicalLifetimeManager());
            unity.RegisterType<IPanodraService, PandoraService>(new HierarchicalLifetimeManager());
            unity.RegisterType<IYamahaService, YamahaService>(new HierarchicalLifetimeManager());
            unity.RegisterType<ISmartHouseService, SmartHouseService>(new HierarchicalLifetimeManager());
            unity.RegisterType<ITelemetryService, TelemetryService>(new HierarchicalLifetimeManager());
            unity.RegisterType<IMPDService, MPDService>(new HierarchicalLifetimeManager());
            unity.RegisterType<ILastFMService, LastFMService>(new HierarchicalLifetimeManager());
            unity.RegisterType<ITVService, TVService>(new HierarchicalLifetimeManager());
            unity.RegisterType<IOrviboService, OrviboService>(new HierarchicalLifetimeManager());
            unity.RegisterType<ILyricsService, LyricsService>(new HierarchicalLifetimeManager());

            config.DependencyResolver = new UnityResolver(unity);
        }

        private void EnableCors(IAppBuilder appBuilder)
        {
            appBuilder.Use(async (context, next) =>
            {
                var serveri = new[] { "*", "http://localhost" };

                IOwinRequest req = context.Request;
                IOwinResponse res = context.Response;

                var origin = req.Headers.Get("Origin");

                if (!String.IsNullOrWhiteSpace(origin) && !res.Headers.ContainsKey("Access-Control-Allow-Origin"))
                {
                    res.Headers.Set("Access-Control-Allow-Origin", origin);
                    res.Headers.AppendCommaSeparatedValues("Access-Control-Allow-Credentials", "true");
                }

                if (req.Method == "OPTIONS")
                {
                    res.StatusCode = 200;
                    res.Headers.AppendCommaSeparatedValues("Access-Control-Allow-Methods", "GET", "POST", "PUT", "DELETE");
                    res.Headers.AppendCommaSeparatedValues("Access-Control-Allow-Headers", "authorization", "content-type");
                    return;
                }

                await next();
            });
        }


        private void SetupSwagger(HttpConfiguration config)
        {
            config.EnableSwagger(c =>
                                {
                                    c.SingleApiVersion("1.0.0", "Smart House - REST API")
                                     .Description("Open Source web api for Smart House running on .NET framework(Mono compatibile)")
                                    .Contact(co => co
                                        .Name("Filip Krišto")
                                        .Url("https://github.com/filipkristo")
                                        .Email("filipkristo@outlook.com"));

                                }
                            )
                        .EnableSwaggerUi();
        }
    }
}
