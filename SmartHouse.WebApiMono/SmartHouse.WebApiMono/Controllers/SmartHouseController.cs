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
			var powerStatus = await YamahaService.PowerStatus();

			if (powerStatus == PowerStatusEnum.StandBy)
			{
				await YamahaService.TurnOn();
				sb.AppendLine("Yamaha Turn on");
				await Task.Delay(TimeSpan.FromSeconds(8));
			}

			await YamahaService.SetInput("AV2");
			sb.AppendLine("Setting AV2 input");

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
			var powerStatus = await YamahaService.PowerStatus();

			PandoraService.Play();
			sb.AppendLine("Pausing pandora radio");

			if (powerStatus == PowerStatusEnum.On)
			{
				await Task.Delay(TimeSpan.FromSeconds(2));
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

		[HttpGet]
		[Route("VolumeUp")]
		public async Task<Result> VolumeUp()
		{
			var sb = new StringBuilder();
			var powerStatus = await YamahaService.PowerStatus();

			if (powerStatus == PowerStatusEnum.On)
			{
				await YamahaService.VolumeUp();
				sb.AppendLine("Yamaha Volume Up");
			}
			else
			{
				sb.AppendLine("Yamaha is turned off");
			}

			return new Result()
			{
				ErrorCode = 0,
				Message = sb.ToString(),
				Ok = true
			};
		}

		[HttpGet]
		[Route("VolumeDown")]
		public async Task<Result> VolumeDown()
		{
			var sb = new StringBuilder();
			var powerStatus = await YamahaService.PowerStatus();

			if (powerStatus == PowerStatusEnum.On)
			{
				await YamahaService.VolumeDown();
				sb.AppendLine("Yamaha Volume Down");
			}
			else
			{
				sb.AppendLine("Yamaha is turned off");	
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
