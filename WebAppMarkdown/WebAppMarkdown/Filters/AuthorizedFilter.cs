// using System.Security.Claims;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.Filters;
// using WebAppMarkdown.Services;
//
// namespace WebAppMarkdown.Filters;
//
// public class AuthorizedFilter : IAsyncAuthorizationFilter
// {
//     private readonly UsersService _usersService;
//     
//     public AuthorizedFilter(UsersService usersService)
//     {
//         _usersService = usersService;
//     }
//     
//     public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
//     {
//         if (!context.HttpContext.Request.Cookies.ContainsKey("auth-cookie"))
//         {
//             // Если куки нет, перенаправляем пользователя на страницу входа
//             context.Result = new RedirectToActionResult("Login", "Account", null);
//             return;
//         }
//     
//         // Получаем значение куки
//         var authCookie = context.HttpContext.Request.Cookies["auth-cookie"];
//     
//         // Проверяем валидность куки через сервис пользователей
//         var isValid = await _usersService.ValidateCookieAsync(authCookie);
//
//         var userIdClaim = ClaimsPrincipal.Current.Claims.FirstOrDefault(x => x.Type == "userId")?.Value;
//
//         if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
//         {
//             return null; // Возвращаем null, если нет userId или он некорректен
//         }
//         if (!isValid)
//         {
//             // Если кука недействительна, удаляем её и перенаправляем пользователя на страницу входа
//             context.HttpContext.Response.Cookies.Delete("auth-cookie");
//             context.Result = new RedirectToPageResult("Login");
//             return;
//         }
//     }
// }