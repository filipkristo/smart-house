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
        private readonly ILastFMService _lastFmService;

        public LastFMController(ISettingsService service, ILastFMService lastFmService)
            : base(service)
        {
            _lastFmService = lastFmService;
        }

        [Route("StartScrobble")]
        [HttpPost]
        public HttpResponseMessage StartScrobble(SongDetails song)
        {
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

            _lastFmService.StartScrobbleBash(song);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [Route("LoveSong")]
        [HttpGet]
        public async Task<string> LoveSong(string artistName, string songName)
        {
            return await _lastFmService.LoveSong(artistName, songName);
        }

        [Route("UnloveSong")]
        [HttpGet]
        public async Task<string> UnloveSong(string artistName, string songName)
        {
            return await _lastFmService.UnloveSong(artistName, songName);
        }

        [Route("GetTopTracks")]
        [HttpGet]
        public async Task<List<LastTrack>> GetTopTracks()
        {
            return await _lastFmService.GetTopTracks();
        }

        [Route("GetSongInfo")]
        [HttpGet]
        public async Task<LastTrack> GetSongInfo(string artistName, string songName)
        {
            return await _lastFmService.GetSongInfo(artistName, songName);
        }

        [Route("GetAlbumInfo")]
        [HttpGet]
        public async Task<LastAlbum> GetAlbumInfo(string artist, string album)
        {
            return await _lastFmService.GetAlbumInfo(artist, album);
        }

        [Route("GetArtistInfo")]
        [HttpGet]
        public async Task<LastArtist> GetArtistInfo(string artist)
        {
            return await _lastFmService.GetArtistInfo(artist);
        }

        [Route("GetSimilarArtist")]
        [HttpGet]
        public async Task<IEnumerable<LastArtist>> GetSimilarArtist(string artist, int limit = 50)
        {
            return await _lastFmService.GetSimilarArtist(artist, limit);
        }

        [Route("GetRecentTopArtists")]
        [HttpGet]
        public async Task<IEnumerable<ArtistTileData>> GetRecentTopArtists()
        {
            return await _lastFmService.GetRecentTopArtists(DateTimeOffset.UtcNow.Date.AddDays(-1), 10);
        }
    }
}
