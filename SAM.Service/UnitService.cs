using SAM.Entities;
using SAM.Repositories.Interfaces;
using SAM.Services.Abstract;

namespace SAM.Services
{
    public class UnitService : BaseService<Unit>
    {
        public UnitService(IRepositoryDatabase<Unit> repository) : base(repository)
        {
        }
    }
}
