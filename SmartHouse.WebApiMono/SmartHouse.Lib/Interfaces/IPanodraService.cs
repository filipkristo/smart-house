using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
	public interface IPanodraService
	{
		Result Play();

		Result Pause();

		Task<Result> Start();

		Result Stop();

		Task<Result> Restart();

		Task<Result> StartTcp();

		Task<Result> StopTcp();

		Task<Result> RestartTcp();

		Result Next();

		Result ThumbUp();

		Result ThumbDown();

		Result TiredOfThisSong();

		Result VolumeUp();

		Result VolumeDown();

		Result ChangeStation(string stationId);

		Result NextStation();

		Result PrevStation();

		PandoraResult GetCurrentSongInfo();

        Task<PandoraResult> GetNowPlaying();

        Task<Result> LoveSong();

        IEnumerable<KeyValue> GetStationList();

		bool IsPlaying();
	}
}
