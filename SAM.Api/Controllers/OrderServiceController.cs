using SAM.Entities;
using SAM.Services.Interfaces;

namespace SAM.Api.Controllers
{
    public class OrderServiceController : BaseController<OrderService>
    {
        public OrderServiceController(IService<OrderService> service) : base(service)
        {
        }
    }
}
