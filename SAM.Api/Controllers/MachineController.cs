using AutoMapper;
using SAM.Services.Dto;
using SAM.Services.Interfaces;

namespace SAM.Api.Controllers
{
    public class MachineController : BaseController<MachineDto, MachineSearchDto>
    {
        public MachineController(IMapper mapper, IService<MachineDto> service) : base(mapper, service) { }
    }
}
