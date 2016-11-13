using System;
using Libmpc;

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
	}
}
