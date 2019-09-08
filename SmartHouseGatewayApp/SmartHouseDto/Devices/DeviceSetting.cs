using System;
namespace SmartHouseDto.Devices
{
    public class DeviceSetting
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public string Value { get; set; }

        public string Description { get; set; }

        public int DeviceId { get; set; }
    }
}
