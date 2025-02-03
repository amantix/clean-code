using Microsoft.AspNetCore.Http;

namespace Markdown.Extensions
{
    public static class ContextExtension
    {
        public static int? GetUserId(this HttpContext context)
        {
            return int.TryParse(context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value, out var Id) ? Id : null;
        }
    }
}
