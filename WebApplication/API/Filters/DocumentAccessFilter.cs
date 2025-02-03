using Application.Abstract.Repositories;
using Application.Abstract.Services;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Filters;

public class DocumentAccessFilter(
    IDocumentRepository documentRepository,
    IJwtWorker worker): IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var document = context.HttpContext.Items["Document"] as Document;
        var token = context.HttpContext.Request.Cookies["simply-cookies"];
        if (string.IsNullOrEmpty(token) && document.AccessType == AccessType.Private)
        {
            context.Result = new UnauthorizedObjectResult("token is empty");
            return;
        }
        var validateTokenResult = worker.ValidateToken(token);
        if (validateTokenResult.isSuccess)
        {
            context.HttpContext.Items["UserId"] = validateTokenResult.userId;
            await next();
        }
        else if (document.AccessType == AccessType.Private)
        {
            context.Result = new UnauthorizedObjectResult("token is invalid");
        }
        else
        {
            await next();
        }
        
    }
}