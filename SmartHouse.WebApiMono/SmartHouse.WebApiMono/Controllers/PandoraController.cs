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
			var result = await PandoraService.StartTcp();
			await Task.Delay(1000);

			return result;
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
			var result = PandoraService.ThumbUp();

			NotifyClients();
			PushNotification("Thumb Up");

			return result;
		}

		[HttpGet]
		[Route("ThumbDown")]
		public Result ThumbDown()
		{
			var result = PandoraService.ThumbDown();

			NotifyClients();
			PushNotification("Thumb Down");

			return result;
		}

		[HttpGet]
		[Route("Tired")]
		public Result Tired()
		{
			var result = PandoraService.TiredOfThisSong();

			NotifyClients();
			PushNotification("Tired of this song");

			return result;
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
		public Result NextStation()
		{
			var result = PandoraService.NextStation();

			NotifyClients();
			PushNotification(result.Message);

			return result;
		}

		[HttpGet]
		[Route("PrevStation")]
		public Result PrevStation()
		{
			var result = PandoraService.PrevStation();

			NotifyClients();
			PushNotification(result.Message);

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

		[HttpGet]
		[Route("Refresh")]
		public void Refresh()
		{
			var context = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<ServerHub>();
			if (context == null)
				return;

			var pandoraInfo = PandoraService.GetCurrentSongInfo();
			context.Clients.All.pandoraRefresh(pandoraInfo);
		}
	}
}
