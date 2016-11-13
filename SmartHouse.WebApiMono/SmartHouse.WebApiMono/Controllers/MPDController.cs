using System;
using System.Web.Http;
using Libmpc;
using SmartHouse.Lib;

namespace SmartHouse.WebApiMono
{
	[RoutePrefix("api/MPD")]
	public class MPDController : BaseController
	{
		private readonly IMPDService MpdService;

		public MPDController(ISettingsService service, IMPDService mpdService) : base(service)
		{
			MpdService = mpdService;
		}

		[HttpGet]
		[Route("Play")]
		public Result Play()
		{
			return MpdService.Play();
		}

		[HttpGet]
		[Route("Pause")]
		public Result Pause()
		{
			return MpdService.Pause();
		}

		[HttpGet]
		[Route("Stop")]
		public Result Stop()
		{
			return MpdService.Stop();
		}

		[HttpGet]
		[Route("Next")]
		public Result Next()
		{
			return MpdService.Next();
		}

		[HttpGet]
		[Route("Previous")]
		public Result Previous()
		{
			return MpdService.Previous();
		}

		[HttpGet]
		[Route("GetStatus")]
		public MpdStatus GetStatus()
		{
			return MpdService.GetStatus();
		}

		[HttpGet]
		[Route("GetCurrentSong")]
		public MpdFile GetCurrentSong()
		{
			return MpdService.GetCurrentSong();
		}
	}
}
