using MongoDB.Bson.Serialization.Attributes;

namespace Bankrupt.Data.Models
{
    public class UserMongoDb
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        [BsonDateTimeOptions(DateOnly = true)]
        public DateTime Birthdate { get; set; }

        public string Role { get; set; }
    }
}
