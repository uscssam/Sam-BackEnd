using SAM.Entities;
using SAM.Repositories.Interfaces;
using SAM.Services.Abstract;

namespace SAM.Services
{
    public class UserService : BaseService<User>
    {
        public UserService(IRepositoryDatabase<User> repository) : base(repository) { }

        public override User Create(User entity)
        {
            var user = repository.Search(x => x.UserName == entity.UserName).FirstOrDefault();
            if (user != null)
            {
                throw new ArgumentException("Nome do usuário já cadastrado");
            }
            return base.Create(entity);
        }

        public override User Update(User entity)
        {
            if(string.IsNullOrEmpty(entity.Password))
            {
                var user = repository.Read(entity.Id);
                entity.Password = user.Password;
            }
            return base.Update(entity);
        }
    }
}
