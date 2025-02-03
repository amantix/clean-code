using API.Extensions;
using API.Filters;
using Application.Interfaces.Services;
using Application.Services;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using Persistence;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// logging
builder.Logging.AddFile("logs.txt");

// settings
builder.Configuration.AddEnvironmentVariables();

// db
services.AddPostgresDb(configuration);
services.AddRepositories();
services.AddMinioS3(configuration);

// swagger
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

// services
services.AddScoped<IUserService, UserService>();
services.AddScoped<IDocumentService, DocumentService>();
services.AddScoped<IDocumentAccessService, DocumentAccessService>();

// md
services.AddMdProcessor();

// controllers, filters
services.AddControllers();
services.AddScoped<HasAccessFilter>();
services.AddScoped<UserExistFilter>();

// reg-auth
services.AddAuthenticator(configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
    var retryCount = 5;

    for (int i = 0; i < retryCount; i++)
    {
        try
        {
            var context = serviceProvider.GetRequiredService<AppDbContext>();
            logger.LogInformation($"Попытка {i+1} применить миграции...");
            context.Database.Migrate();
            logger.LogInformation("Миграции успешно применены.");
            break;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Ошибка при применении миграций. Попытка {i+1} из {retryCount}.");
            if (i == retryCount - 1)
            {
                throw;
            }
            Thread.Sleep(TimeSpan.FromSeconds(5));
        }
    }
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.MapControllers();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.UseCookiePolicy(new CookiePolicyOptions()
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});

app.MapGet("/", () => Results.Redirect("/Page"));
app.Run();