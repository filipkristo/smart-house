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
        private readonly ILastFMService LastFMService;

        public LastFMController(ISettingsService service, ILastFMService lastFMService)
            : base(service)
        {
            LastFMService = lastFMService;
        }

        [Route("StartScrobble")]
        [HttpPost]
        public HttpResponseMessage StartScrobble(SongDetails song)
        {
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

            LastFMService.StartScrobbleBash(song);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [Route("LoveSong")]
        [HttpGet]
        public async Task<string> LoveSong(string artistName, string songName)
        {
            return await LastFMService.LoveSong(artistName, songName);
        }

        [Route("UnloveSong")]
        [HttpGet]
        public async Task<string> UnloveSong(string artistName, string songName)
        {
            return await LastFMService.UnloveSong(artistName, songName);
        }

        [Route("GetTopTracks")]
        [HttpGet]
        public async Task<List<LastTrack>> GetTopTracks()
        {
            return await LastFMService.GetTopTracks();
        }

        [Route("GetSongInfo")]
        [HttpGet]
        public async Task<LastTrack> GetSongInfo(string artistName, string songName)
        {
            return await LastFMService.GetSongInfo(artistName, songName);
        }

        [Route("GetAlbumInfo")]
        [HttpGet]
        public async Task<LastAlbum> GetAlbumInfo(string artist, string album)
        {
            return await LastFMService.GetAlbumInfo(artist, album);
        }

        [Route("GetArtistInfo")]
        [HttpGet]
        public async Task<LastArtist> GetArtistInfo(string artist)
        {
            return await LastFMService.GetArtistInfo(artist);
        }

        [Route("GetSimilarArtist")]
        [HttpGet]
        public async Task<IEnumerable<LastArtist>> GetSimilarArtist(string artist, int limit = 50)
        {
            return await LastFMService.GetSimilarArtist(artist, limit);
        }

        [Route("GetRecentTopArtists")]
        [HttpGet]
        public async Task<IEnumerable<ArtistTileData>> GetRecentTopArtists()
        {
            return await LastFMService.GetRecentTopArtists();
        }
    }
}
