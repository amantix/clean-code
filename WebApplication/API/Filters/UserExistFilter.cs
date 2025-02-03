using Application.Abstract.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Guid = System.Guid;

namespace API.Filters;

public class UserExistFilter(IUserService userService): IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var useridClaim = context.HttpContext.User
            .FindFirst(claim => claim.Type == "Sub");

        Guid userId = Guid.Empty;
        bool isValid = (useridClaim?.Value != null && 
                        Guid.TryParse(useridClaim.Value, out userId) && 
                        userId != Guid.Empty);
        
        if (!isValid)
        {
            context.Result = new BadRequestObjectResult("Invalid user id");
            return;
        }

        var result = await userService.GetUserById(userId);
        if (result.IsOk == false)
        {
            context.Result = new NotFoundObjectResult("User not found");
            return;
        }
        
        context.HttpContext.Items["UserId"] = userId;
    }
}