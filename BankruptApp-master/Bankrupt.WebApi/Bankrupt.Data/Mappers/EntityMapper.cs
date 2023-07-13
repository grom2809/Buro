using Bankrupt.Core.Entities;
using Bankrupt.Data.Models;

namespace Bankrupt.Data.Mappers
{
    public static class EntityMapper
    {
        public static UserMongoDb ToMongoDbEntity(this User user)
        {
            return new UserMongoDb() { Birthdate = new DateTime(user.Birthdate.Year, user.Birthdate.Month, user.Birthdate.Day), Id = user.Id, 
                Email = user.Email.Trim(), Name = user.Name.Trim(), Role = user==null ? Enum.GetName(Role.User) : Enum.GetName(user.Role), Password=user.Password.Trim() };
        }

        public static User ToEntity(this UserMongoDb user)
        {
            return new User() { Birthdate = DateOnly.FromDateTime(user.Birthdate), Id = user.Id, Email = user.Email.Trim(), Password = user.Password.Trim(), 
                Name = user.Name.Trim(), Role = user.Role == Enum.GetName(Role.Admin) ? Role.Admin : Role.User };
        }

        public static DocumentMongoDb ToMongoDbEntity(this Document document)
        {
            return new DocumentMongoDb() { Id=document.Id, FileName=document.FileName.Trim(), UserId=document.UserId, ObjectId= document.ObjectId,
                Date = new DateTime(document.Date.Year, document.Date.Month, document.Date.Day) };
        }

        public static Document ToEntity(this DocumentMongoDb document)
        {
            return new Document() {
                Id = document.Id,
                FileName = document.FileName.Trim(),
                UserId = document.UserId,
                ObjectId = document.ObjectId,
                Date = DateOnly.FromDateTime(document.Date)
            };
        }
    }
}