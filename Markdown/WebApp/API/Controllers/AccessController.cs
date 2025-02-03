using API.Attributes;
using API.Filters;
using API.Requests;
using Application.Interfaces.Services;
using Core;
using Core.Models;
using Infrastructure;
using Infrastructure.Auth;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class AccessController(IDocumentAccessService accessService, IUserService userService) : ControllerBase
{
    [RoleAuthorize("user")]
    [HasAccess(DocumentAccessRoles.Author)]
    [HttpPost("Change")]
    public async Task<IActionResult> Change([FromBody] ManageAccessRequest request)
    {
        var userResult = await userService.GetByLogin(request.UserLogin);
        if (userResult.IsFailure)
        {
            Problem(userResult.Message);
        }

        var user = userResult.Data;

        var addAccessResult =
            await accessService.AddAccess(user!.Id, request.DocumentId, (DocumentAccessRoles)request.AccessLevel);
        if (addAccessResult.IsFailure)
        {
            return BadRequest(addAccessResult.Message);
        }

        return Ok();
    }

    [RoleAuthorize("user")]
    [ServiceFilter(typeof(UserExistFilter))]
    [HasAccess(DocumentAccessRoles.Author)]
    [HttpDelete("Remove")]
    public async Task<IActionResult> Remove([FromBody] ManageAccessRequest request)
    {
        var userResult = await userService.GetByLogin(request.UserLogin);
        if (userResult.IsFailure)
        {
            BadRequest(userResult.Message);
        }

        var user = userResult.Data;

        var deleteAccessResult = await accessService.DeleteAccess(user!.Id, request.DocumentId);
        if (deleteAccessResult.IsFailure)
        {
            return BadRequest(deleteAccessResult.Message);
        }

        return Ok();
    }
}