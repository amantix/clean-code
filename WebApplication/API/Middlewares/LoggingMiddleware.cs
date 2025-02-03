using System.Net;

namespace API.Middlewares;

public class LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        httpContext.Request.EnableBuffering();
        using var streamReader = new StreamReader(httpContext.Request.Body);
        var body = await streamReader.ReadToEndAsync();
        httpContext.Request.Body.Position = 0;
        var headers = httpContext.Request.Headers
            .Select(x => $"{x.Key}: {string.Join(", ", x.Value.ToString())}");
        
        var requestAbout = $"Id: {httpContext.Connection.Id}\n" +
                           $"Method: {httpContext.Request.Method}\n" +
                           $"Url: {httpContext.Request.Path}\n" +
                           $"Headers: {headers}\n" +
                           $"Body: {body}";
        logger.LogInformation(requestAbout);
            
        await next(httpContext);
        
        
        var responseAbout = $"Id: {httpContext.Connection.Id}\n" +
                            $"Status code: {httpContext.Response.StatusCode}\n ";
        logger.LogInformation(responseAbout);
    }
}