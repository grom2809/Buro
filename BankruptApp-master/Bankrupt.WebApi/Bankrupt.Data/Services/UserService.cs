using Bankrupt.Core;
using Bankrupt.Core.Entities;
using Bankrupt.Core.Exceptions;
using Bankrupt.Core.Reps;
using Bankrupt.Core.Services;

namespace Bankrupt.Data.Services
{
    public class UserService : IUserService
    {
        private IUserRep userRep;

        public UserService(IUserRep userRep)
        {
            this.userRep = userRep;
        }

        public Task<List<User>> GetAllUsers()
        {
            return userRep.GetAllUsers();
        }

        public Task<User> GetUser(string login, string password)
        {
            return userRep.GetUser(login, password);
        }

        public Task<User> GetUser(Guid id)
        {
            return userRep.GetUser(id);
        }

        public async Task UpdateUser(User user)
        {
            if (user == null) 
                throw new ValidationException("Нет пользователя для добавления", StatusCodes.Status409Conflict);

            var isEmailUnique = userRep.IsEmailUnique(user.Email, user.Id);
            if (!isEmailUnique)
                throw new ValidationException("Пользователь с таким email уже существует", StatusCodes.Status409Conflict);

            var existingUser = await userRep.GetUser(user.Id);
            if (existingUser == null)
                throw new ValidationException("Такого пользователя не существует", StatusCodes.Status409Conflict);

            var password = string.IsNullOrWhiteSpace(user.Password) ? existingUser.Password : user.Password;
            user.Role = existingUser.Role;
            user.Password = password;
            await userRep.UpdateUser(user);
        }

        public Task<Guid> AddUser(User user)
        {
            if (user == null)
                throw new ValidationException("Нет пользователя для добавления", StatusCodes.Status409Conflict);

            var isEmailUnique = userRep.IsEmailUnique(user.Email, user.Id);
            if (!isEmailUnique)
                throw new ValidationException("Пользователь с таким email уже существует", StatusCodes.Status409Conflict);

            return userRep.AddUser(user);
        }

        public Task DeleteUserById(Guid id)
        {
            return userRep.DeleteUserById(id);
        }
    }
}
