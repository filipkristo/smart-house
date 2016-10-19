using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
	public class TemperatureUdp
	{
		public static TemperatureData TemperatureData { get; } = new TemperatureData();

		public async Task StartListen()
		{
			await Task.Run(() =>
			{
				using (var client = new UdpClient())
				{
					var localEp = new IPEndPoint(IPAddress.Any, 9876);
					client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
					client.ExclusiveAddressUse = false;

					client.Client.Bind(localEp);

					while (true)
					{
						try
						{
							var buffer = client.Receive(ref localEp);
							var data = Encoding.ASCII.GetString(buffer);

							lock (TemperatureData)
							{
								TemperatureData.Temperature = Convert.ToDecimal(data.Split(';')[0]);
								TemperatureData.Humidity = Convert.ToDecimal(data.Split(';')[1]);
								TemperatureData.HeatIndex = Convert.ToDecimal(data.Split(';')[2]);
								TemperatureData.Measured = DateTime.Now;
							}
						}
						catch (Exception ex)
						{
							Logger.LogErrorMessage("Reading UDP", ex);
						}
					}
				}

			});
		}
	}
}
