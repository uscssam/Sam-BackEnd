using SAM.Entities;
using SAM.Repositories.Abstract;
using SAM.Repositories.Database.Context;
using System.Diagnostics.CodeAnalysis;

namespace SAM.Repositories.Database
{
    [ExcludeFromCodeCoverage]
    public class OrderServiceRepository : RepositoryDatabase<OrderService>
    {
        public OrderServiceRepository(MySqlContext context) : base(context)
        {
        }
    }
}
