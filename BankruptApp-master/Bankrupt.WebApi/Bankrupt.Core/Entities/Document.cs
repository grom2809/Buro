using MongoDB.Bson;
namespace Bankrupt.Core.Entities
{
    public class Document
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string FileName { get; set; }

        public DateOnly Date { get; set; }

        public byte[] Data { get; set; }

        public ObjectId ObjectId { get; set; }
    }
}
