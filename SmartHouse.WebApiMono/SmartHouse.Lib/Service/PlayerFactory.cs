using SmartHouse.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
    public class PlayerFactory : IPlayerFactoryService
    {
        private readonly IPlayerService _playerService;

        public PlayerFactory()
        {
            _playerService = new DeezerService();
        }

        public Result ChangeStation(string stationId) => _playerService.ChangeStation(stationId);

        public string GetAmplifierInput() => _playerService.GetAmplifierInput();

        public PandoraResult GetCurrentSongInfo() => _playerService.GetCurrentSongInfo();

        public Task<PandoraResult> GetNowPlaying() => _playerService.GetNowPlaying();

        public Task<IEnumerable<KeyValue>> GetStationList() => _playerService.GetStationList();

        public Task<bool> IsPlaying() => _playerService.IsPlaying();

        public Task<Result> LoveSong() => _playerService.LoveSong();

        public Result Next() => _playerService.Next();

        public Task<Result> NextStation() => _playerService.NextStation();

        public Result Pause() => _playerService.Pause();

        public Result Play() => _playerService.Play();

        public Result Prev() => _playerService.Prev();

        public Task<Result> PrevStation() => _playerService.PrevStation();

        public Task<Result> Restart() => _playerService.Restart();

        public Task<Result> RestartTcp() => _playerService.RestartTcp();

        public Task<Result> Start() => _playerService.Start();

        public Task<Result> StartTcp() => _playerService.StartTcp();

        public Result Stop() => _playerService.Stop();

        public Task<Result> StopTcp() => _playerService.StopTcp();

        public Result ThumbDown() => _playerService.ThumbDown();

        public Result ThumbUp() => _playerService.ThumbUp();

        public Result TiredOfThisSong() => _playerService.TiredOfThisSong();

        public Result VolumeDown() => _playerService.VolumeDown();

        public Result VolumeUp() => _playerService.VolumeUp();
    }
}
