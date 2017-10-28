using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IF.Lastfm.Core.Objects;

namespace SmartHouse.Lib
{
	public interface ILastFMService : IDisposable
	{
        string StartScrobbleBash(SongDetails song);
        Task<string> StartScrobble(SongDetails song);
		Task<string> UpdateNowPlaying(SongDetails song);
		Task<string> LoveSong(string artistName, string trackName);
        Task<string> UnloveSong(string artistName, string trackName);
        Task<LastTrack> GetSongInfo(string artistName, string trackName);
		Task<List<LastTrack>> GetTopTracks();
        Task<LastAlbum> GetAlbumInfo(string artist, string album);
        Task<LastArtist> GetArtistInfo(string artist);
        Task<IEnumerable<LastArtist>> GetSimilarArtist(string artist, int limit = 50);
	    Task<IEnumerable<ArtistTileData>> GetRecentTopArtists(DateTimeOffset since, int count);
	}
}
