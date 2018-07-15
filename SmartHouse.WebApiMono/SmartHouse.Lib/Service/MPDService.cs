using System;
using System.Net;
using Libmpc;
using System.Threading.Tasks;
using System.Linq;

namespace SmartHouse.Lib
{
    public class MPDService : IMPDService
    {
        private readonly Mpc MpdClient;

        public MPDService()
        {
            MpdClient = new Mpc();
        }

        private void ConnectIfNotConnected()
        {
            try
            {
                if (MpdClient.Connected)
                    return;

                MpdClient.Connection = new MpcConnection(new IPEndPoint(IPAddress.Loopback, 6600));
            }
            catch (Exception ex)
            {
                Logger.LogErrorMessage("Mpd Error", ex);
                throw;
            }
        }

        public Result Play()
        {
            //ConnectIfNotConnected();
            //MpdClient.Play();

            return new Result()
            {
                ErrorCode = 0,
                Message = "Ok",
                Ok = true
            };
        }

        public Result Pause()
        {
            //ConnectIfNotConnected();
            //MpdClient.Pause(true);

            return new Result()
            {
                ErrorCode = 0,
                Message = "Ok",
                Ok = true
            };
        }

        public Result Stop()
        {
            //ConnectIfNotConnected();
            //MpdClient.Stop();

            return new Result()
            {
                ErrorCode = 0,
                Message = "Ok",
                Ok = true
            };
        }

        public Result Next()
        {
            ConnectIfNotConnected();
            MpdClient.Next();

            return new Result()
            {
                ErrorCode = 0,
                Message = "Ok",
                Ok = true
            };
        }

        public Result Previous()
        {
            ConnectIfNotConnected();
            MpdClient.Previous();

            return new Result()
            {
                ErrorCode = 0,
                Message = "Ok",
                Ok = true
            };
        }

        public MpdStatus GetStatus()
        {
            ConnectIfNotConnected();
            return MpdClient.Status();
        }

        public MpdFile GetCurrentSong()
        {
            ConnectIfNotConnected();
            return MpdClient.CurrentSong();
        }

        public async Task<SongResult> GetNowPlaying(bool lastFM)
        {
            ConnectIfNotConnected();

            var song = GetCurrentSong();
            var status = GetStatus();

            using (var lastFMService = new LastFMService())
            {
                var result = new SongResult()
                {
                    Album = song.Album,
                    AlbumUri = null,
                    Artist = song.Artist,
                    DurationSeconds = status.TimeTotal,
                    Loved = false,
                    PlayedSeconds = status.TimeElapsed,
                    Song = song.Title,
                    Genre = song.Genre
                };

                if (lastFM)
                {
                    result.Loved = (await lastFMService.GetSongInfo(result.Artist, result.Song))?.IsLoved ?? false;
                    result.AlbumUri = (await lastFMService.GetAlbumInfo(result.Artist, result.Album))?.Images?.Large?.ToString();
                }

                return result;
            }
        }

        public async Task<Result> LoveSong()
        {
            ConnectIfNotConnected();

            using (var lastFMService = new LastFMService())
            {
                var mpdInfo = await GetNowPlaying(false);
                var mpdSong = GetCurrentSong();

                if (mpdInfo.Loved)
                    return new Result()
                    {
                        Ok = true,
                        ErrorCode = 0,
                        Message = "You already liked this song"
                    };

                var status = await lastFMService.LoveSong(mpdInfo.Artist, mpdInfo.Song);
                SaveToFavoritesPlaylist(mpdSong);

                return new Result
                {
                    Ok = true,
                    ErrorCode = 0,
                    Message = $"You liked {mpdInfo.Song} song. Status: {status}"
                };
            }            
        }

        private void SaveToFavoritesPlaylist(MpdFile mpdSong)
        {
            var playlistName = mpdSong.HasGenre ? mpdSong.Genre : "Favorites";

            if (!ExistsInPlaylist(playlistName, mpdSong.File))
            {
                MpdClient.PlaylistAdd(playlistName, mpdSong.File);
            }
        }

        private bool ExistsInPlaylist(string playlistName, string fileName)
        {
            try
            {
                return MpdClient.ListPlaylist(playlistName).Any(x => x == fileName);
            }
            catch { }

            return false;
        }

        public void Dispose()
        {
            if(MpdClient.Connected)
                MpdClient.Connection.Disconnect();            
        }
    }
}
