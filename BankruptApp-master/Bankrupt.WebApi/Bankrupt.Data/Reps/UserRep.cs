using Bankrupt.Core;
using Bankrupt.Core.Entities;
using Bankrupt.Core.Exceptions;
using Bankrupt.Core.Reps;
using Bankrupt.Data.Mappers;
using Bankrupt.Data.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Bankrupt.Data.Reps
{
    public class UserRep : IUserRep
    {
        private readonly IMongoCollection<UserMongoDb> usersCollection;

        public UserRep(IOptions<BankruptDbSettings> bookStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(bookStoreDatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(bookStoreDatabaseSettings.Value.DatabaseName);
            usersCollection = mongoDatabase.GetCollection<UserMongoDb>(bookStoreDatabaseSettings.Value.UsersCollectionName);
        }

        public async Task<User> GetUser(string login, string password)
        {
            var userMongo = await usersCollection.Find(u => u.Email.Equals(login) && u.Password.Equals(password)).FirstOrDefaultAsync();
            if (userMongo == null)
                throw new ValidationException("Такого пользователя не существует", StatusCodes.Status409Conflict);
            return userMongo.ToEntity();
        }

        public async Task<User> GetUser(string email)
        {
            var userMongo = await usersCollection.Find(u => u.Email.Equals(email)).FirstOrDefaultAsync();
            if (userMongo == null)
                throw new ValidationException("Такого пользователя не существует", StatusCodes.Status409Conflict);
            return userMongo.ToEntity();
        }

        public async Task<User> GetUser(Guid id)
        {
            var userMongo = await usersCollection.Find(u => u.Id.Equals(id)).FirstOrDefaultAsync();
            if (userMongo == null)
                throw new ValidationException("Такого пользователя не существует", StatusCodes.Status409Conflict);
            return userMongo.ToEntity();
        }

        public async Task<List<User>> GetAllUsers()
        {
            var usersMongo = await usersCollection.Find(u => u.Role == Enum.GetName(Role.User)).ToListAsync();
            return usersMongo.Select(u => u.ToEntity()).ToList();
        }

        public async Task<Guid> AddUser(User user)
        {
            if (user == null)
                throw new ValidationException("Ошибка добавления пользователя", StatusCodes.Status409Conflict);
            await usersCollection.InsertOneAsync(user.ToMongoDbEntity());
            return user.Id;
        }

        public Task UpdateUser(User user)
        {
            return usersCollection.ReplaceOneAsync(u => u.Id.Equals(user.Id), user.ToMongoDbEntity());
        }

        public async Task UpdateUserRole(string email, Role role)
        {
            var userMongo = await usersCollection.Find(u => u.Email.Equals(email)).FirstOrDefaultAsync();
            if (userMongo == null)
                throw new ValidationException("Такого пользователя не существует", StatusCodes.Status409Conflict);

            userMongo.Role = Enum.GetName(role);
            await usersCollection.ReplaceOneAsync(u => u.Id.Equals(userMongo.Id), userMongo);
        }

        public async Task DeleteUserById(Guid id)
        {
            var result = await usersCollection.DeleteOneAsync(u => u.Id.Equals(id));
            if (result.DeletedCount == 0)
                throw new ValidationException("Такого пользователя не существует", StatusCodes.Status409Conflict);
        }

        public bool IsEmailUnique(string email, Guid id)
        {
            return !usersCollection.Find(u => u.Email.Equals(email) && !u.Id.Equals(id)).Any();
        }
    }
}
