using Bankrupt.Core;
using Bankrupt.Core.Entities;
using Bankrupt.Core.Exceptions;
using Bankrupt.Core.Reps;
using Bankrupt.Data.Mappers;
using Bankrupt.Data.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Bankrupt.Data.Reps
{
    public class DocumentRep : IDocumentRep
    {
        private readonly IMongoCollection<DocumentMongoDb> documentsCollection;
        private readonly IGridFSBucket gridFS;

        public DocumentRep(IOptions<BankruptDbSettings> bookStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(bookStoreDatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(bookStoreDatabaseSettings.Value.DatabaseName);
            documentsCollection = mongoDatabase.GetCollection<DocumentMongoDb>(bookStoreDatabaseSettings.Value.DocumentsCollectionName);
            gridFS = new GridFSBucket(mongoDatabase);
        }

        public async Task<Guid> AddDocument(Guid userId, Document document)
        {
            if (document == null)
                throw new ValidationException("Нельзя добавить пустой документ", StatusCodes.Status409Conflict);
            document.ObjectId = await gridFS.UploadFromBytesAsync(document.FileName, document.Data);
            await documentsCollection.InsertOneAsync(document.ToMongoDbEntity());
            return document.Id;
        }

        public async Task DeleteDocument(Guid id, Guid userId)
        {
            var document = await GetDocument(id, userId);
            await gridFS.DeleteAsync(document.ObjectId);
            var result = await documentsCollection.DeleteOneAsync(d => d.Id.Equals(id));
            if (result.DeletedCount == 0)
                throw new ValidationException("Такого документа не существует", StatusCodes.Status409Conflict);
        }

        public async Task<Document> GetDocument(Guid id, Guid userId)
        {
            var documentMongo = await documentsCollection.Find(d => d.Id.Equals(id) && d.UserId.Equals(userId)).FirstOrDefaultAsync();
            if (documentMongo == null)
                throw new ValidationException("Такого документа не существует", StatusCodes.Status409Conflict);
            var document = documentMongo.ToEntity();
            document.Data = await gridFS.DownloadAsBytesAsync(document.ObjectId);
            return document;
        }

        public async Task<List<Document>> GetUserDocuments(Guid userId)
        {
            var documentsMongo = await documentsCollection.Find(d => d.UserId.Equals(userId)).ToListAsync();
            var documents = documentsMongo.Select(d => d.ToEntity()).ToList();
            foreach (var doc in documents)
            {
                doc.Data = await gridFS.DownloadAsBytesAsync(doc.ObjectId);
            }
            return documents;
        }
    }
}