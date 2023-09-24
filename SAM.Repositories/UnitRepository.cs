using SAM.Entities;
using SAM.Repositories.Abstract;
using SAM.Repositories.Database.Context;

namespace SAM.Repositories.Database
{
    public class UnitRepository : RepositoryDatabase<Unit>
    {
        public UnitRepository(MySqlContext context) : base(context)
        {
        }
    }
}
