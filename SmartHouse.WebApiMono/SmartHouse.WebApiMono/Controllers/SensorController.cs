using System;
using System.Web.Http;
using SmartHouse.Lib;

namespace SmartHouse.WebApiMono
{
	[RoutePrefix("api/Sensor")]
	public class SensorController : BaseController
	{
		public SensorController(ISettingsService settingsService) : base(settingsService)
		{

		}

		[HttpPost]
		[Route("SaveTemperature")]
		public Result SaveTemperature(TemperatureData data)
		{
			MainClass.Log.Info($"Temperature: {data?.Temperature}");
			MainClass.Log.Info($"Humidity: {data?.Humidity}");

			return new Result()
			{
				ErrorCode = 0,
				Message = "OK",
				Ok = true
			};
		}

		[HttpGet]
		[Route("GetCurrentTemperature")]
		public TemperatureData GetCurrentTemperature()
		{
			return TemperatureUdp.TemperatureData;
		}
	}
}
