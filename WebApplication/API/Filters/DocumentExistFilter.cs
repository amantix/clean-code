using Application.Abstract.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Filters;

public class DocumentExistFilter(IDocumentRepository documentRepository): IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        context.ActionArguments.TryGetValue("documentId", out var documentIdObject);
        Guid documentId = Guid.Empty;
        if (documentIdObject != null &&
            !Guid.TryParse(documentIdObject.ToString(), out documentId))
        {
            context.Result = new BadRequestObjectResult("Invalid document Id");
            return;
        }
        var document = await documentRepository.GetDocumentById(documentId);
        if (document == null)
        {
            context.Result = new NotFoundObjectResult("Document not found");
            return;
        }
        context.HttpContext.Items.Add("Document", document);
        await next();
    }
}