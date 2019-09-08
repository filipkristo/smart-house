using System;
using dto = SmartHouseDto.Commands;
using entity = SmartHouseDataStore.Entities;

using AutoMapper;

namespace SmartHouseDataStore.AutoMapperProfile
{
    public class CommandProfile : Profile
    {
        public CommandProfile()
        {
            CreateMap<entity.Command, dto.Command>();            
        }
    }
}
