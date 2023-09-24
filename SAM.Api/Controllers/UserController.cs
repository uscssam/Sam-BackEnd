using SAM.Entities;
using SAM.Repositories.Interfaces;

namespace SAM.Api.Controllers
{
    public class UserController : BaseController<User>
    {
        public UserController(IRepositoryDatabase<User> repository) : base(repository)
        {
        }
    }
}
