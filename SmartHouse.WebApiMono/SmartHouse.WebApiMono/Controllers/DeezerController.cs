using Newtonsoft.Json;
using SmartHouse.Lib;
using SmartHouse.Lib.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace SmartHouse.WebApiMono.Controllers
{
    [RoutePrefix("api/Deezer")]
    public class DeezerController : BaseController
    {
        private readonly ILastFMService _lastFMService;
        private readonly IPlayerService _playerFactoryService;

        public DeezerController(ISettingsService settingsService, ILastFMService lastFMService) : base(settingsService)
        {
            _lastFMService = lastFMService;
            _playerFactoryService = new DeezerService();
        }

        [HttpPost]
        [Route("SongChange")]
        public async Task SongChange(DeezerState deezerState)
        {
            if(File.Exists("deezer.json"))
            {
                var lastSong = _playerFactoryService.GetCurrentSongInfo();

                if((DateTime.UtcNow - lastSong.LastModifed) > TimeSpan.FromMinutes(1))
                {
                    await _lastFMService.StartScrobble(new SongDetails
                    {
                        Album = lastSong.Album,
                        Song = lastSong.Song,
                        Artist = lastSong.Artist
                    });
                }
            }

            var context = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<ServerHub>();
            if (context == null)
                return;

            var pandoraInfo = new PandoraResult
            {
                Album = deezerState.AlbumName,
                AlbumUri = deezerState.AlbumUri,
                Artist = deezerState.ArtistName,
                IsPlaying = deezerState.IsPlaying,
                LastModifed = deezerState.UpdatedUtc,
                Loved = false,
                Radio = deezerState.StreamName,
                Song = deezerState.SongName
            };
            context.Clients.All.pandoraRefresh(pandoraInfo);

            var song = new SongDetails()
            {
                Album = pandoraInfo.Album,
                Artist = pandoraInfo.Artist,
                Song = pandoraInfo.Song
            };

            await _lastFMService.UpdateNowPlaying(song);

            var json = JsonConvert.SerializeObject(deezerState);
            File.WriteAllText("deezer.json", json, Encoding.UTF8);
        }
    }
}
