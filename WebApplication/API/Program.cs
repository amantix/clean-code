using API.Extensions;
using API.Middlewares;
using API.Utils;
using Application.Services;
using MarkdownProccesor;
using Microsoft.AspNetCore.CookiePolicy;
using Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Configuration.AddEnvironmentVariables();

var configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
builder.Services.AddDbContext<AppDbContext>(
    options =>
    {
        options.UseNpgsql(configuration.GetConnectionString(nameof(AppDbContext)));
    });
builder.Services.Configure<MinioOptions>(configuration.GetSection(nameof(MinioOptions)));

builder.Services.AddScoped<ResponseResultCreator>();
builder.Services.AddSingleton<MarkdownToHtmlProcessor>();

builder.Services.AddApiAuthentication(configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>());
builder.Services.AddAppServices();
builder.Services.AddAppRepositories();
builder.Services.AddValidators();
builder.Services.AddFilters();

var app = builder.Build();

/*if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}*/

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ошибка при применении миграций.");
    }
}


app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});

app.UseMiddleware<LoggingMiddleware>();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();