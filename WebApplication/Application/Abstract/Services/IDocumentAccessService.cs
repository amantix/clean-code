using Application.ResponseResult;
using Domain.Enums;

namespace Application.Abstract.Services;

public interface IDocumentAccessService
{
    Task<bool> TryProvideAccessEditToUser(Guid userId, Guid documentId);
    Task<bool> TryProvideAccessReadToUser(Guid userId, Guid documentId);
    Task<Result> CreateDocumentPermission(Guid userId, Guid documentId, AccessLevel accessLevel);
    Task<Result> UpdateDocumentPermission(Guid userId, Guid documentId, AccessLevel accessLevel);
    Task<Result> GetDocumentAccessLevelProvides(Guid documentId);
    Task<Result> DeleteDocumentPermission(string email, Guid documentId);
}