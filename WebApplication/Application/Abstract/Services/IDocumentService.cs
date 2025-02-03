using Application.ResponseResult;
using Domain.Enums;

namespace Application.Abstract.Services;

public interface IDocumentService
{
    Task<Result> PutDocument(string name, AccessType accessType, Guid userId);
    Task<Result> UpdateDocument(Guid documentId, string name, AccessType accessType, Guid userId);
    Task<Result> UpdateDocumentContent(Guid documentId, string content);
    Task<Result> DeleteDocumentById(Guid documentId);
    Task<Result> GetDocumentContentById(Guid documentId);
    Task<Result> GetAllDocuments(Guid userId);
}