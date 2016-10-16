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

namespace SmartHouse.WebApiMono
{
	public class SelfhostStartup
	{
		// This code configures Web API. The Startup class is specified as a type
		// parameter in the WebApp.Start method.
		public void Configuration(IAppBuilder appBuilder)
		{
			var config = new HttpConfiguration();

			appBuilder.Use(async (context, next) =>
			{
				var serveri = new[] { "*", "http://localhost" };

				IOwinRequest req = context.Request;
				IOwinResponse res = context.Response;

				var origin = req.Headers.Get("Origin");

				if (!String.IsNullOrWhiteSpace(origin) && !res.Headers.ContainsKey("Access-Control-Allow-Origin"))
				{
					res.Headers.Set("Access-Control-Allow-Origin", origin);
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

			config.MapHttpAttributeRoutes();
			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);

			var unity = new UnityContainer();
			unity.RegisterType<BaseController>();
			unity.RegisterType<PandoraController>();
			unity.RegisterType<YamahaController>();
			unity.RegisterType<SmartHouseController>();
			unity.RegisterType<UtilController>();
			unity.RegisterType<SettingsController>();

			unity.RegisterType<ISettingsService, SettingService>(new HierarchicalLifetimeManager());
			unity.RegisterType<IPanodraService, PandoraService>(new HierarchicalLifetimeManager());
			unity.RegisterType<IYamahaService, YamahaService>(new HierarchicalLifetimeManager());

			config.DependencyResolver = new UnityResolver(unity);

			config.MessageHandlers.Add(new MessageLoggingHandler());
			config.Filters.Add(new ExceptionFilter());

			var formatters = config.Formatters;
			var jsonFormatter = formatters.JsonFormatter;
			var settings = jsonFormatter.SerializerSettings;
			settings.Formatting = Formatting.Indented;
			settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

			config.EnsureInitialized();

			config.EnableSwagger(c =>
					{
						c.SingleApiVersion("1.0.0", "Smart House - REST API")
				         .Description("Open Source web api for Smart House running on mono framework")
						.Contact(co => co
							.Name("Filip Krišto")
							.Url("https://github.com/filipkristo")
							.Email("filipkristo@outlook.com"));
				                 						
					}
				)
			.EnableSwaggerUi();
			// Swagger configuration end

			appBuilder.UseWebApi(config);
		}

		private string GetXMLCommentsFile()
		{
			var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
			var commentsFileName = Assembly.GetExecutingAssembly().GetName().Name + ".XML";
			var commentsFile = Path.Combine(baseDirectory, commentsFileName);
			return commentsFile;
		}

	}
}
