using Microsoft.AspNetCore.Mvc;
using SAM.Api.Token;
using SAM.Entities;

namespace SAM.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : Controller
    {
        private readonly GenerateToken _generateToken;

        public LoginController(GenerateToken generateToken)
        {
            _generateToken = generateToken;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] Authenticate authInfo)
        {
            var token = await _generateToken.GenerateJwt(authInfo);
            if(token == null)
            {
                return NotFound(new { Message = "Usuário ou senha Inválidos" });
            }
            return Ok(new { Token = token });
        }
    }
}
