﻿using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
	public class TelemetryService : ITelemetryService
	{
		private static TemperatureData TemperatureData { get; } = new TemperatureData();

		//private LivingTelemetryRoomService azureTelemetry = new LivingTelemetryRoomService();

		public TelemetryService()
		{
		}

		public async Task<TemperatureData> GetLastTemperature()
		{			
			await Task.FromResult<object>(null);

			return TemperatureData;
		}

		public async Task InitializeAzure()
		{
			//await azureTelemetry.Initialize();
			await Task.FromResult<object>(null);
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
			lock (TemperatureData)
			{
				TemperatureData.Temperature = Convert.ToDecimal(data.Split(';')[0]);
				TemperatureData.Humidity = Convert.ToDecimal(data.Split(';')[1]);
				TemperatureData.HeatIndex = Convert.ToDecimal(data.Split(';')[2]);
				TemperatureData.Measured = DateTime.Now;
			}

			//azureTelemetry.SendDeviceTelemetryData(TemperatureData.Temperature, TemperatureData.Humidity, TemperatureData.HeatIndex);

			return await SaveTemperature(TemperatureData);
		}

		private async Task SendRequestToCustomWebsite(decimal temp, decimal humi, decimal heatindex)
		{
			using (var client = new HttpClient())
			{
				var result = await client.GetStringAsync($"http://piautomation.dx.am/api.php?RoomId=1&Temp={temp}&Humi={humi}&HeatIndex={heatindex}&id={Guid.NewGuid()}");
			}
		}

		public void Dispose()
		{
			//azureTelemetry?.Dispose();
		}
	}
}