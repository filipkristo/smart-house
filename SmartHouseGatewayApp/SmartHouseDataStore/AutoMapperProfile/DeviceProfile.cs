using System;
using AutoMapper;
using dto = SmartHouseDto.Devices;
using entity = SmartHouseDataStore.Entities;

namespace SmartHouseDataStore.AutoMapperProfile
{
    public class DeviceProfile : Profile
    {
        public DeviceProfile()
        {
            CreateMap<entity.Device, dto.Device>();
            CreateMap<entity.DeviceSetting, dto.DeviceState>();
            CreateMap<entity.DeviceType, dto.DeviceType>();
            CreateMap<entity.DeviceSetting, dto.DeviceSetting>();
        }   
    }
}
