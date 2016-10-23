using System;
using System.Threading.Tasks;
using System.Web.Http;
using SmartHouse.Lib;

namespace SmartHouse.WebApiMono
{
	[RoutePrefix("api/Sensor")]
	public class SensorController : BaseController
	{
		private readonly ITelemetryService TelemetryService;

		public SensorController(ISettingsService settingsService, ITelemetryService telemetryService) : base(settingsService)
		{
			this.TelemetryService = telemetryService;
		}

		[HttpPost]
		[Route("SaveTemperature")]
		public async Task<Result> SaveTemperature(TemperatureData data)
		{
			return await TelemetryService.SaveTemperature(data);
		}

		[HttpGet]
		[Route("GetCurrentTemperature")]
		public async Task<TemperatureData> GetCurrentTemperature()
		{
			return await TelemetryService.GetLastTemperature();
		}
	}
}
