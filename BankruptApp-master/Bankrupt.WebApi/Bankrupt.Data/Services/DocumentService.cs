using Bankrupt.Core.Entities;
using Bankrupt.Core.Reps;
using Bankrupt.Core.Services;

namespace Bankrupt.Data.Services
{
    public class DocumentService : IDocumentService
    {
        private IDocumentRep documentRep;
        public DocumentService(IDocumentRep documentRep) 
        {
            this.documentRep = documentRep;
        }

        public Task<Guid> AddDocument(Guid userId, Document document)
        {
            return documentRep.AddDocument(userId, document);
        }

        public Task DeleteDocument(Guid id, Guid userId)
        {
            return documentRep.DeleteDocument(id, userId);
        }

        public Task<Document> GetDocument(Guid id, Guid userId)
        {
            return documentRep.GetDocument(id, userId);
        }

        public Task<List<Document>> GetUserDocuments(Guid userId)
        {
            return documentRep.GetUserDocuments(userId);
        }
    }
}
