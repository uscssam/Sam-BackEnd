using SAM.Entities;
using SAM.Repositories.Interfaces;
using SAM.Services.Interfaces;

namespace SAM.Api.Controllers
{
    public class UserController : BaseController<User>
    {
        public UserController(IService<User> service) : base(service)
        {
        }
    }
}
