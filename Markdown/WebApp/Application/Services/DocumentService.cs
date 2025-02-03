using System.Text;
using Application.Interfaces.MinioS3;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Utils;
using Core.Models;

namespace Application.Services;

public class DocumentService(IDocumentRepository documentRepository, IMinioStorage minioStorage) : IDocumentService
{
    public async Task<Result<List<Document>>> GetAll()
    {
        var repositoryResult = await documentRepository.GetAll();

        return repositoryResult.IsFailure
            ? Result<List<Document>>.Failure(repositoryResult.Message!)
            : Result<List<Document>>.Success(repositoryResult.Data!);
    }

    public async Task<Result<Document>> GetById(Guid id)
    {
        var repositoryResult = await documentRepository.GetById(id);

        return repositoryResult.IsFailure
            ? Result<Document>.Failure(repositoryResult.Message!)
            : Result<Document>.Success(repositoryResult.Data!);
    }

    public async Task<Result<byte[]>> GetContentById(Guid id)
    {
        var minioResult = await minioStorage.DownloadFile(id.ToString());
        
        return minioResult.IsFailure
            ? Result<byte[]>.Failure(minioResult.Message!)
            : Result<byte[]>.Success(minioResult.Data!);
    }
    
    public async Task<Result<List<Document>>> GetByAuthorId(Guid authorId)
    {
        var repositoryResult = await documentRepository.GetByAuthorId(authorId);

        return repositoryResult.IsFailure
            ? Result<List<Document>>.Failure(repositoryResult.Message!)
            : Result<List<Document>>.Success(repositoryResult.Data!);
    }

    public async Task<Result<List<Document>>> GetByUserIdWithAccess(Guid authorId)
    {
        var repositoryResult = await documentRepository.GetByUserIdWithAccess(authorId);

        return repositoryResult.IsFailure
            ? Result<List<Document>>.Failure(repositoryResult.Message!)
            : Result<List<Document>>.Success(repositoryResult.Data!);
    }

    public async Task<Result<Guid>> Create(string documentName, Guid authorId)
    {
        var authorsDocsResult = await documentRepository.GetByAuthorId(authorId);
        if (authorsDocsResult.IsFailure)
        {
            return Result<Guid>.Failure(authorsDocsResult.Message!);
        }

        var authorsDocs = authorsDocsResult.Data;
        if (authorsDocs != null && authorsDocs
                .Any(doc => doc.Name == documentName))
        {
            return Result<Guid>.Failure("Document with such name already exists");
        }

        var createResult = await documentRepository.Create(documentName, authorId);
        if (createResult.IsFailure)
        {
            return Result<Guid>.Failure(createResult.Message!);
        }
        var documentId = createResult.Data;
        
        var minioResult = await minioStorage.UploadFile(documentId.ToString(), Encoding.UTF8.GetBytes(documentName));
        
        if (minioResult.IsFailure)
        {
            var docDeletedResult = await documentRepository.Delete(documentId);
            return docDeletedResult.IsFailure 
                ? Result<Guid>.Failure("ДОКУМЕНТ БЫЛ СОЗДАН, НО НЕ УДАЛЁН! " + docDeletedResult.Message!) 
                : Result<Guid>.Failure(minioResult.Message);
        }
        
        return Result<Guid>.Success(documentId);
    }

    public async Task<Result> Update(Guid documentId, string documentText)
    {
        
        var minioResult = await minioStorage.UploadFile(documentId.ToString(), Encoding.UTF8.GetBytes(documentText));
        return minioResult.IsFailure 
            ? Result.Failure(minioResult.Message) 
            : Result.Success();
    }

    public async Task<Result> Rename(Guid documentId, string newName)
    {
        var repositoryResult = await documentRepository.Rename(documentId, newName);

        return repositoryResult.IsFailure 
            ? Result.Failure(repositoryResult.Message) 
            : Result.Success();
    }

    public async Task<Result> Delete(Guid documentId)
    {
        var deleteRepositoryResult = await documentRepository.Delete(documentId);
        if (deleteRepositoryResult.IsFailure)
        {
            return Result.Failure(deleteRepositoryResult.Message);
        }
        
        var minioResult = await minioStorage.DeleteFile(documentId.ToString());

        return minioResult.IsFailure
            ? Result.Failure(minioResult.Message!)
            : Result.Success();
    }
}