using System;
using System.Net;
using Libmpc;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
    public class MPDService : IMPDService
    {
        private readonly Mpc MpdClient = new Mpc();

        public MPDService()
        {
            MpdClient.Connection = new MpcConnection(new System.Net.IPEndPoint(IPAddress.Parse("10.110.166.90"), 6600));
        }

        public Result Play()
        {
            MpdClient.Play();

            return new Result()
            {
                ErrorCode = 0,
                Message = "Ok",
                Ok = true
            };
        }

        public Result Pause()
        {
            MpdClient.Pause(true);

            return new Result()
            {
                ErrorCode = 0,
                Message = "Ok",
                Ok = true
            };
        }

        public Result Stop()
        {
            MpdClient.Stop();

            return new Result()
            {
                ErrorCode = 0,
                Message = "Ok",
                Ok = true
            };
        }

        public Result Next()
        {
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
            return MpdClient.Status();
        }

        public MpdFile GetCurrentSong()
        {
            return MpdClient.CurrentSong();
        }

        public async Task<SongResult> GetNowPlaying()
        {
            var song = GetCurrentSong();
            var status = GetStatus();
            var lastFMService = new LastFMService();

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

            result.Loved = (await lastFMService.GetSongInfo(result.Artist, result.Song))?.IsLoved ?? false;
            result.AlbumUri = (await lastFMService.GetAlbumInfo(result.Artist, result.Album))?.Images?.Large?.ToString();

            return result;
        }

        public async Task<Result> LoveSong()
        {
            var lastFMService = new LastFMService();
            var mpdInfo = await GetNowPlaying();
            var mpdSong = GetCurrentSong();

            if (mpdInfo.Loved)
                return new Result()
                {
                    Ok = true,
                    ErrorCode = 0,
                    Message = "You already liked this song"
                };

            var status = await lastFMService.LoveSong(mpdInfo.Artist, mpdInfo.Song);            

            // TODO: Add loved song to genre playlist
            //MpdClient.ListPlaylist(mpdSong.Genre);
            //MpdClient.PlaylistAdd(mpdInfo.Genre, mpdSong.File);

            return new Result
            {
                Ok = true,
                ErrorCode = 0,
                Message = $"You liked {mpdInfo.Song} song. Status: {status}"
            };
        }

        public void Dispose()
        {
            MpdClient?.Connection.Disconnect();
        }
    }
}
