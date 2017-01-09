using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SmartHouse.Lib
{
	public class TcpServer
	{
		public const string IpAddress = "127.0.0.1";
		public const int Port = 5731;

		public TcpServer()
		{
		}

#pragma warning disable RECS0135 // Function does not reach its end or a 'return' statement by any of possible execution paths
		public async Task StartTcpServer()
#pragma warning restore RECS0135 // Function does not reach its end or a 'return' statement by any of possible execution paths
		{
			var ipAddress = IPAddress.Parse(IpAddress);
			var tcpServerListener = new TcpListener(ipAddress, Port);
			tcpServerListener.Start();

			while (true)
			{
				using (var tcpClient = await tcpServerListener.AcceptTcpClientAsync())
				using (var netStream = tcpClient.GetStream())
				using (var streamReader = new StreamReader(netStream, Encoding.UTF8))
				using (var streamWritter = new StreamWriter(netStream, Encoding.UTF8) { AutoFlush = true })
				{
					tcpClient.SendTimeout = (int)TimeSpan.FromSeconds(10).TotalMilliseconds;
					tcpClient.ReceiveTimeout = (int)TimeSpan.FromSeconds(10).TotalMilliseconds;

					Logger.LogDebugMessage("StartTcpServer: Reading data from network stream reader");
					var payload = await streamReader.ReadLineAsync();

					Result result = null;
					try
					{
						var command = new TcpCommands();
						result = await command.ExecuteTcpCommand(payload);
					}
					catch (Exception ex)
					{
						result = new Result()
						{
							ErrorCode = -1,
							Message = ex.Message,
							Ok = false
						};
					}

					var json = JsonConvert.SerializeObject(result);

					Logger.LogDebugMessage($"TCP Server Length: {json.Length}");
					await streamWritter.WriteLineAsync(json);
					Logger.LogDebugMessage($"Socket finished");
				}
			}

		}

		public async Task<T> SendCommandToServer<T>(string data) where T : class
		{
			using (var tcpClient = new TcpClient(IpAddress, Port))
			{
				using (var netStream = tcpClient.GetStream())
				using (var streamReader = new StreamReader(netStream, Encoding.UTF8))
				using (var streamWritter = new StreamWriter(netStream, Encoding.UTF8) { AutoFlush = true })
				{
					tcpClient.SendTimeout = (int)TimeSpan.FromSeconds(10).TotalMilliseconds;
					tcpClient.ReceiveTimeout = (int)TimeSpan.FromSeconds(10).TotalMilliseconds;

					Logger.LogDebugMessage($"SendCommandToServer: Writing data to network stream writter");
					await streamWritter.WriteLineAsync(data);

					Logger.LogDebugMessage($"SendCommandToServer: Reading data from network stream reader");
					var response = await streamReader.ReadLineAsync();
					Logger.LogDebugMessage($"SendCommandToServer: TCP Client length: {response.Length}");

					var result = JsonConvert.DeserializeObject(response, typeof(T)) as T;
					return result;
				}
			}
		}
	}
}
