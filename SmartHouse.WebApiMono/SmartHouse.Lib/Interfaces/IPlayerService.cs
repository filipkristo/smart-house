using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
    public interface IPlayerService
    {
        Result Play();

        Result Pause();

        Task<Result> Start();

        Result Stop();

        Task<Result> Restart();

        Task<Result> StartTcp();

        Task<Result> StopTcp();

        Task<Result> RestartTcp();

        Result Prev();

        Result Next();

        Result ThumbUp();

        Result ThumbDown();

        Result TiredOfThisSong();

        Result VolumeUp();

        Result VolumeDown();

        Result ChangeStation(string stationId);

        Task<Result> NextStation();

        Task<Result> PrevStation();

        PandoraResult GetCurrentSongInfo();

        Task<PandoraResult> GetNowPlaying();

        Task<Result> LoveSong();

        Task<IEnumerable<KeyValue>> GetStationList();

        Task<bool> IsPlaying();

        string GetAmplifierInput();
    }
}
