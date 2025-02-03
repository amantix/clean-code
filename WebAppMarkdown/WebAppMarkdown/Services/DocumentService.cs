using WebAppMarkdown.Core.Models;
using WebAppMarkdown.Db.Models;
using WebAppMarkdown.Db.Repositories;

namespace WebAppMarkdown.Services;

public class DocumentService
{
    public readonly DocumentRepository _documentsRepository;
    public DocumentService(
        DocumentRepository documentsRepository) 
    {
        _documentsRepository = documentsRepository;
    }

    public async Task Create(Guid userId, string text)
    {
        DocumentEntity entity = new DocumentEntity
        {
            UserId = userId,
            MarkdownContent = text,
            CreatedAt = DateTime.UtcNow,
        };
        await _documentsRepository.AddMarkdownEntryAsync(entity);
    }

    public async Task Delete(Guid userId, Guid id)
    {
        await _documentsRepository.DeleteMarkdownEntryAsync(userId, id);
    }

    public async Task<List<Document>> GetDocumentsById(Guid userId) 
    {
        var listDocumentEntities = await _documentsRepository.GetEntriesByUserIdAsync(userId);

        return listDocumentEntities
            .Select(MapDocumentEntityToDocument).ToList();
    }
    
    public async Task<List<Document>> GetDocumentsByDocId(Guid Id) 
    {
        var listDocumentEntities = await _documentsRepository.GetDocsByIdAsync(Id);

        return listDocumentEntities
            .Select(MapDocumentEntityToDocument).ToList();
    }
    
    
    private Document MapDocumentEntityToDocument(DocumentEntity entity)
    {
        return new Document
        {
            Id = entity.Id,
            UserId = entity.UserId,
            MarkdownContent = entity.MarkdownContent,
            CreatedAt = entity.CreatedAt,
        };
    }
}