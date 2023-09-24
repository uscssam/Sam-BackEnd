using SAM.Entities;
using SAM.Repositories.Abstract;
using SAM.Repositories.Database.Context;

namespace SAM.Repositories.Database
{
    public class UserRepository : RepositoryDatabase<User>
    {
        public UserRepository(MySqlContext context) : base(context)
        {
        }
    }
}
