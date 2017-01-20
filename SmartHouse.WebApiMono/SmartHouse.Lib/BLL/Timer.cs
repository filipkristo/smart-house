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
            Logger.LogInfoMessage($"Turning of smart house for {TimeoutMinutes} minutes");
			await Task.Delay(TimeSpan.FromMinutes(TimeoutMinutes));

            Logger.LogInfoMessage($"Starting to turn off smartHouse");
            using (var client = new HttpClient())
			{
				await client.GetStringAsync("http://127.0.0.1:8081/api/SmartHouse/TurnOff");
			}
		}
	}
}
