using AutoMapper;
using Microsoft.Extensions.Logging;
using SAM.Repositories.Interfaces;
using SAM.Services.Abstract;
using SAM.Services.Dto;
using System.Text;

namespace SAM.Services
{
    public class UserService : BaseService<UserDto, User>
    {
        private readonly ILogger logger;

        public UserService(ILogger<UserService> logger, IMapper mapper, IRepositoryDatabase<User> repository) : base(mapper, repository)
        {
            this.logger = logger;
        }

        public override UserDto Create(UserDto entity)
        {
            var user = repository.Search(x => x.UserName == entity.UserName).FirstOrDefault();
            if (user != null)
            {
                throw new ArgumentException("Nome do usuário já cadastrado");
            }
            entity.Password = BCrypt.Net.BCrypt.HashPassword(entity.Password, BCrypt.Net.BCrypt.GenerateSalt());
            return base.Create(entity);
        }

        public override UserDto Update(int id, UserDto entity)
        {
            if(string.IsNullOrEmpty(entity.Password))
            {
                var user = repository.Read(id);
                entity.Password = user.Password;
            }
            else
            {
                entity.Password = BCrypt.Net.BCrypt.HashPassword(entity.Password, BCrypt.Net.BCrypt.GenerateSalt());
                logger.LogInformation($"Senha do usuário {entity.UserName} alterada");
            }
            return base.Update(id, entity);
        }
    }
}
