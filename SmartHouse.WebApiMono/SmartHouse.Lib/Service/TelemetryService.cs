using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
	public class TelemetryService : ITelemetryService
	{
		private static TemperatureData TemperatureData { get; } = new TemperatureData();

		private Action<TemperatureData> SignalR;

		public TelemetryService(Action<TemperatureData> signalR)
		{
			SignalR = signalR;
		}

		public async Task<TemperatureData> GetLastTemperature()
		{			
			await Task.FromResult<object>(null);
			return TemperatureData;
		}

		public async Task<Result> SaveTemperature(TemperatureData data)
		{
			await SendRequestToCustomWebsite(TemperatureData.Temperature, TemperatureData.Humidity, TemperatureData.HeatIndex);

			return new Result()
			{
				Message = $"Inserted temperature {data.Temperature}, humidity: {data.Humidity}, heatindex: {data.HeatIndex}",
				ErrorCode = 0,
				Ok = true
			};
		}

		public async Task<Result> SaveTemperatureUdp(string data)
		{
			//var uri = "https://github.com/fsautomata/azure-iot-sdks/blob/master/c/doc/device_setup_raspberrypi2_rasbian.md";

			lock (TemperatureData)
			{
				TemperatureData.Temperature = Convert.ToDecimal(data.Split(';')[0]);
				TemperatureData.Humidity = Convert.ToDecimal(data.Split(';')[1]);
				TemperatureData.HeatIndex = Convert.ToDecimal(data.Split(';')[2]);
				TemperatureData.Measured = DateTime.Now;

				SignalR?.Invoke(TemperatureData);
			}

			return await Task.FromResult<Result>(new Result());
			//return await SaveTemperature(TemperatureData);
		}

		private async Task<string> SendRequestToCustomWebsite(decimal temp, decimal humi, decimal heatindex)
		{
			using (var client = new HttpClient())
			{
				var result = await client.GetStringAsync($"http://piautomation.dx.am/api.php?RoomId=1&Temp={temp}&Humi={humi}&HeatIndex={heatindex}&id={Guid.NewGuid()}");
				return result;
			}
		}

		public void Dispose()
		{
			
		}
	}
}
