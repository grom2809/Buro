using Bankrupt.Core.Entities;

namespace Bankrupt.Core.Reps
{
    public interface IUserRep
    {
        Task<User> GetUser(string login, string password);
        Task<User> GetUser(Guid id);
        Task<List<User>> GetAllUsers();
        Task<Guid> AddUser(User user);
        Task UpdateUser(User user);
        Task DeleteUserById(Guid id);
        bool IsEmailUnique(string email, Guid id);
    }
}
