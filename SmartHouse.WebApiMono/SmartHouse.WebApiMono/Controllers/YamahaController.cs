using System;
using System.Threading.Tasks;
using System.Web.Http;
using SmartHouse.Lib;

namespace SmartHouse.WebApiMono
{
	[RoutePrefix("api/Yamaha")]
	public class YamahaController : BaseController
	{
		private readonly IYamahaService YamahaService;

		public YamahaController(ISettingsService service, IYamahaService yamahaService) : base(service)
		{
			YamahaService = yamahaService;
		}

		[HttpGet]
		[Route("Status")]
		public async Task<YamahaBasicStatus> Status()
		{
			return await YamahaService.GetInfo();
		}

		[HttpGet]
		[Route("TurnOn")]
		public async Task<string> TurnOn()
		{
			return await YamahaService.TurnOn();
		}

		[HttpGet]
		[Route("TurnOff")]
		public async Task<string> TurnOff()
		{
			return await YamahaService.TurnOff();
		}

		[HttpGet]
		[Route("SetInput")]
		public async Task<string> SetInput(string input)
		{
			return await YamahaService.SetInput(input);
		}

		[HttpGet]
		[Route("VolumeUp")]
		public async Task<int> VolumeUp()
		{
			return await YamahaService.VolumeUp();
		}

		[HttpGet]
		[Route("VolumeDown")]
		public async Task<int> VolumeDown()
		{
			return await YamahaService.VolumeDown();
		}

		[HttpGet]
		[Route("SetVolume")]
		public async Task<string> SetVolume(decimal volume)
		{
			var newVolume = (int)Math.Round(volume * 10, 0);
			var result = await YamahaService.SetVolume(newVolume);
			NotifyClients();

			return result;
		}
	}
}
