using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Utils;
using Core;
using Core.Models;

namespace Application.Services;

public class DocumentAccessService(IDocumentAccessRepository accessRepository) : IDocumentAccessService
{
    public async Task<Result> AddAccess(Guid userId, Guid documentId, DocumentAccessRoles role)
    {
        var repositoryResult = await accessRepository.CreateOrUpdate(userId, documentId, role);

        return repositoryResult.IsFailure
            ? Result.Failure(repositoryResult.Message!)
            : Result.Success();
    }

    public async Task<Result> DeleteAccess(Guid userId, Guid documentId)
    {
        var repositoryResult = await accessRepository.Delete(userId, documentId);

        return repositoryResult.IsFailure
            ? Result.Failure(repositoryResult.Message!)
            : Result.Success();
    }

    public async Task<Result<DocumentAccessRoles>> GetAccessRole(Guid userId, Guid documentId)
    {

        var repositoryResult = await accessRepository.GetAccess(userId, documentId);

        return repositoryResult.IsFailure
            ? Result<DocumentAccessRoles>.Failure(repositoryResult.Message!)
            : Result<DocumentAccessRoles>.Success(repositoryResult.Data);
    }

    public async Task<Result<List<DocumentAccess>>> GetUsersAccesses(Guid userId)
    {
        var repositoryResult = await accessRepository.GetUsersAccesses(userId);

        return repositoryResult.IsFailure
            ? Result<List<DocumentAccess>>.Failure(repositoryResult.Message!)
            : Result<List<DocumentAccess>>.Success(repositoryResult.Data!);
    }
}