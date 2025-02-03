using API.DTO;
using API.Validations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Filters;

public class UserRegisterValidateFilter(UserRegisterValidator validator): IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var userRegister = context.ActionArguments["userRegister"] as UserRegister;
        if (userRegister == null)
        {
            context.Result = new BadRequestObjectResult("UserRegister is not required");
        }
        var result = validator.Validate(userRegister!);
        if (!result.IsValid)
        {
            context.Result = new BadRequestObjectResult(
                string.Join("\n\r", result.Errors
                    .Select(x => x.ErrorMessage)));
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        
    }
}