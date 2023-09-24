using SAM.Entities;
using SAM.Repositories.Abstract;
using SAM.Repositories.Database.Context;

namespace SAM.Repositories.Database
{
    public class MachineRepository : RepositoryDatabase<Machine>
    {
        public MachineRepository(MySqlContext context) : base(context)
        {
        }
    }
}
