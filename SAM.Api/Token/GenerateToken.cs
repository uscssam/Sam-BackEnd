using Microsoft.IdentityModel.Tokens;
using SAM.Entities;
using SAM.Repositories.Database;
using SAM.Repositories.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SAM.Api.Token
{
    public interface IGenerateToken
    {
        Task<string> GenerateJwt(Authenticate authenticate);
    }

    public class GenerateToken : IGenerateToken
    {
        private readonly TokenConfiguration _configuration;
        private readonly UserRepository _userRepository;

        public GenerateToken(TokenConfiguration configuration, IRepositoryDatabase<User> userRepository)
        {
            _configuration = configuration;
            _userRepository = (UserRepository)userRepository;
        }

        public async Task<string> GenerateJwt(Authenticate authenticate)
        {
            return await Task.FromResult(((Func<string>)(() =>
            {
                var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration.Secret!));
                var tokenHandler = new JwtSecurityTokenHandler();


                var user = _userRepository.Search(u => u.UserName == authenticate.Username).FirstOrDefault();

                if (user == null)
                    return null!;

                if(BCrypt.Net.BCrypt.Verify(authenticate.Password, user.Password) == false)
                    return null!;

                List<Claim> claims = new()
                {
                    new Claim("subject", _configuration.Subject!),
                    new Claim("module", _configuration.Module!),
                    new Claim("name", user.UserName),
                    new Claim("fullname", user.Fullname),
                    new Claim("role", user.Level.ToString()),
                    new Claim("idUser", user.Id.ToString())
                };

                var jwtToken = new JwtSecurityToken(
                    issuer: _configuration.Issuer,
                    audience: _configuration.Audience,
                    claims: claims,
                    expires: DateTime.Now.AddHours(_configuration.ExpirationtimeInHours),
                    signingCredentials: new SigningCredentials(securityKey, "HS256"));

                return tokenHandler.WriteToken(jwtToken);
            }))());
        }
    }
}
