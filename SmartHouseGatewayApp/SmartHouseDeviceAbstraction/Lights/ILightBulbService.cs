using System.Threading.Tasks;

namespace SmartHouseDeviceAbstraction.Lights
{
    public interface ILightBulbService
    {
        Task TurnOnAsync();

        Task TurnOffAsync();

        Task ToogleAsync();
    }
}
