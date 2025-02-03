using API.Requests;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] UserRequest request)
    {
        var registerResult = await userService.Register(request.Login, request.Password);

        if (registerResult.IsFailure)
        {
            return BadRequest(registerResult.Message);
        }
        return Created();
    }
    
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] UserRequest request)
    {
        var tokenRequest = await userService.Login(request.Login, request.Password);

        if (tokenRequest.IsFailure)
        {
            return BadRequest(tokenRequest.Message);
        }

        var context = HttpContext;
        context.Response.Cookies.Append("token", tokenRequest.Data!);
        
        return Ok();
    }
}