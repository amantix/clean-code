using Markdown;
using Microsoft.EntityFrameworkCore;
using WebAppMarkdown;
using WebAppMarkdown.Db;
using WebAppMarkdown.Db.Interfaces;
using WebAppMarkdown.Db.Repositories;
using WebAppMarkdown.Jwt;
using WebAppMarkdown.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddApiAuthentication(builder.Configuration);

    
// Добавление контекста базы данных
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(ApplicationDbContext))));

// Регистрация репозиториев
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
builder.Services.AddScoped<JwtProvider>();
builder.Services.AddScoped<PasswordHasher>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<DocumentRepository>();
builder.Services.AddScoped<DocumentService>();
builder.Services.AddScoped<UsersService>();
builder.Services.AddScoped<MarkdownToHTML>();
builder.Services.AddScoped<MarkdownParser>();
builder.Services.AddScoped<MarkdownConverterService>();

// builder.Services.AddScoped<DocumentService>();
// builder.Services.AddScoped<AuthCookieFilter>();
// builder.Services.AddScoped<MD>();
// builder.Services.AddAuthOptions(builder.Configuration);

var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapControllers();
app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
