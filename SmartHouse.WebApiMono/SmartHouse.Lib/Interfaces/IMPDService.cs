using System;
using Libmpc;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
	public interface IMPDService : IDisposable
    {
		Result Play();

		Result Pause();

		Result Stop();

		Result Next();

		Result Previous();

		MpdStatus GetStatus();

		MpdFile GetCurrentSong();

        Task<SongResult> GetNowPlaying(bool lastFM);

        Task<Result> LoveSong();
    }
}
