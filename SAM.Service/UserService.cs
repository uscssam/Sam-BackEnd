using SAM.Entities;
using SAM.Repositories.Interfaces;
using SAM.Services.Abstract;

namespace SAM.Services
{
    public class UserService : BaseService<User>
    {
        public UserService(IRepositoryDatabase<User> repository) : base(repository)
        {
        }
    }
}
