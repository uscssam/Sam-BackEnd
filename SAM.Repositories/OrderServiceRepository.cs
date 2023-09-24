using SAM.Entities;
using SAM.Repositories.Abstract;
using SAM.Repositories.Database.Context;

namespace SAM.Repositories.Database
{
    public class OrderServiceRepository : RepositoryDatabase<OrderService>
    {
        public OrderServiceRepository(MySqlContext context) : base(context)
        {
        }
    }
}
