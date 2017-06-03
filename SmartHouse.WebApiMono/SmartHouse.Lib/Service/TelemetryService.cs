using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
	public class TelemetryService : ITelemetryService
	{
        private const string tempFile = "temperature";

        private static TelemetryData TemperatureData { get; } = new TelemetryData();

        public Action<TelemetryData> SignalR { get; set; }        

        public TelemetryService()
        {

        }		

		public async Task<TelemetryData> GetLastTemperature()
		{
            return await GetTemperatureFromFile();		
		}

		public Result SaveTemperature(TelemetryData data)
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
                TemperatureData.GasValue = Convert.ToDecimal(data.Split(';')[3]);
                TemperatureData.Measured = DateTime.UtcNow;

                SignalR?.Invoke(TemperatureData);
			}

            await SaveTemperatureToFile(TemperatureData);

            return new Result() { Ok = true, ErrorCode = 0, Message = "Saved temperature"};			
		}

        private async Task SaveTemperatureToFile(TelemetryData data)
        {
            using (var fileStream = File.Open(tempFile, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var JSON = JsonConvert.SerializeObject(data);
                var bytes = Encoding.UTF8.GetBytes(JSON);
                await fileStream.WriteAsync(bytes, 0, bytes.Length);
            }
        }

        private async Task<TelemetryData> GetTemperatureFromFile()
        {            
            if (!File.Exists(tempFile))
                return null;

            using (var fileStream = File.Open(tempFile, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                var bytes = new byte[fileStream.Length];
                await fileStream.ReadAsync(bytes, 0, bytes.Length);

                var JSON = Encoding.UTF8.GetString(bytes);                
                return JsonConvert.DeserializeObject(JSON, typeof(TelemetryData)) as TelemetryData;
            }
        }

        public async Task<Result> AirCondition(byte On)
        {
            using (var tcpClient = new TcpClient("10.110.166.197", 9000))
            {
                using (var netStream = tcpClient.GetStream())
                using (var streamReader = new StreamReader(netStream, Encoding.ASCII))
                using (var streamWritter = new StreamWriter(netStream, Encoding.ASCII) { AutoFlush = true })
                {
                    tcpClient.SendTimeout = (int)TimeSpan.FromSeconds(10).TotalMilliseconds;
                    tcpClient.ReceiveTimeout = (int)TimeSpan.FromSeconds(10).TotalMilliseconds;                    

                    await streamWritter.WriteLineAsync(On.ToString());                    
                    var response = await streamReader.ReadLineAsync();

                    return new Result()
                    {
                        Ok = true,
                        ErrorCode = 0,
                        Message = response
                    };
                }
            }
        }

        public async Task<byte> GetAirConditionState()
        {
            using (var tcpClient = new TcpClient("10.110.166.197", 9000))
            {
                using (var netStream = tcpClient.GetStream())
                using (var streamReader = new StreamReader(netStream, Encoding.ASCII))
                using (var streamWritter = new StreamWriter(netStream, Encoding.ASCII) { AutoFlush = true })
                {
                    tcpClient.SendTimeout = (int)TimeSpan.FromSeconds(10).TotalMilliseconds;
                    tcpClient.ReceiveTimeout = (int)TimeSpan.FromSeconds(10).TotalMilliseconds;

                    await streamWritter.WriteLineAsync("?");
                    var response = await streamReader.ReadLineAsync();

                    return Convert.ToByte(response);
                }
            }
        }      
	}
}
