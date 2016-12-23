using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using IF.Lastfm.Core.Objects;
using SmartHouse.Lib;

namespace SmartHouse.WebApiMono
{
	[RoutePrefix("api/LastFM")]
	public class LastFMController : BaseController
	{
		ILastFMService LastFMService;

		public LastFMController(ISettingsService service, ILastFMService lastFMService)
			:base(service)
		{
			LastFMService = lastFMService;
		}

		[Route("StartScrobble")]
		[HttpPost]
		public async Task<HttpResponseMessage> StartScrobble(SongDetails song)
		{
			if(!ModelState.IsValid)
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

			await LastFMService.StartScrobble(song);
			return new HttpResponseMessage(HttpStatusCode.OK);
		}

		[Route("GetSongInfo")]
		[HttpGet]
		public async Task<LastTrack> GetSongInfo(string artistName, string songName)
		{
			return await LastFMService.GetInfo(artistName, songName);
		}

		[Route("LoveSong")]
		[HttpGet]
		public async Task<string> LoveSong(string artistName, string songName)
		{
			return await LastFMService.LoveSong(artistName, songName);
		}

		[Route("GetTopTracks")]
		[HttpGet]
		public async Task<List<LastTrack>> GetTopTracks()
		{
			return await LastFMService.GetTopTracks();
		}
	}
}
