using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;

namespace SmartHouse.AzureIot
{
	public class LivingTelemetryRoomService : IDisposable
	{
		private DeviceClient deviceClient;

		public LivingTelemetryRoomService()
		{
		}

		private byte[] Serialize(object obj)
		{
			string json = JsonConvert.SerializeObject(obj);
			return Encoding.UTF8.GetBytes(json);

		}

		private dynamic DeSerialize(byte[] data)
		{
			string text = Encoding.UTF8.GetString(data);
			return JsonConvert.DeserializeObject(text);
		}

		public async Task Initialize()
		{
			deviceClient = DeviceClient.CreateFromConnectionString(AzureIot.AureIoTAuth.ConnectionString, TransportType.Http1);
			await deviceClient.OpenAsync();

			DeviceProperties device = new DeviceProperties();
			Thermostat thermostat = new Thermostat();

			thermostat.ObjectType = "DeviceInfo";
			thermostat.IsSimulatedDevice = false;
			thermostat.Version = "1.0";

			device.HubEnabledState = true;
			device.DeviceID = AzureIot.AureIoTAuth.DeviceId;
			device.Manufacturer = "Arduino";
			device.ModelNumber = "UNO";
			device.SerialNumber = "";
			device.FirmwareVersion = "";
			device.Platform = "Arduino";
			device.Processor = "ATMega";
			device.InstalledRAM = "";
			device.DeviceState = "normal";

			device.Latitude = 43.51183799354402;
			device.Longitude = 16.472984513097344;

			thermostat.DeviceProperties = device;

			var TriggerAlarm = new Command();
			TriggerAlarm.Name = "TriggerAlarm";

			var param = new CommandParameter();
			param.Name = "Message";
			param.Type = "String";
			TriggerAlarm.Parameters = new CommandParameter[] { param };

			thermostat.Commands = new Command[] { TriggerAlarm };

			try
			{
				var msg = new Message(Serialize(thermostat));
				await deviceClient?.SendEventAsync(msg);
			}
			catch (System.Exception e)
			{
				Console.WriteLine("Exception while sending device meta data :\n" + e.Message);
			}

			Console.WriteLine("Sent meta data to IoT Suite\n" + AzureIot.AureIoTAuth.Hostname);

			await Task.Run(() => ReceiveDataFromAzure());
		}

		public async void SendDeviceTelemetryData(decimal temperature, decimal humidity, decimal heatIndex)
		{
			var data = new TelemetryData();
			data.DeviceId = AzureIot.AureIoTAuth.DeviceId;
			data.Temperature = temperature;
			data.Humidity = humidity;
			data.HeatIndex = heatIndex;

			try
			{
				var msg = new Message(Serialize(data));
				if (deviceClient != null)
				{
					await deviceClient.SendEventAsync(msg);
				}
			}
			catch (System.Exception e)
			{
				Console.WriteLine("Exception while sending device telemetry data :\n" + e.Message.ToString());
			}
			Console.WriteLine("Sent telemetry data to IoT Suite\nTemperature=" + string.Format("{0:0.00}", temperature) + "\nHumidity=" + string.Format("{0:0.00}", humidity));

		}

		private async void ReceiveDataFromAzure()
		{

			while (true)
			{
				Message message = await deviceClient.ReceiveAsync();
				if (message != null)
				{
					try
					{
						dynamic command = DeSerialize(message.GetBytes());

						Console.WriteLine($"Received from IOT hub command: {command.Parameters.Message}, name: {command.Name}");

						await deviceClient.CompleteAsync(message);
					}
					catch
					{
						await deviceClient.RejectAsync(message);
					}
				}
			}
		}

		public void Dispose()
		{
			deviceClient?.Dispose();
			deviceClient = null;
		}
	}
}
