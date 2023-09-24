using SAM.Entities;
using SAM.Repositories.Interfaces;

namespace SAM.Api.Controllers
{
    public class MachineController : BaseController<Machine>
    {
        public MachineController(IRepositoryDatabase<Machine> repository) : base(repository)
        {
        }
    }
}
