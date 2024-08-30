using SAM.Entities;
using SAM.Repositories.Interfaces;
using SAM.Services.Interfaces;

namespace SAM.Api.Controllers
{
    public class UnitController : BaseController<Unit>
    {
        public UnitController(IService<Unit> service) : base(service)
        {
        }
    }
}
