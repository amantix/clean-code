using Core.Models;
using Application;
using Application.Interfaces.Repositories;
using Application.Utils;
using Core;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Persistence.Repositories;

public class DocumentsRepository(AppDbContext dbContext) : IDocumentRepository
{
    public async Task<Result<List<Document>>> GetAll()
    {
        var documentEntities = await dbContext.Documents
            .AsNoTracking()
            .ToListAsync();

        return Result<List<Document>>.Success(
            documentEntities
                .Select(
                    dEntity => Document.Create(dEntity.Id, dEntity.Name, dEntity.AuthorId, dEntity.AuthorName,
                        dEntity.CreationDateTime))
                .ToList());
    }

    public async Task<Result<Document>> GetById(Guid id)
    {
        var documentEntity = await dbContext.Documents
            .AsNoTracking()
            .FirstOrDefaultAsync(doc => doc.Id == id);

        if (documentEntity == null)
            return Result<Document>.Failure("Document not found.");

        var document = Document.Create(documentEntity.Id, documentEntity.Name, documentEntity.AuthorId,
            documentEntity.AuthorName, documentEntity.CreationDateTime);
        return Result<Document>.Success(document);
    }

    public async Task<Result<List<Document>>> GetByAuthor(string author)
    {
        var documentEntities = await dbContext.Documents
            .AsNoTracking()
            .Where(doc => doc.Author.Equals(author))
            .ToListAsync();

        return Result<List<Document>>.Success(
            documentEntities
                .Select(
                    dEntity => Document.Create(dEntity.Id, dEntity.Name, dEntity.AuthorId, dEntity.AuthorName,
                        dEntity.CreationDateTime))
                .ToList());
    }

    public async Task<Result<List<Document>>> GetByAuthorId(Guid authorId)
    {
        var documentEntities = await dbContext.Documents
            .AsNoTracking()
            .Where(doc => doc.AuthorId == authorId)
            .ToListAsync();

        return Result<List<Document>>.Success(
            documentEntities
                .Select(
                    dEntity => Document.Create(dEntity.Id, dEntity.Name, dEntity.AuthorId, dEntity.AuthorName,
                        dEntity.CreationDateTime))
                .ToList());
    }

    public async Task<Result<List<Document>>> GetByUserIdWithAccess(Guid userId)
    {
        var documentEntities = await dbContext.Documents
            .AsNoTracking()
            .Join(
                dbContext.DocumentAccesses.Where(access =>
                    access.UserId == userId && access.Role != DocumentAccessRoles.None),
                doc => doc.Id,
                access => access.DocumentId,
                (doc, access) => doc
            )
            .ToListAsync();

        return Result<List<Document>>.Success(
            documentEntities
                .Select(
                    dEntity => Document.Create(dEntity.Id, dEntity.Name, dEntity.AuthorId, dEntity.AuthorName,
                        dEntity.CreationDateTime))
                .ToList());
    }

    public async Task<Result<Guid>> Create(string documentName, Guid authorId)
    {
        var newDocGuid = Guid.NewGuid();
        var newAccessGuid = Guid.NewGuid();

        try
        {
            var authorName = await dbContext.Users
                .AsNoTracking()
                .Where(user => user.Id == authorId)
                .Select(user => user.Login)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(authorName))
            {
                return Result<Guid>.Failure("Author not found.");
            }

            var documentEntity = new DocumentEntity
            {
                Id = newDocGuid,
                Name = documentName,
                AuthorId = authorId,
                AuthorName = authorName,
                CreationDateTime = DateTime.UtcNow,
            };

            var accessEntity = new DocumentAccessEntity
            {
                Id = newAccessGuid,
                UserId = authorId,
                DocumentId = documentEntity.Id,
                Role = DocumentAccessRoles.Author
            };


            await dbContext.Documents.AddAsync(documentEntity);
            await dbContext.DocumentAccesses.AddAsync(accessEntity);
            await dbContext.SaveChangesAsync();

            return Result<Guid>.Success(documentEntity.Id);
        }
        catch (Exception ex)
        {
            return Result<Guid>.Failure($"Failed to create document: {ex.Message}");
        }
    }

    public async Task<Result> Rename(Guid documentId, string documentName)
    {
        try
        {
            await dbContext.Documents
                .Where(doc => doc.Id == documentId)
                .ExecuteUpdateAsync(doc => doc
                    .SetProperty(document => document.Name, documentName));

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Failed to rename document: {ex.Message}");
        }
    }

    public async Task<Result> Delete(Guid documentId)
    {
        try
        {
            await dbContext.Documents
                .Where(doc => doc.Id == documentId)
                .ExecuteDeleteAsync();

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Failed to delete document: {ex.Message}");
        }
    }
}