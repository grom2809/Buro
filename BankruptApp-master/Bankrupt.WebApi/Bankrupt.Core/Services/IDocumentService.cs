using Bankrupt.Core.Entities;

namespace Bankrupt.Core.Services
{
    public interface IDocumentService
    {
        Task<List<Document>> GetUserDocuments(Guid userId);
        Task<Document> GetDocument(Guid id, Guid userId);
        Task<Guid> AddDocument(Guid userId, Document document);
        Task DeleteDocument(Guid id, Guid userId);
    }
}