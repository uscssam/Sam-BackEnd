using AutoMapper;
using SAM.Services.Dto;
using SAM.Services.Interfaces;

namespace SAM.Api.Controllers
{
    public class OrderServiceController : BaseController<OrderServiceDto, OrderServiceSearchDto>
    {
        public OrderServiceController(IMapper mapper, IService<OrderServiceDto> service) : base(mapper, service)
        {
        }
    }
}
