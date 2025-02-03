using Application.Utils;
using Core;
using Core.Models;

namespace Application.Interfaces.Services;

public interface IDocumentAccessService
{
    Task<Result> AddAccess(Guid userId, Guid documentId, DocumentAccessRoles role);
    Task<Result> DeleteAccess(Guid userId, Guid documentId);
    Task<Result<DocumentAccessRoles>> GetAccessRole(Guid userId, Guid documentId);
    
    Task<Result<List<DocumentAccess>>> GetUsersAccesses(Guid userId);
}