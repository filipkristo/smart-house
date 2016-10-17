using System;
using System.Collections.Generic;

namespace SmartHouse.Lib
{
	public interface IPanodraService
	{
		Result Play();

		Result Pause();

		Result Start();

		Result Stop();

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
