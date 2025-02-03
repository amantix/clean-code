using System.Text;
using API.Filters;
using API.Validations;
using Application.Abstract.Repositories;
using Application.Abstract.Services;
using Application.Repositories;
using Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions;

public static class ServicesCollectionExtensions
{
    public static void AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IMdService, MdService>();
        services.AddScoped<IMinioService, MinioService>();
        services.AddScoped<IDocumentService, DocumentService>();
        services.AddScoped<IDocumentAccessService, DocumentAccessService>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IJwtWorker, JwtWorker>();
    }

    public static void AddAppRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IDocumentRepository, DocumentRepository>();
        services.AddScoped<IDocumentPermissionRepository, DocumentPermissionRepository>();
    }

    public static void AddFilters(this IServiceCollection services)
    {
        services.AddScoped<DocumentAccessFilter>();
        services.AddScoped<DocumentReadFilter>();
        services.AddScoped<DocumentEditFilter>();
        services.AddScoped<UserExistFilter>();
        services.AddScoped<DocumentExistFilter>();
        services.AddScoped<UserRegisterValidateFilter>();
        services.AddScoped<UserLoginEmailValidateFilter>();
    }

    public static void AddValidators(this IServiceCollection services)
    {
        services.AddScoped<UserLoginEmailValidator>();
        services.AddScoped<UserRegisterValidator>();
    }
    public static void AddApiAuthentication(
        this IServiceCollection services,
        JwtOptions jwtOptions)
    {
        if (jwtOptions == null)
        {
            throw new ApplicationException("JWT is not configured");
        }

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtOptions.Secret))
                };

                options.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = 403;
                        context.Response.ContentType = "application/json";
                        return context.Response.WriteAsync("{\"error\": \"Access Denied\"}");
                    },
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["simply-cookies"];
                        return Task.CompletedTask;
                    }
                };
            });
        services.AddAuthorization();
    }
}