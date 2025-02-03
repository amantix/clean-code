using Data.Utils;
using Data.Entities;
using Services.Schemas;


namespace Services.Abstracts
{
    public interface IDocumentService
    {
        Task<ActionResult> CreateDocumentAsync(int userId, string title, string content, bool isPublic, string Link);
        Task<ActionResult<ICollection<Document>?>> GetUserDocumentsAsync(int userId);
        Task<ActionResult<DocumentDTO>> GetDocumentAsync(int documentId);
        Task<ActionResult<string>> ChangeDocumentAsync(int documentId, string newTitle, string newContent, bool isPublic, string Link);
        Task<ActionResult<string>> DeleteDocumentAsync(int documentId);
        Task<bool> IsDocumentExistAsync(int documentId);
    }
}
