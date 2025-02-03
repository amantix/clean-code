using MarkDown.Classes;
using MarkDown.DataBase;
using MarkDown.DataBase.repository;
using MarkDown.Infastructure;
using Microsoft.EntityFrameworkCore;
using WebAPI.AuthCheck;
using WebAPI.Filter;
using WebAPI.Services;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            builder.Services.Configure<JwtOption>(configuration.GetSection(nameof(JwtOption)));
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddControllersWithViews();
         
            builder.Services.AddDbContext<MyDbContext>(
                options =>
                {
                    options.UseNpgsql(configuration.GetConnectionString(nameof(MyDbContext)));
                });
            builder.Services.AddScoped<JwtProvider>();
            builder.Services.AddScoped<JwtOption>();
            builder.Services.AddScoped<PasswordHasher>();
            builder.Services.AddScoped<UsersRepository>();
            builder.Services.AddScoped<DocumentsRepository>();
            builder.Services.AddScoped<UsersService>();
            builder.Services.AddScoped<TextService>();
            builder.Services.AddScoped<DocumentService>();
            builder.Services.AddScoped<AuthCookieFilter>();
            builder.Services.AddScoped<MD>();
            builder.Services.AddAuthOption(configuration);

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.UseStaticFiles();
            app.MapDefaultControllerRoute();

            app.Run();
        }
    }
}
