using System;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using SmartHouse.Lib;

namespace SmartHouse.WebApiMono
{
	[RoutePrefix("api/SmartHouse")]
	public class SmartHouseController : BaseController
	{
		private readonly IYamahaService YamahaService;
		private readonly IPanodraService PandoraService;

		public SmartHouseController(ISettingsService service, IYamahaService yamahaService, IPanodraService pandoraService) : base(service)
		{
			YamahaService = yamahaService;
			PandoraService = pandoraService;
		}

		[HttpGet]
		[Route("TurnOn")]
		public async Task<Result> TurnOn()
		{
			var sb = new StringBuilder();
			var yamahaInfo = await YamahaService.GetInfo();

			if (yamahaInfo.Main_Zone.Basic_Status.Power_Control.Power != "On")
			{
				await YamahaService.TurnOn();
				sb.AppendLine("Yamaha Turn on");
			}
			await YamahaService.SetInput("AV2");
			sb.AppendLine("Setting AV2 input");

			await Task.Delay(TimeSpan.FromSeconds(1));

			PandoraService.Play();
			sb.AppendLine("Playing pandora radio");

			return new Result()
			{
				ErrorCode = 0,
				Message = sb.ToString(),
				Ok = true
			};
		}

		[HttpGet]
		[Route("TurnOff")]
		public async Task<Result> TurnOff()
		{
			var sb = new StringBuilder();
			var yamahaInfo = await YamahaService.GetInfo();

			PandoraService.Play();
			sb.AppendLine("Pausing pandora radio");

			await Task.Delay(TimeSpan.FromSeconds(2));

			if (yamahaInfo.Main_Zone.Basic_Status.Power_Control.Power == "On")
			{
				await YamahaService.TurnOff();
				sb.AppendLine("Yamaha Turn Off");
			}

			return new Result()
			{
				ErrorCode = 0,
				Message = sb.ToString(),
				Ok = true
			};
		}
	}
}
