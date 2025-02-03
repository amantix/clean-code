using Data.Abstracts;
using Data.Utils;
using Data.Entities;
using Data.Repositories;
using Services.Abstracts;
using Services.Schemas;


namespace Services.Services
{
    public class DocumentService(IDocumentRepository repository, MinioService minIoService) : IDocumentService
    {
        public async Task<ActionResult<DocumentDTO>> GetDocumentAsync(int documentId)
        {

            var document = await repository.GetDocumentByIdAsync(documentId);
            if (!document.IsSuccess)
                return ActionResult<DocumentDTO>.Fail(document.ErrorMessage!)!;

            var documentDto = new DocumentDTO
            {
                Id = documentId,
                UserID = document.Data.UserId,
                Title = document.Data.Title,
                IsPublic = document.Data.isPublic,
                Link = document.Data.Link!,
            };

            return ActionResult<DocumentDTO>.Ok(documentDto);
        }

        public async Task<ActionResult<ICollection<Document>?>> GetUserDocumentsAsync(int userId)
        {
            return await repository.GetDocumentsAsync(userId);
        }


        public async Task<ActionResult> CreateDocumentAsync(int userId, string title, string content, bool isPublic, string Link)
        {
            var ctx = new CancellationTokenSource();
            var createResult = await repository.CreateDocumentAsync(userId, title, isPublic, Link);

            if (!createResult.IsSuccess)
                return ActionResult.Fail(createResult.ErrorMessage!);

            try
            {
                await minIoService.UploadMarkdownTextAsync(content, $"{createResult.Data}.md", ctx.Token);
            }
            catch (Exception ex)
            {
                await repository.DeleteDocumentAsync(createResult.Data);
                return ActionResult.Fail(ex.Message);
            }
            return ActionResult.Ok();
        }

        public async Task<ActionResult<string>> ChangeDocumentAsync(int documentId, string newTitle, string newContent, bool isPublic, string Link)
        {
            var ctx = new CancellationTokenSource();

            await repository.ChangeDocumentAsync(documentId, newTitle, isPublic, Link);
            try
            {
                await minIoService.UploadMarkdownTextAsync(newContent, $"{documentId}.md", ctx.Token);
            }
            catch (Exception ex)
            {
                return ActionResult<string>.Fail(ex.Message);
            }
            return ActionResult<string>.Ok(newTitle);

        }


        public async Task<ActionResult<string>> DeleteDocumentAsync(int documentId)
        {
            return await repository.DeleteDocumentAsync(documentId);
        }

        
        

        public async Task<bool> IsDocumentExistAsync(int documentId)
        {
            return await repository.IsDocumentExistsByIdAsync(documentId);
        }
    }
}
