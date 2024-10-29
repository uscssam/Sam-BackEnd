using AutoMapper;
using SAM.Repositories.Interfaces;
using SAM.Services.Abstract;
using SAM.Services.Dto;

namespace SAM.Services
{
    public class UserService : BaseService<UserDto, User>
    {
        public UserService(IMapper mapper, IRepositoryDatabase<User> repository) : base(mapper, repository) { }

        public override UserDto Create(UserDto entity)
        {
            var user = repository.Search(x => x.UserName == entity.UserName).FirstOrDefault();
            if (user != null)
            {
                throw new ArgumentException("Nome do usuário já cadastrado");
            }
            return base.Create(entity);
        }

        public override UserDto Update(int id, UserDto entity)
        {
            if(string.IsNullOrEmpty(entity.Password))
            {
                var user = repository.Read(id);
                entity.Password = user.Password;
            }
            return base.Update(id, entity);
        }
    }
}
