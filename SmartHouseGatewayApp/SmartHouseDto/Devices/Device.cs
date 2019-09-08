using System;
using System.Collections.Generic;

namespace SmartHouseDto.Devices
{
    public class Device
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public string CrlAssembly { get; set; }

        public string CrlType { get; set; }

        public string Description { get; set; }

        public int DeviceTypeId { get; set; }

        public DeviceType DeviceType { get; set; }

        public IReadOnlyList<DeviceState> DeviceStates { get; set; }

        public IReadOnlyList<DeviceSetting> DeviceSettings { get; set; }
    }
}
