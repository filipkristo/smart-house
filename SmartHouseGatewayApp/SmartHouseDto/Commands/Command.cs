using SmartHouseDto.Devices;

namespace SmartHouseDto.Commands
{
    public class Command
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Device Device { get; set; }
    }
}
