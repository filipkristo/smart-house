using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
    public abstract class BasePlayerService
    {
        public abstract PandoraResult GetCurrentSongInfo();

        public abstract Result ChangeStation(string stationId);

        public abstract Task<IEnumerable<KeyValue>> GetStationList();

        public async Task<Result> NextStation()
        {
            var info = GetCurrentSongInfo();
            var stations = (await GetStationList().ConfigureAwait(false)).OrderBy(x => x.Value).ToList();

            var currentStation = stations.Find(x => x.Value.Contains(info.Radio));
            var currentStationIndex = stations.IndexOf(currentStation);

            var nextStation = default(KeyValue);

            if (currentStationIndex == stations.Count - 1)
                nextStation = stations[0];
            else
                nextStation = stations[currentStationIndex + 1];

            ChangeStation(nextStation.Key);

            return new Result()
            {
                ErrorCode = 0,
                Message = $"Starting to play {nextStation.Value} station",
                Ok = true
            };
        }

        public async Task<Result> PrevStation()
        {
            var info = GetCurrentSongInfo();
            var stations = (await GetStationList().ConfigureAwait(false)).OrderBy(x => x.Value).ToList();

            var currentStation = stations.Find(x => x.Value.Contains(info.Radio));
            var currentStationIndex = stations.IndexOf(currentStation);

            var nextStation = default(KeyValue);

            if (currentStationIndex == 0)
                nextStation = stations.Last();
            else
                nextStation = stations[currentStationIndex - 1];

            ChangeStation(nextStation.Key);

            return new Result()
            {
                ErrorCode = 0,
                Message = $"Starting to play {nextStation.Value} station",
                Ok = true
            };
        }

    }
}
