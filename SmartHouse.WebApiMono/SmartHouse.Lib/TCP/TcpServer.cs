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

						} while (netStream.DataAvailable);

						Logger.LogInfoMessage($"Payload: {payload}");

						Result result = null;

						try
						{
							var command = new TcpCommands();
							result = command.ExecuteTcpCommand(payload);
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

		public async Task<T> SendCommandToServer<T>(string data)
		{
			var buffer = new byte[2048];

			using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
			{
				socket.Blocking = true;

				var ipAddress = IPAddress.Parse(IpAddress);
				var ipLocal = new IPEndPoint(ipAddress, Port);

				socket.Connect(ipLocal);

				Logger.LogInfoMessage("Client connected");

				if (!socket.Connected)
					throw new Exception("Connection NOT established on " + ipLocal.Address.ToString() + ", port" + ipLocal.Port);

				var response = String.Empty;

				var buffOut = Encoding.UTF8.GetBytes(response);

				Logger.LogInfoMessage($"Sending {response}");
				socket.Send(buffOut);
				Logger.LogInfoMessage($"Data sent");

				using (var netStream = new NetworkStream(socket))
				{
					do
					{
						var size = await netStream.ReadAsync(buffer, 0, buffer.Length);
						var str = Encoding.UTF8.GetString(buffer, 0, size);
						response += str;

					} while (netStream.DataAvailable);
				}

				Logger.LogInfoMessage($"Result: {response}");

				var result = (T)JsonConvert.DeserializeObject(response);
				return result;
			}
		}
	}
}
