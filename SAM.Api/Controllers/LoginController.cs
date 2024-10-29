using Microsoft.AspNetCore.Mvc;
using SAM.Api.Token;
using SAM.Entities;

namespace SAM.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : Controller
    {
        private readonly IGenerateToken _generateToken;

        public LoginController(IGenerateToken generateToken)
        {
            _generateToken = generateToken;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] Authenticate authInfo)
        {
            var token = await _generateToken.GenerateJwt(authInfo);
            if(token == null)
            {
                return NotFound("Nome de usuário ou senha inválidos");
            }
            return Ok(new { Token = token });
        }
    }
}
