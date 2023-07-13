using Bankrupt.Auth.Common;
using Bankrupt.Auth.WebApi.Dto;
using Bankrupt.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Bankrupt.Auth.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private IAuthService authService;
        private readonly IOptions<AuthOptions> authOptions;

        public AuthController(ILogger<AuthController> logger, IAuthService authService, IOptions<AuthOptions> authOptions)
        {
            _logger = logger;
            this.authService = authService;
            this.authOptions = authOptions;
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            var user = await authService.AuthenticateUser(login.Email, login.Password);

            if (user != null) 
            {
                var token = authService.GeneretaeJWT(user, authOptions);
                return Ok(new {access_token = token, user_id = user.Id});
            }

            return Unauthorized();
        }
    }
}