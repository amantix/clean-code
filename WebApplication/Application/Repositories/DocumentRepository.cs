using Application.Abstract.Repositories;
using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Entities;

namespace Application.Repositories;

public class DocumentRepository(AppDbContext context): IDocumentRepository
{
    private IDocumentRepository _documentRepositoryImplementation;

    public async Task<Document?> CreateDocument(Document document)
    {
        try
        {
            var documentEntity = new DocumentEntity
            {
                DocumentId = document.Id,
                AuthorId = document.UserId,
                AccessTypeId = (int)document.AccessType,
                Title = document.Title
            };
            await context.Documents.AddAsync(documentEntity);
            await context.SaveChangesAsync();
            return document;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<bool> UpdateDocument(Document document)
    {
        var documentEntity  = await context.Documents
            .FirstOrDefaultAsync(d => d.DocumentId == document.Id);
        if (documentEntity == null)
        {
            return false;
        }
        
        documentEntity.AuthorId = document.UserId;
        documentEntity.Title = document.Title;
        documentEntity.AccessTypeId = (int)document.AccessType;
        
        context.Documents.Update(documentEntity);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteDocumentById(Guid documentId)
    {
        var documentEntity = await context.Documents
            .FirstOrDefaultAsync(u => u.DocumentId == documentId);
        if (documentEntity == null)
        {
            return false;
        }
        context.Documents.Remove(documentEntity);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<Document?> GetDocumentById(Guid documentId)
    {
        var document = await context.Documents
            .FirstOrDefaultAsync(d => d.DocumentId == documentId);
        if (document == null)
        {
            return null;
        }
        return new Document
        {
            Id = document.DocumentId,
            AccessType = (AccessType)document.AccessTypeId,
            Title = document.Title,
            UserId = document.AuthorId
        };
    }

    public async Task<List<Document>> GetAllDocuments(Guid userId)
    {
        var user = await context.Users
            .Where(u => u.UserId == userId)
            .Include(u => u.Documents)
            .FirstAsync();
            
        return user.Documents
            .Select(d => new Document
            {
                Title = d.Title,
                Id = d.DocumentId,
                UserId = d.AuthorId,
                AccessType = (AccessType)d.AccessTypeId
            })
            .ToList();
    }
}