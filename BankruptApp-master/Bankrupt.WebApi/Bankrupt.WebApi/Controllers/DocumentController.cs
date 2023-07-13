using Bankrupt.WebApi.Mappers;
using Microsoft.AspNetCore.Mvc;
using Bankrupt.Core.Entities;
using Bankrupt.Core.Services;

namespace Bankrupt.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentController : ControllerBase
    {
        private readonly ILogger<UserController> logger;
        private readonly IDocumentService documentService;

        public DocumentController(ILogger<UserController> logger, IDocumentService documentService)
        {
            this.logger = logger;
            this.documentService = documentService;
        }

        [HttpGet]
        //[Authorize]
        [Route("User/{userId}")]
        public async Task<IActionResult> GetDocuments(Guid userId)
        {
            var documents = await documentService.GetUserDocuments(userId);
            var dtoDocuments = documents.Select(d => d.ToDto()).ToList();
            return Ok(dtoDocuments);
        }

        [HttpGet]
        //[Authorize]
        [Route("id={id}&userId={userId}")]
        public async Task<IActionResult> GetDocument(Guid id, Guid userId)
        {
            var document = await documentService.GetDocument(id, userId);
            return File(document.Data, "application/octet-stream", document.FileName);
        }

        [HttpPost, DisableRequestSizeLimit]
        //[Authorize(Roles = "User")]
        [Route("Upload")]
        public async Task<IActionResult> Upload()
        {
            var formCollection = await Request.ReadFormAsync();
            formCollection.TryGetValue("userId", out var userId);
            var file = formCollection.Files.First();
            var document = new Document();
            document.FileName = file.FileName.Trim();
            document.Date = DateOnly.FromDateTime(DateTime.Now);
            document.UserId = Guid.Parse(userId.ToString());
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                document.Data = ms.ToArray();
            }
            document.Id = Guid.NewGuid();
            var id = await documentService.AddDocument(document.UserId, document);
            return Ok(id);
        }

        [HttpDelete]
        //[Authorize(Roles = "User")]
        [Route("id={id}&userId={userId}")]
        public async Task<IActionResult> DeleteDocument(Guid id, Guid userId)
        {
            await documentService.DeleteDocument(id, userId);
            return Ok();
        }
    }
}