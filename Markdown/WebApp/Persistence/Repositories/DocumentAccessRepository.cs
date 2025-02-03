using Application;
using Application.Interfaces.Repositories;
using Application.Utils;
using Core;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Persistence.Repositories;

public class DocumentAccessRepository(AppDbContext dbContext) : IDocumentAccessRepository
{
    public async Task<Result<DocumentAccessRoles>> GetAccess(Guid userId, Guid documentId)
    {
        var userEntity = await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Id == userId);
        
        if (userEntity!.Role == "admin")
        {
            return Result<DocumentAccessRoles>.Success(DocumentAccessRoles.Author);
        }
        
        var accessEntity = await dbContext.DocumentAccesses
            .FirstOrDefaultAsync(access => access.UserId == userId && access.DocumentId == documentId);

        Console.WriteLine($"userId: {userId}, DocumentId: {documentId}");
        return accessEntity != null
            ? Result<DocumentAccessRoles>.Success(accessEntity.Role)
            : Result<DocumentAccessRoles>.Success(DocumentAccessRoles.None);
    }

    public async Task<Result> CreateOrUpdate(Guid userId, Guid documentId, DocumentAccessRoles role)
    {
        var existingAccess = await dbContext.DocumentAccesses
            .FirstOrDefaultAsync(access => access.UserId == userId && access.DocumentId == documentId);

        if (existingAccess != null)
        {
            existingAccess.Role = role;
        }
        else
        {
            var documentAccess = new DocumentAccessEntity
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                DocumentId = documentId,
                Role = role
            };
            
            await dbContext.DocumentAccesses.AddAsync(documentAccess);
        }
        
        try
        {
            await dbContext.SaveChangesAsync();

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Failed to create or update document access: {ex.Message}");
        }
    }

    public async Task<Result> Delete(Guid userId, Guid documentId)
    {
        try
        {
            await dbContext.DocumentAccesses
                .Where(access => access.UserId == userId && access.DocumentId == documentId)
                .ExecuteDeleteAsync();

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Failed to delete document access: {ex.Message}");
        }
    }

    public async Task<Result<List<DocumentAccess>>> GetUsersAccesses(Guid userId)
    {
        try
        {
            var accessibleDocuments = await dbContext.DocumentAccesses
                .Where(access => access.UserId == userId)
                .Select(access => DocumentAccess.Create(access.Id, access.UserId, access.DocumentId, access.Role))
                .ToListAsync();

            return Result<List<DocumentAccess>>.Success(accessibleDocuments);
        }
        catch (Exception ex)
        {
            return Result<List<DocumentAccess>>.Failure($"Failed to get user's accesses: {ex.Message}");
        }
    }
}