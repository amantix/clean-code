using Application.Utils;
using Core;
using Core.Models;

namespace Application.Interfaces.Repositories;

public interface IDocumentAccessRepository
{
    Task<Result<DocumentAccessRoles>> GetAccess(Guid userId, Guid documentId);
    Task<Result> CreateOrUpdate(Guid userId, Guid documentId, DocumentAccessRoles role);
    Task<Result> Delete(Guid userId, Guid documentId);
    
    Task<Result<List<DocumentAccess>>> GetUsersAccesses(Guid userId);
}