using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IF.Lastfm.Core.Objects;

namespace SmartHouse.Lib
{
	public interface ILastFMService
	{
		Task<string> StartScrobble(SongDetails song);
		Task<string> UpdateNowPlaying(SongDetails song);
		Task<string> LoveSong(string artistName, string trackName);
		Task<LastTrack> GetInfo(string artistName, string trackName);
		Task<List<LastTrack>> GetTopTracks();
	}
}
