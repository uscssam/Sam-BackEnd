using AutoMapper;
using SAM.Services.Dto;
using SAM.Services.Interfaces;

namespace SAM.Api.Controllers
{
    public class UnitController : BaseController<UnitDto, UnitSearchDto>
    {
        public UnitController(IMapper mapper, IService<UnitDto> service) : base(mapper, service)
        {
        }
    }
}
