using Bankrupt.Core.Entities;
using Bankrupt.WebApi.Dto;

namespace Bankrupt.WebApi.Mappers
{
    public static class EntityMapper
    {
        public static UserDto ToDto(this User user)
        {
            return new UserDto() { Birthdate = user.Birthdate.ToString("dd.MM.yyyy"), Id = user.Id, Email = user.Email.Trim(), 
                Name = user.Name.Trim(), Role = Enum.GetName(user.Role) };
        }

        public static User ToEntity(this UserUpdateDto user)
        {
            return new User() { Birthdate = user.Birthdate, Id = Guid.Parse(user.Id), Email = user.Email.Trim(), Name = user.Name.Trim(), Password=user.Password };
        }

        public static User ToEntity(this UserRegisterDto user)
        {
            return new User()
            {
                Birthdate = user.Birthdate,
                Id = Guid.NewGuid(),
                Email = user.Email.Trim(),
                Name = user.Name.Trim(),
                Role = Role.User,
                Password = user.Password.Trim()
            };
        }

        public static DocumentDto ToDto(this Document document)
        {
            return new DocumentDto() { Id=document.Id, Data=document.Data, FileName=document.FileName.Trim(), UserId=document.UserId, Date=document.Date.ToString("dd.MM.yyyy") };
        }
    }
}