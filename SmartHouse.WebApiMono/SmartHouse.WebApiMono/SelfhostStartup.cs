using System;
using System.IO;
using System.Reflection;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
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

			config.MapHttpAttributeRoutes();
			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);

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
						c.SingleApiVersion("1.0.0", "Smart House REST API")
						.Description("Web api for Smart house")
						.Contact(co => co
							.Name("Filip Krišto")
							.Url("https://twitter.com/filipkristo")
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
