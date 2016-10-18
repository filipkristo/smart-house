using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using SmartHouse.Lib;

namespace SmartHouse.WebApiMono
{
	[RoutePrefix("api/Pandora")]
	public class PandoraController : BaseController
	{
		private IPanodraService PandoraService;

		public PandoraController(ISettingsService service, IPanodraService pandoraService) : base(service)
		{
			PandoraService = pandoraService;
		}

		[HttpGet]
		[Route("Play")]
		public Result Play()
		{
			return PandoraService.Play();
		}

		[HttpGet]
		[Route("Start")]
		public async Task<Result> Start()
		{
			return await PandoraService.StartTcp();
		}

		[HttpGet]
		[Route("Stop")]
		public async Task<Result> Stop()
		{
			return await PandoraService.StopTcp();
		}

		[HttpGet]
		[Route("Restart")]
		public async Task<Result> Restart()
		{
			return await PandoraService.RestartTcp();
		}

		[HttpGet]
		[Route("Next")]
		public Result Next()
		{
			return PandoraService.Next();
		}

		[HttpGet]
		[Route("ThumbUp")]
		public Result ThumbUp()
		{
			return PandoraService.ThumbUp();
		}

		[HttpGet]
		[Route("ThumbDown")]
		public Result ThumbDown()
		{
			return PandoraService.ThumbDown();
		}

		[HttpGet]
		[Route("Tired")]
		public Result Tired()
		{
			return PandoraService.TiredOfThisSong();
		}

		[HttpGet]
		[Route("VolumeUp")]
		public Result VolumeUp()
		{
			return PandoraService.VolumeUp();
		}

		[HttpGet]
		[Route("VolumeDown")]
		public Result VolumeDown()
		{
			return PandoraService.VolumeDown();
		}

		[HttpGet]
		[Route("ChangeStation")]
		public Result ChangeStation(string stationId)
		{
			return PandoraService.ChangeStation(stationId);
		}

		[HttpGet]
		[Route("CurrentSongInfo")]
		public PandoraResult CurrentSongInfo()
		{
			return PandoraService.GetCurrentSongInfo();
		}

		[HttpGet]
		[Route("StationList")]
		public IEnumerable<KeyValue> StationList()
		{
			return PandoraService.GetStationList();
		}

		[HttpGet]
		[Route("IsPlaying")]
		public bool IsPlaying()
		{
			return PandoraService.IsPlaying();
		}
	}
}
