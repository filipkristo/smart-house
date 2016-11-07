using System;
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

		public async Task StartTcpServer()
		{
			var ipAddress = IPAddress.Parse(IpAddress);
			var tcpServerListener = new TcpListener(ipAddress, Port);
			tcpServerListener.Start();

			while (true)
			{
				using (var serverSocket = await tcpServerListener.AcceptSocketAsync())
				{
					if (serverSocket.Connected)
					{
						Logger.LogInfoMessage("Server socket Connected");

						var payload = String.Empty;

						var buffer = new byte[512];
						int readBytes = 0;

						var netStream = new NetworkStream(serverSocket);

						do
						{
							readBytes = await netStream.ReadAsync(buffer, 0, buffer.Length);
							payload += Encoding.UTF8.GetString(buffer, 0, readBytes);

							if (!netStream.DataAvailable)
								await Task.Delay(50);

						} while (netStream.DataAvailable);

						Logger.LogInfoMessage($"Payload: {payload}");
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
						var buffOut = Encoding.UTF8.GetBytes(json);

						SocketError errors;
						serverSocket.Send(buffOut, 0, buffOut.Length, SocketFlags.None, out errors);

						Logger.LogInfoMessage($"Socket result {errors.ToString()}");
					}

					serverSocket.Close();
				}
			}
		}

		public async Task<T> SendCommandToServer<T>(string data) where T : class
		{
			var buffer = new byte[512];

			using (var tcpClient = new TcpClient())
			{
				tcpClient.Connect(IpAddress, Port);

				var stream = tcpClient.GetStream();

				var bytes = Encoding.UTF8.GetBytes(data);
				await stream.WriteAsync(bytes, 0, bytes.Length);

				var response = string.Empty;

				do
				{
					var size = await stream.ReadAsync(buffer, 0, buffer.Length);
					var str = Encoding.UTF8.GetString(buffer, 0, size);
					response += str;

					if (!stream.DataAvailable)
						await Task.Delay(50);

				} while (stream.DataAvailable);

				Logger.LogInfoMessage($"Result: {response}");

				var result = JsonConvert.DeserializeObject(response) as T;
				return result;
			}

		}
	}
}
