using SAM.Repositories.Abstract;
using SAM.Repositories.Database.Context;
using System.Diagnostics.CodeAnalysis;

namespace SAM.Repositories.Database
{
    [ExcludeFromCodeCoverage]
    public class UserRepository : RepositoryDatabase<User>
    {
        public UserRepository(MySqlContext context) : base(context) { }
    }
}
