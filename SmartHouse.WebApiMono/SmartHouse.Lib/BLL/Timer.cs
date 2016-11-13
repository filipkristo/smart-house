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

		public async Task RunCommand()
		{
			await Task.Delay(TimeSpan.FromMinutes(TimeoutMinutes));

			using (var client = new HttpClient())
			{
				await client.GetStringAsync("http://127.0.0.1:8081/api/SmartHouse/TurnOff");
			}
		}
	}
}
