using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
	public interface IPanodraService
	{
		Result Play();

		Result Pause();

		Result Start();

		Result Stop();

		Result Restart();

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

		PandoraResult GetCurrentSongInfo();

		IEnumerable<KeyValue> GetStationList();

		bool IsPlaying();
	}
}
