using SAM.Entities;
using SAM.Repositories.Abstract;
using SAM.Repositories.Database.Context;
using System.Diagnostics.CodeAnalysis;

namespace SAM.Repositories.Database
{
    [ExcludeFromCodeCoverage]
    public class MachineRepository : RepositoryDatabase<Machine>
    {
        public MachineRepository(MySqlContext context) : base(context)
        {
        }
    }
}
