using WebAppMarkdown.Db.Models;

namespace WebAppMarkdown.Db.Interfaces;

public interface IDocumentRepository
{
    Task AddMarkdownEntryAsync(DocumentEntity documentEntity);
    Task<List<DocumentEntity>> GetEntriesByUserIdAsync(Guid userId);
}