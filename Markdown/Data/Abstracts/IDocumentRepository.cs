using Data.Entities;
using Data.Utils;


namespace Data.Abstracts
{
    public interface IDocumentRepository
    {
        Task<ActionResult<int>> CreateDocumentAsync(int userId, string title, bool isPublic, string Link);
        Task<ActionResult<string>> ChangeDocumentAsync(int documentId, string newTitle, bool isPublic, string Link);
        Task<ActionResult<Document>> GetDocumentByIdAsync(int documentId);
        Task<ActionResult<ICollection<Document>?>> GetDocumentsAsync(int userId);
        Task<ActionResult<string>> DeleteDocumentAsync(int documentId);
        Task<bool> IsDocumentExistsByIdAsync(int documentId);
        Task<ActionResult<string>> GetAuthorNameAsync(int documentId);
    }
}
