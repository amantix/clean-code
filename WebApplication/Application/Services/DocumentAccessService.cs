using Application.Abstract.Repositories;
using Application.Abstract.Services;
using Application.Repositories;
using Application.ResponseResult;
using Domain.Enums;
using Domain.Models;

namespace Application.Services;

public class DocumentAccessService(
    IDocumentPermissionRepository permissionRepository,
    IDocumentRepository documentRepository,
    IUserRepository userRepository): IDocumentAccessService
{
    public async Task<bool> TryProvideAccessEditToUser(Guid userId, Guid documentId)
    {
        var documentPermission = await permissionRepository
            .GetDocumentPermission(userId, documentId);
        if (documentPermission != null &&
            documentPermission.AccessLevel == AccessLevel.Edit)
        {
            return true;
        }
        var document = await documentRepository.GetDocumentById(documentId);
        if (document != null && document.UserId == userId)
        {
            return true;
        }

        return false;
    }

    public async Task<bool> TryProvideAccessReadToUser(Guid userId, Guid documentId)
    {
        var documentPermission = await permissionRepository
            .GetDocumentPermission(userId, documentId);
        if (documentPermission != null &&
            (documentPermission.AccessLevel == AccessLevel.Edit ||
             documentPermission.AccessLevel == AccessLevel.Read))
        {
            return true;
        }
        var document = await documentRepository.GetDocumentById(documentId);
        if (document != null && document.UserId == userId)
        {
            return true;
        }

        return false;
    }

    public async Task<Result> CreateDocumentPermission(
        Guid userId, Guid documentId, AccessLevel accessLevel)
    {
        var documentPermissionExist = await permissionRepository.GetDocumentPermission(userId, documentId);
        if (documentPermissionExist != null)
        {
            return Result.Failure(new Error("Document permission already exist", ErrorType.BadRequest));
        }
        var isCreated = await permissionRepository
            .CreateDocumentPermission(documentId, userId, accessLevel);
        if (!isCreated)
        {
            return Result.Failure(new Error(
                "Can't create document permission", 
                ErrorType.ServerError));
        }
        return Result.Success();
    }

    public async Task<Result> UpdateDocumentPermission(Guid userId, Guid documentId, AccessLevel accessLevel)
    {
        var documentPermissionExist = await permissionRepository.GetDocumentPermission(userId, documentId);
        if (documentPermissionExist == null)
        {
            return Result.Failure(new Error("Document permission not found", ErrorType.NotFound));
        }
        documentPermissionExist.AccessLevel = accessLevel;
        var isUpdated = await permissionRepository.UpdateDocumentPermission(documentId, userId, accessLevel);
        if (!isUpdated)
        {
            return Result.Failure(new Error("Can't update document permission", ErrorType.ServerError));
        }
        return Result.Success();
    }

    public async Task<Result> GetDocumentAccessLevelProvides(Guid documentId)
    {
        var result = await permissionRepository.GetPermissionsProvides(documentId);
        if (result == null)
        {
            return Result.Failure(new Error("Can't get document permissions", ErrorType.NotFound));
        }
        return Result<DocumentAccessLevelProvide>.Success(result);
    }

    public async Task<Result> DeleteDocumentPermission(string email, Guid documentId)
    {
        var user = await userRepository.GetUserByEmail(email);
        if (user == null)
        {
            return Result.Failure(new Error("Can't find user", ErrorType.NotFound));
        }
        var documentPermissionExist = await permissionRepository.DeleteDocumentPermission(documentId, user.Id);
        if (!documentPermissionExist)
        {
            return Result.Failure(new Error("Can't delete document permission", ErrorType.NotFound));
        }
        return Result.Success();
        
    }
}