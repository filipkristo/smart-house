using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using SmartHouse.Lib;

namespace SmartHouse.WebApiMono
{
	[RoutePrefix("api/Player")]
	public class PlayerController : BaseController
	{
		private IPlayerFactoryService _playerService;
		private ILastFMService _lastFmService;

		public PlayerController(ISettingsService service, IPlayerFactoryService pandoraService, ILastFMService lastFMService) : base(service)
		{
			_playerService = pandoraService;
			_lastFmService = lastFMService;
		}

		[HttpGet]
		[Route("Play")]
		public Result Play()
		{
			return _playerService.Play();
		}

		[HttpGet]
		[Route("Start")]
		public async Task<Result> Start()
		{
			var result = await _playerService.StartTcp();
			await Task.Delay(1000);

			return result;
		}

		[HttpGet]
		[Route("Stop")]
		public async Task<Result> Stop()
		{
			return await _playerService.StopTcp();
		}

		[HttpGet]
		[Route("Restart")]
		public async Task<Result> Restart()
		{
			return await _playerService.RestartTcp();
		}

		[HttpGet]
		[Route("Next")]
		public Result Next()
		{
			return _playerService.Next();
		}

		[HttpGet]
		[Route("ThumbUp")]
		public Result ThumbUp()
		{
			var result = _playerService.ThumbUp();

			NotifyClients();
			PushNotification("Thumb Up");

			return result;
		}

		[HttpGet]
		[Route("ThumbDown")]
		public Result ThumbDown()
		{
			var result = _playerService.ThumbDown();

			NotifyClients();
			PushNotification("Thumb Down");

			return result;
		}

		[HttpGet]
		[Route("Tired")]
		public Result Tired()
		{
			var result = _playerService.TiredOfThisSong();

			NotifyClients();
			PushNotification("Tired of this song");

			return result;
		}

		[HttpGet]
		[Route("VolumeUp")]
		public Result VolumeUp()
		{
			return _playerService.VolumeUp();
		}

		[HttpGet]
		[Route("VolumeDown")]
		public Result VolumeDown()
		{
			return _playerService.VolumeDown();
		}

		[HttpGet]
		[Route("ChangeStation")]
		public Result ChangeStation(string stationId)
		{
			var result = _playerService.ChangeStation(stationId);
			NotifyClients();

			return result;
		}

		[HttpGet]
		[Route("NextStation")]
		public async Task<Result> NextStation()
		{
			var result = await _playerService.NextStation();

			NotifyClients();
			PushNotification(result.Message);

			return result;
		}

		[HttpGet]
		[Route("PrevStation")]
		public async Task<Result> PrevStation()
		{
			var result = await _playerService.PrevStation();

			NotifyClients();
			PushNotification(result.Message);

			return result;
		}

		[HttpGet]
		[Route("CurrentSongInfo")]
		public PandoraResult CurrentSongInfo()
		{
			return _playerService.GetCurrentSongInfo();
		}

		[HttpGet]
		[Route("StationList")]
		public async Task<IEnumerable<KeyValue>> StationList()
		{
			return await _playerService.GetStationList();
		}

		[HttpGet]
		[Route("IsPlaying")]
		public async Task<bool> IsPlaying()
		{
			return await _playerService.IsPlaying();
		}

		[HttpGet]
		[Route("Refresh")]
		public void Refresh()
		{
			var context = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<ServerHub>();
			if (context == null)
				return;

			var pandoraInfo = _playerService.GetCurrentSongInfo();
			context.Clients.All.pandoraRefresh(pandoraInfo);
		}

		[HttpGet]
		[Route("StartScrobble")]
		public async Task<string> StartScrobble()
		{
			var pandoraInfo = _playerService.GetCurrentSongInfo();
			var song = new SongDetails()
			{
				Album = pandoraInfo.Album,
				Artist = pandoraInfo.Artist,
				Song = pandoraInfo.Song
			};

			return await _lastFmService.StartScrobble(song);
		}

		[HttpGet]
		[Route("UpdateNowPlaying")]
		public async Task<string> UpdateNowPlaying()
		{
			var pandoraInfo = _playerService.GetCurrentSongInfo();
			var song = new SongDetails()
			{
				Album = pandoraInfo.Album,
				Artist = pandoraInfo.Artist,
				Song = pandoraInfo.Song
			};

			return await _lastFmService.UpdateNowPlaying(song);
		}
	}
}
