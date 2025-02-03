using API.DTO;
using API.Filters;
using API.Utils;
using Application.Abstract.Services;
using Application.ResponseResult;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
[ServiceFilter(typeof(UserExistFilter))]
[ApiController]
[Route("api/document")]
public class DocumentController(
    IMinioService minioService,
    IDocumentService documentService,
    IUserService userService,
    IDocumentAccessService documentAccessService,
    ResponseResultCreator creator): ControllerBase
{
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateDocument([FromBody] UserDocument documentResponse)
    {
        var userId = Guid.Parse(HttpContext.Items["UserId"].ToString());
        var result = await documentService.PutDocument(
            documentResponse.Title,
            documentResponse.Type,
            userId);
        if (!result.IsOk)
        {
            return creator.CreateAction(result);
        }
        
        var document = ((Result<Document>)result).Value;
        return Ok(document);
    }
    
    [HttpPatch("update/{documentId:guid}")]
    public async Task<IActionResult> UpdateDocument(Guid documentId, [FromBody] UserDocument documentResponse)
    {
        var userId = Guid.Parse(HttpContext.Items["UserId"].ToString());
        var result = await documentService.UpdateDocument(documentId,
            documentResponse.Title, 
            documentResponse.Type, 
            userId);

        if (!result.IsOk)
        {
            return creator.CreateAction(result);
        }
        
        return Ok();
    }
    
    [HttpDelete("delete/{documentId:guid}")]
    public async Task<IActionResult> DeleteDocument(Guid documentId)
    {
        var result = await documentService.DeleteDocumentById(documentId);
        if (!result.IsOk)
        {
            return creator.CreateAction(result);
        }

        return Ok();
    }
    
    [HttpGet("all")]
    public async Task<IActionResult> GetAllDocuments()
    {
        var userId = Guid.Parse(HttpContext.Items["UserId"].ToString());
        var result = await documentService.GetAllDocuments(userId);
        if (!result.IsOk)
        {
            return creator.CreateAction(result);
        }
        var documents = ((Result<List<Document>>)result).Value;
        if (documents == null)
        {
            return Problem();
        }
        return Ok(documents);
    }
    
    [HttpPost("provide/{documentId:guid}")]
    public async Task<IActionResult> ProvideDocument(Guid documentId, 
        [FromBody] UserDocumentProvide documentProvide)
    {
        var result = await userService.GetUserByEmail(documentProvide.Email);
        if (!result.IsOk)
        {
            return creator.CreateAction(result);
        }

        var user = ((Result<User>)result).Value;
        var userId = user.Id;
        var accessLevel = documentProvide.AccessLevel;
        var resultResponse = await documentAccessService
            .CreateDocumentPermission(userId, documentId, accessLevel);
        if (!resultResponse.IsOk)
        {
            return creator.CreateAction(resultResponse);
        }
        return Ok();
    }
    
    [HttpGet("url/{documentId:guid}")]
    public async Task<IActionResult> GetDocumentUrl(Guid documentId)
    {
        var result = await minioService.GetDownloadFileAsync($"{documentId}.txt");
        return File(result, "text/plain", $"{documentId}.txt");
    }
}