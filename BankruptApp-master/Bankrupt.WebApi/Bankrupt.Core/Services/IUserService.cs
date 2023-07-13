using Bankrupt.Core.Entities;

namespace Bankrupt.Core.Services
{
    public interface IUserService
    {
        Task<User> GetUser(string login, string password);
        Task<User> GetUser(Guid id);
        Task<List<User>> GetAllUsers();
        Task<Guid> AddUser(User user);
        Task UpdateUser(User user);
        Task DeleteUserById(Guid id);
    }
}
