using AutoMapper;
using SAM.Entities;
using SAM.Services.Dto;

namespace SAM.Services.AutoMapper;
public class MapperProfile : Profile
{

    public MapperProfile()
    {
        CreateMap<Machine, MachineDto>()
            .ReverseMap();
        CreateMap<OrderService, OrderServiceDto>()
            .ReverseMap();
        CreateMap<User, UserDto>()
            .ReverseMap();
        CreateMap<Unit, UnitDto>()
            .ReverseMap();
        CreateMap<MachineSearchDto, MachineDto>();
        CreateMap<OrderServiceSearchDto, OrderServiceDto>();
        CreateMap<UserSearchDto, UserDto>();
        CreateMap<UnitSearchDto, UnitDto>();
    }
}
