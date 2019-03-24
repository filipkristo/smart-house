using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
    public interface ILastFMService
    {
        Task<string> StartScrobble(SongDetails song);
        Task<string> UpdateNowPlaying(SongDetails song);
        Task<string> LoveSong(string artistName, string trackName);
        Task<string> UnloveSong(string artistName, string trackName);
        Task<TrackInfoResponse> GetSongInfo(string artistName, string trackName);
        Task<IReadOnlyList<TrackInfoResponse>> GetTopTracks();
        Task<AlbumInfoResponse> GetAlbumInfo(string artist, string album);
        Task<ArtistInfoResponse> GetArtistInfo(string artist);
        Task<IReadOnlyList<ArtistInfoResponse>> GetSimilarArtist(string artist);
        Task<IReadOnlyList<ArtistTileData>> GetRecentTopArtists();
    }
}
