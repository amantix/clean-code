using Data.Entities;
using Markdown.Extensions;
using Markdown.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstracts;
using System.Threading.Tasks;

namespace Markdown.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class DocumentController(IDocumentService documentsService, IMarkdownService service, UserUtils userUtils) : ControllerBase
    {

        [HttpPost("create")]
        public async Task<IActionResult> CreateDocumentAsync([FromBody] DocumentRequest document)
        {
            if(!await userUtils.IsValidUser(HttpContext.User))
            {
                return Unauthorized();
            }

            var userId = HttpContext.GetUserId();
            Console.WriteLine("Id: " + userId);
            var createResult = await documentsService.CreateDocumentAsync((int)userId!, document.Title, document.Content, document.IsPublic, document.Link);

            return createResult.IsSuccess
                ? Ok()
                : BadRequest(new { Error = createResult.ErrorMessage });
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetAllDocumentsAsync()
        {
            if (!await userUtils.IsValidUser(HttpContext.User))
            {
                return Unauthorized();
            }
            var userId = HttpContext.GetUserId();

            var getResult = await documentsService.GetUserDocumentsAsync((int)userId!);

            return getResult.IsSuccess
                ? Ok(new { Documents = getResult.Data })
                : BadRequest(new { Error = getResult.ErrorMessage });
        }

        [HttpGet("{documentId:int}")]
        public async Task<IActionResult> GetDocumentAsync(int documentId)
        {
            if(!await documentsService.IsDocumentExistAsync(documentId))
            {
                return BadRequest(new { Error = "Document doesn't exist" });
            }
            if (!await userUtils.IsValidUser(HttpContext.User))
            {
                return Unauthorized();
            }
            var userId = HttpContext.GetUserId();
            Console.WriteLine("userId: " + userId);
            var getDocumentResult = await documentsService.GetDocumentAsync(documentId);
            Console.WriteLine("document: " + getDocumentResult.Data.UserID);

            if (!await userUtils.IsUserAuthor((int)userId!, getDocumentResult.Data))
            {
                return BadRequest("Is not author of document");
            }

            var content = await service.GetContentMarkdownAsync(documentId);

            return getDocumentResult.IsSuccess
                ? Ok(new { Document = getDocumentResult.Data, Content = content.Data })
                : BadRequest(new { Error = getDocumentResult.ErrorMessage });
        }

        [HttpPut("{documentId:int}/change")]
        public async Task<IActionResult> ChangeDocumentAsync(int documentId, [FromBody] DocumentRequest document)
        {
            if (!await userUtils.IsValidUser(HttpContext.User))
            {
                return Unauthorized();
            }
            var userId = HttpContext.GetUserId();

            var getDocumentResult = await documentsService.GetDocumentAsync(documentId);

            if (!await userUtils.IsUserAuthor((int)userId!, getDocumentResult.Data))
            {
                return BadRequest("Is not author of document");
            }
            var renameResult = await documentsService.ChangeDocumentAsync(documentId, document.Title!, document.Content, document.IsPublic, document.Link);

            return renameResult.IsSuccess
                ? Ok(new { NewName = renameResult.Data })
                : BadRequest(new { Error = renameResult.ErrorMessage });
        }
        [HttpDelete("{documentId:int}/delete")]
        public async Task<IActionResult> DeleteDocumentAsync(int documentId)
        {
            if (!await userUtils.IsValidUser(HttpContext.User))
            {
                return Unauthorized();
            }
            var userId = HttpContext.GetUserId();

            var getDocumentResult = await documentsService.GetDocumentAsync(documentId);

            if (!await userUtils.IsUserAuthor((int)userId!, getDocumentResult.Data))
            {
                return BadRequest("Is not author of document");
            }
            var deleteResult = await documentsService.DeleteDocumentAsync(documentId);

            return deleteResult.IsSuccess
                ? Ok()
                : BadRequest(new { Error = deleteResult.ErrorMessage });
        }
    }
}
