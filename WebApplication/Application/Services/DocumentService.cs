using System.Text;
using Application.Abstract.Repositories;
using Application.Abstract.Services;
using Application.ResponseResult;
using Domain.Enums;
using Domain.Models;

namespace Application.Services;

public class DocumentService(
    IMinioService minioService, 
    IDocumentRepository documentRepository): IDocumentService
{
    public async Task<Result> PutDocument(string name, AccessType accessType, Guid userId)
    {
        var document = new Document
        {
            Id = Guid.NewGuid(),
            Title = name,
            AccessType = accessType,
            UserId = userId
        };
        var createdDocument = await documentRepository.CreateDocument(document);
        if (createdDocument == null)
        {
            return Result.Failure(new Error("Document could not be created",ErrorType.ServerError));
        }
        
        string fileName = $"{createdDocument.Id}.txt";
        using var stream = new MemoryStream();
        await stream.WriteAsync(Encoding.UTF8.GetBytes("document"));
        var isFileUpload = await minioService.UploadFileAsync(fileName, stream);

        if (!isFileUpload)
        {
            return Result.Failure(new Error("File could not be uploaded", ErrorType.ServerError));
        }
        
        return Result<Document>.Success(createdDocument);
    }

    public async Task<Result> UpdateDocument(Guid documentId, string name, AccessType accessType, Guid userId)
    {
        var document = new Document()
        {
            Id = documentId,
            UserId = userId,
            AccessType = accessType,
            Title = name
        };
        var isDocumentUpdated = await documentRepository.UpdateDocument(document);
        if (!isDocumentUpdated)
        {
            return Result.Failure(new Error("Document not found", ErrorType.NotFound));
        }
        return Result.Success();
    }

    public async Task<Result> UpdateDocumentContent(Guid documentId, string content)
    {
        string fileName = $"{documentId}.txt";
        var isUpdatedContent = await minioService.UpdateFileContentAsync(fileName, content);
        if (!isUpdatedContent)
        {
            return Result.Failure(new Error("File not found", ErrorType.NotFound));
        }
        return Result.Success();
    }

    public async Task<Result> DeleteDocumentById(Guid documentId)
    {
        var isDocumentDeleted = await documentRepository.DeleteDocumentById(documentId);
        if (!isDocumentDeleted)
        {
            return Result.Failure(new Error("Document not found", ErrorType.NotFound));
        }
        
        string fileName = $"{documentId}.txt";
        var isFileDeleted = await minioService.DeleteFileAsync(fileName);
        if (!isFileDeleted)
        {
            return Result.Failure(new Error("File not found", ErrorType.NotFound));
        }
        
        return Result.Success();
    }

    public async Task<Result> GetDocumentContentById(Guid documentId)
    {
        string fileName = $"{documentId}.txt";
        var content = await minioService.DownloadFileContentAsync(fileName);
        if (content == null)
        {
            return Result.Failure(new Error("File not found", ErrorType.NotFound));
        }
        return Result<string>.Success(content);
    }

    public async Task<Result> GetAllDocuments(Guid userId)
    {
        var documents = await documentRepository.GetAllDocuments(userId);
        return Result<List<Document>>.Success(documents);
    }
}