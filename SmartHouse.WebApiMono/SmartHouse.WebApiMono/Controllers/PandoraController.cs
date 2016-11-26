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
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
			PandoraService.StartTcp();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

			await Task.Delay(1000);
			return null;
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
			var result = PandoraService.ThumbDown();

			NotifyClients();
			PushNotification("Thumb Up");

			return result;
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
			var result = PandoraService.ChangeStation(stationId);
			NotifyClients();

			return result;
		}

		[HttpGet]
		[Route("NextStation")]
		public Result NextStation(string stationId)
		{
			var result = PandoraService.NextStation();
			NotifyClients();

			return result;
		}

		[HttpGet]
		[Route("PrevStation")]
		public Result PrevStation(string stationId)
		{
			var result = PandoraService.PrevStation();
			NotifyClients();

			return result;
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
