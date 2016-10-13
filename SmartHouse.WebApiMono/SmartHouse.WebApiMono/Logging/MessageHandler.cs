using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHouse.WebApiMono
{
	public abstract class MessageHandler : DelegatingHandler
	{
		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			var corrId = string.Format("{0}{1}", DateTime.Now.Ticks, Thread.CurrentThread.ManagedThreadId);
			var requestInfo = string.Format("{0} {1}", request.Method, request.RequestUri);

			var requestMessage = await request.Content.ReadAsByteArrayAsync();
			await IncommingMessageAsync(corrId, requestInfo, requestMessage);

			var response = await base.SendAsync(request, cancellationToken);

			if (response.Content == null)
				return response;

			byte[] responseMessage;

			if (response.IsSuccessStatusCode)
				responseMessage = await response.Content.ReadAsByteArrayAsync();
			else
				responseMessage = Encoding.UTF8.GetBytes(response.ReasonPhrase);

			await OutgoingMessageAsync(corrId, requestInfo, responseMessage);

			return response;
		}

		protected abstract Task IncommingMessageAsync(string correlationId, string requestInfo, byte[] message);
		protected abstract Task OutgoingMessageAsync(string correlationId, string requestInfo, byte[] message);
	}
}
