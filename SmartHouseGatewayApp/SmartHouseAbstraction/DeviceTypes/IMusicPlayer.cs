using System.Threading.Tasks;

namespace SmartHouseCoreAbstraction.DeviceTypes
{
    public interface IMusicPlayer
    {
        Task PlayAsync();

        Task PauseAsync();

        Task StopAsync();
    }
}