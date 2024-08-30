using SAM.Entities;
using SAM.Repositories.Interfaces;
using SAM.Services.Abstract;

namespace SAM.Service
{
    public class MachineService : BaseService<Machine>
    {
        public MachineService(IRepositoryDatabase<Machine> repository) : base(repository) { }

        public List<Machine> ListByUnit(int unitId)
        {
            return repository.Search(m => m.Id == unitId);
        }
    }
}
