namespace Bankrupt.WebApi.Dto
{
    public class DocumentDto
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string FileName { get; set; }

        public string Date { get; set; }

        public byte[] Data { get; set; }
    }
}
