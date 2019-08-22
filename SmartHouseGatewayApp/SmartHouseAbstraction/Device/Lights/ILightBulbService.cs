using System.Threading.Tasks;

namespace SmartHouseAbstraction.Device.Lights
{
    public interface ILightBulbService
    {
        Task TurnOnAsync();

        Task TurnOffAsync();

        Task ToogleAsync();
    }
}
