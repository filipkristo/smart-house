using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
	public class TelemetryService : ITelemetryService
	{
        private const string tempFile = "temperature";

        private static TemperatureData TemperatureData { get; } = new TemperatureData();

		private Action<TemperatureData> SignalR;

        public TelemetryService()
        {

        }

		public TelemetryService(Action<TemperatureData> signalR)
		{
			SignalR = signalR;
		}

		public async Task<TemperatureData> GetLastTemperature()
		{
            return await GetTemperatureFromFile();		
		}

		public Result SaveTemperature(TemperatureData data)
		{			
			return new Result()
			{
				Message = $"Temperature {data.Temperature}, humidity: {data.Humidity}, heatindex: {data.HeatIndex}",
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

            await SaveTemperatureToFile(TemperatureData);

            return new Result() { Ok = true, ErrorCode = 0, Message = "Saved temperature"};			
		}

        private async Task SaveTemperatureToFile(TemperatureData data)
        {
            using (var fileStream = File.Open(tempFile, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var JSON = JsonConvert.SerializeObject(data);
                var bytes = Encoding.UTF8.GetBytes(JSON);
                await fileStream.WriteAsync(bytes, 0, bytes.Length);
            }
        }

        private async Task<TemperatureData> GetTemperatureFromFile()
        {            
            if (!File.Exists(tempFile))
                return null;

            using (var fileStream = File.Open(tempFile, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                var bytes = new byte[fileStream.Length];
                await fileStream.ReadAsync(bytes, 0, bytes.Length);

                var JSON = Encoding.UTF8.GetString(bytes);                
                return JsonConvert.DeserializeObject(JSON, typeof(TemperatureData)) as TemperatureData;
            }
        }

        public void Dispose()
		{
			
		}
	}
}
