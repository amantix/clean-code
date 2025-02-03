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
[Route("api/document/provide")]
public class DocumentAccessLevelProvideController(
    IUserService userService,
    IDocumentAccessService documentAccessService,
    ResponseResultCreator creator): ControllerBase
{
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

    [HttpGet("get/{documentId:guid}")]
    public async Task<IActionResult> GetDocumentAccessLevelProvides(Guid documentId)
    {
        var result = await documentAccessService.GetDocumentAccessLevelProvides(documentId);
        if (result.IsOk)
        {
            var res = (Result<DocumentAccessLevelProvide>)result;
            return Ok(res.Value);
        }
        return creator.CreateAction(result);
    }

    [HttpDelete("delete/{documentId:guid}")]
    public async Task<IActionResult> DeleteDocumentAccessLevelProvides(Guid documentId, 
        [FromBody] UserDocumentProvide documentProvide)
    {
        var res = await documentAccessService
            .DeleteDocumentPermission(documentProvide.Email, documentId);
        if (!res.IsOk)
        {
            return creator.CreateAction(res);
        }
        return Ok();
    }
}