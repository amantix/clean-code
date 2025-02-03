using Microsoft.AspNetCore.DataProtection.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebAppMarkdown.Contracts;
using WebAppMarkdown.Services;

namespace WebAppMarkdown.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : Controller
{
    private readonly UsersService _usersService;


    public UserController(UsersService usersService) 
    {
        _usersService = usersService;
    }
    
    [HttpGet("/login")] public IActionResult GetLoginPage() => View("login");
    
    [HttpGet("/register")] public IActionResult GetRegisterPage() => View("register");

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterContract request)
    {
        await _usersService.Register(request.Email, request.Username, request.Password);
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginContract request)
    {
        var token = await _usersService.Login(request.Email, request.Password);
        Response.Cookies.Append("auth-cookie", token);
        return Ok(new { token }); // Возвращаем токен в теле ответа
    }
}