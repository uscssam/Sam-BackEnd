using SAM.Entities;
using SAM.Repositories.Interfaces;

namespace SAM.Api.Controllers
{
    public class UnitController : BaseController<Unit>
    {
        public UnitController(IRepositoryDatabase<Unit> repository) : base(repository)
        {
        }
    }
}
