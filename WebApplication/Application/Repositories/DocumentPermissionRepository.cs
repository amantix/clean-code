using Application.Abstract.Repositories;
using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Entities;

namespace Application.Repositories;

public class DocumentPermissionRepository(AppDbContext context): IDocumentPermissionRepository
{
    public async Task<bool> CreateDocumentPermission(Guid documentId, Guid userId, AccessLevel accessLevel)
    {
        try
        {
            var doc = await GetDocumentPermission(userId, documentId);
            if (doc != null)
            {
                return false;
            }
            var permissionEntity = new DocumentPermissionEntity
            {
                AccessLevelId = (int)accessLevel,
                UserId = userId,
                DocumentId = documentId,
                DocumentPermissionId = Guid.NewGuid()
            };
            await context.Permissions.AddAsync(permissionEntity);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> UpdateDocumentPermission(Guid documentId, Guid userId, AccessLevel accessLevel)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteDocumentPermission(Guid documentId, Guid userId)
    {
        var permission = await context.Permissions.FirstOrDefaultAsync(p => p.DocumentId == documentId && p.UserId == userId);
        if (permission != null)
        {
            context.Permissions.Remove(permission);
            await context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<DocumentPermission?> GetDocumentPermission(Guid userId, Guid documentId)
    {
        var permissionEntity = await context.Permissions
            .FirstOrDefaultAsync(p => p.DocumentId == documentId && p.UserId == userId);
        if (permissionEntity == null)
        {
            return null;
        }

        return new DocumentPermission
        {
            DocumentId = permissionEntity.DocumentId,
            UserId = permissionEntity.UserId,
            AccessLevel = (AccessLevel)permissionEntity.AccessLevelId,
        };
    }

    public async Task<DocumentAccessLevelProvide?> GetPermissionsProvides(Guid documentId)
    {
        var doc = await context.Documents.Where(d => d.DocumentId == documentId)
            .Include(d => d.Permissions)
            .ThenInclude(documentPermissionEntity => documentPermissionEntity.User)
            .FirstOrDefaultAsync();
        if (doc == null)
        {
            return null;
        }

        return new DocumentAccessLevelProvide
        {
            DocumentId = doc.DocumentId,
            Provides = doc.Permissions.Select(p => new AccessLevelProvide
            {
                Email = p.User.UserEmail,
                Level = (AccessLevel)p.AccessLevelId
            }).ToList()
        };

    }
}