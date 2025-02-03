using Domain.Models;

namespace Application.Abstract.Repositories;

public interface IDocumentRepository
{
    Task<Document?> CreateDocument(Document document);
    Task<bool> UpdateDocument(Document document);
    Task<bool> DeleteDocumentById(Guid documentId);
    Task<Document?> GetDocumentById(Guid documentId);
    Task<List<Document>> GetAllDocuments(Guid userId);
}