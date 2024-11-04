using Microsoft.AspNetCore.Mvc;
using SAM.Api.Token;
using SAM.Entities;

namespace SAM.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : Controller
    {
        private readonly ILogger logger;
        private readonly IGenerateToken _generateToken;

        public LoginController(ILogger<LoginController> logger, IGenerateToken generateToken)
        {
            this.logger = logger;
            _generateToken = generateToken;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] Authenticate authInfo)
        {
            var token = await _generateToken.GenerateJwt(authInfo);
            if(token == null)
            {
                logger.LogWarning($"Tentativa de login inválida para o usuário {authInfo.Username}.", DateTime.Now);
                return NotFound("Nome de usuário ou senha inválidos");
            }
            return Ok(new { Token = token });
        }
    }
}
