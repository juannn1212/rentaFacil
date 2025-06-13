using AutoMapper;
using VehicleService.Domain.Entities;
using VehicleService.Application.DTOs;

namespace VehicleService.Application.Mappings
{
    public class VehicleProfile : Profile
    {
        public VehicleProfile()
        {
            CreateMap<Vehicle, VehicleDto>();
            CreateMap<CreateVehicleDto, Vehicle>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(_ => true));
            CreateMap<UpdateVehicleDto, Vehicle>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
