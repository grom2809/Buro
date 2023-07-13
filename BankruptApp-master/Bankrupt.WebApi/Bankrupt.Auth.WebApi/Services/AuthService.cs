using Bankrupt.Auth.Common;
using Bankrupt.Core;
using Bankrupt.Core.Entities;
using Bankrupt.Core.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Bankrupt.Auth.WebApi.Services
{
    public class AuthService : IAuthService
    {
        public IUserService userService;

        public AuthService(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task<User> AuthenticateUser(string email, string password)
        {
            return await userService.GetUser(email, password);
        }

        public string GeneretaeJWT(User user, IOptions<AuthOptions> authOptions)
        {
            var authParams = authOptions.Value;

            var securityKey = authParams.GetSymmetricSecurityKey();
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim("role", user.Role.ToString())
            };

            var token = new JwtSecurityToken(authParams.Issuer, authParams.Audience, claims,
                expires: DateTime.Now.AddSeconds(authParams.TokenLifetime), signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
