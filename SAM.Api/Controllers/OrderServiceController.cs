using SAM.Entities;
using SAM.Repositories.Interfaces;

namespace SAM.Api.Controllers
{
    public class OrderServiceController : BaseController<OrderService>
    {
        public OrderServiceController(IRepositoryDatabase<OrderService> repository) : base(repository)
        {
        }
    }
}
