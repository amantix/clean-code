using API.DTO;
using API.Validations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Filters;

public class UserLoginEmailValidateFilter(UserLoginEmailValidator validator): IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var userLogin = context.ActionArguments["userLoginEmail"] as UserLoginEmail;
        if (userLogin == null)
        {
            context.Result = new BadRequestObjectResult("Please provide valid user login email");
        }
        var result = validator.Validate(userLogin!);
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