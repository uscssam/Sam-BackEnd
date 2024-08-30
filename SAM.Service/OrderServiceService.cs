using SAM.Entities;
using SAM.Repositories.Interfaces;
using SAM.Services.Abstract;

namespace SAM.Services
{
    public class OrderServiceService : BaseService<OrderService>
    {
        public OrderServiceService(IRepositoryDatabase<OrderService> repository) : base(repository)
        {
        }
    }
}
