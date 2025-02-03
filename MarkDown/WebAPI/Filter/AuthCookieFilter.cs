using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using WebAPI.Services;

namespace WebAPI.Filter
{
    public class AuthCookieFilter : IAsyncActionFilter
    {
        private readonly UsersService _usersService;

        public AuthCookieFilter(UsersService usersService)
        {
            _usersService = usersService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var httpContext = context.HttpContext;

            if (!httpContext.Request.Cookies.ContainsKey("j-cookie"))
            {
                context.Result = new RedirectToPageResult("/login");
                return; 
            }
            await next(); 
        }
    }
}
