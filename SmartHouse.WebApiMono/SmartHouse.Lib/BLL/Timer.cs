using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
	public class Timer
	{
		public static int TimeoutMinutes { get; set; }

		public Timer()
		{
			
		}

#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void
		public async void RunCommand()
#pragma warning restore RECS0165 // Asynchronous methods should return a Task instead of void
		{
			await Task.Delay(TimeSpan.FromMinutes(TimeoutMinutes));

			using (var client = new HttpClient())
			{
				await client.GetStringAsync("http://127.0.0.1:8081/api/SmartHouse/TurnOff");
			}
		}
	}
}
