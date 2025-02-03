using Application.Abstract.Services;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Filters;

public class DocumentReadFilter(IDocumentAccessService accessService): IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var document = context.HttpContext.Items["Document"] as Document;
        if (context.HttpContext.Items["UserId"] != null)
        {
            var userId = Guid.Parse(context.HttpContext.Items["UserId"].ToString());
        
            var isAccessProvided = await accessService
                .TryProvideAccessReadToUser(userId, document.Id);

            if (isAccessProvided)
            {
                await next();
                return;
            }
            
        }
        if (document.AccessType == AccessType.PublicRead || 
            document.AccessType == AccessType.PublicEdit)
        {
            await next();
        }
        else
        {
            context.Result = new ForbidResult("You are not authorized to access document");;
        }
    }
}