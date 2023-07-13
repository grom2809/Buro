using Bankrupt.Auth.Common;
using Bankrupt.Core.Entities;
using Microsoft.Extensions.Options;

namespace Bankrupt.Core.Services
{
    public interface IAuthService
    {
        Task<User> AuthenticateUser(string email, string password);
        string GeneretaeJWT(User user, IOptions<AuthOptions> authOptions);
    }
}