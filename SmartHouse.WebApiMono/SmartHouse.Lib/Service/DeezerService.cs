using Newtonsoft.Json;
using SmartHouse.Lib.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
    public class DeezerService : BasePlayerService, IPlayerService
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        static DeezerService()
        {
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.BaseAddress = new Uri("http://10.110.166.91/");
            _httpClient.Timeout = TimeSpan.FromSeconds(5);
        }

        public override Result ChangeStation(string stationId)
        {
            SendUdpCommand($"aradio;{stationId}");
            return GetOkResult();
        }

        public override PandoraResult GetCurrentSongInfo()
        {
            var json = File.ReadAllText("deezer.json");
            var deezerState = JsonConvert.DeserializeObject<DeezerState>(json);

            return new PandoraResult
            {
                Album = deezerState.AlbumName,
                AlbumUri = deezerState.AlbumUri,
                Artist = deezerState.ArtistName,
                DurationSeconds = 0,
                IsPlaying = deezerState.IsPlaying,
                LastModifed = deezerState.UpdatedUtc,
                Loved = false,
                Radio = deezerState.StreamName,
                Song = deezerState.SongName
            };
        }

        public async Task<PandoraResult> GetNowPlaying()
        {
            var json = await _httpClient.GetStringAsync("api/Player/State").ConfigureAwait(false);
            var deezerState = JsonConvert.DeserializeObject<DeezerState>(json);

            File.WriteAllText("deezer.json", json, Encoding.UTF8);

            return new PandoraResult
            {
                Album = deezerState.AlbumName,
                AlbumUri = deezerState.AlbumUri,
                Artist = deezerState.ArtistName,
                DurationSeconds = 0,
                IsPlaying = deezerState.IsPlaying,
                LastModifed = deezerState.UpdatedUtc,
                Loved = false,
                Radio = deezerState.StreamName,
                Song = deezerState.SongName
            };
        }

        public override async Task<IEnumerable<KeyValue>> GetStationList()
        {
            var json = await _httpClient.GetStringAsync("api/Deezer/FavoriteArtists").ConfigureAwait(false);
            var favorites = JsonConvert.DeserializeObject<IEnumerable<DeezerFavoriteStation>>(json);

            return favorites.Select(x => new KeyValue
            {
                Key = x.Id.ToString(),
                Value = x.Name
            });
        }

        public async Task<bool> IsPlaying()
        {
            var state = await GetNowPlaying().ConfigureAwait(false);
            return state.IsPlaying;
        }

        public async Task<Result> LoveSong()
        {
            var lastFMService = new LastFMService();

            var songInfo = await GetNowPlaying().ConfigureAwait(false);

            if (songInfo.Loved)
                return new Result()
                {
                    Ok = true,
                    ErrorCode = 0,
                    Message = "You already liked this song"
                };

            var status = await lastFMService.LoveSong(songInfo.Artist, songInfo.Song).ConfigureAwait(false);
            ThumbUp();

            return new Result
            {
                Ok = true,
                ErrorCode = 0,
                Message = $"You liked {songInfo.Song} song. Status: {status}"
            };
        }

        public Result Next()
        {
            SendUdpCommand("next");
            return GetOkResult();
        }

        public Result Pause()
        {
            SendUdpCommand("pause");
            return GetOkResult();
        }

        public Result Play()
        {
            SendUdpCommand("play");
            return GetOkResult();
        }

        public Result Prev()
        {
            SendUdpCommand("prev");
            return GetOkResult();
        }

        public Task<Result> Restart()
        {
            return Task.FromResult(GetOkResult());
        }

        public Task<Result> RestartTcp()
        {
            return Task.FromResult(GetOkResult());
        }

        public Task<Result> Start()
        {
            return Task.FromResult(GetOkResult());
        }

        public Task<Result> StartTcp()
        {
            return Task.FromResult(GetOkResult());
        }

        public Result Stop()
        {
            SendUdpCommand("stop");
            return GetOkResult();
        }

        public Task<Result> StopTcp()
        {
            return Task.FromResult(GetOkResult());
        }

        public Result ThumbDown()
        {
            SendUdpCommand("dislike");
            return GetOkResult();
        }

        public Result ThumbUp()
        {
            SendUdpCommand("love");
            return GetOkResult();
        }

        public Result TiredOfThisSong()
        {
            return GetOkResult();
        }

        public Result VolumeDown()
        {
            SendUdpCommand("volumedown");
            return GetOkResult();
        }

        public Result VolumeUp()
        {
            SendUdpCommand("volumeup");
            return GetOkResult();
        }

        private static void SendUdpCommand(string command)
        {
            using (var udpClient = new UdpClient("10.110.166.91", 9876))
            {
                var bytes = Encoding.UTF8.GetBytes(command);
                udpClient.Send(bytes, bytes.Length);
            }
        }

        private static Result GetOkResult()
        {
            return new Result
            {
                ErrorCode = 0,
                Message = "Ok",
                Ok = true
            };
        }

        public string GetAmplifierInput() => "HDMI3";
    }
}
