using System;
using System.Threading.Tasks;

namespace SmartHouse.WebApiMono
{
	public class MessageLoggingHandler : MessageHandler
	{

		protected override async Task IncommingMessageAsync(string correlationId, string requestInfo, byte[] message)
		{
			await Task.Run(() =>
					{

					}
				);
		}


		protected override async Task OutgoingMessageAsync(string correlationId, string requestInfo, byte[] message)
		{
			await Task.Run(() =>
			{

			});
		}
	}
}
