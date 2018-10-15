using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouseLambda.Service.Interfaces
{
    public interface ISmartHouseService
    {
        Task TurnOnSmartHouse();

        Task TurnOffSmartHouse();

        Task TurnOnAirConditioner();

        Task TurnOffAirConditioner();

        Task Play();

        Task Pause();

        Task NextSong();

        Task VolumeUp();

        Task VolumeDown();

        Task Louder();

        Task ToLouder();

        Task LoveSong();

        Task SetVolume(int volume);

        Task Mute();

        Task TiredOfThisSong();

        Task Ban();

        Task Pandora();

        Task TV();
    }
}
