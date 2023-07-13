using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bankrupt.Data.Models
{
    public class DocumentMongoDb
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string FileName { get; set; }

        [BsonDateTimeOptions(DateOnly = true)]
        public DateTime Date { get; set; }

        public ObjectId ObjectId { get; set; }
    }
}
