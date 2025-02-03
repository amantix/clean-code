using API.DTO;
using API.Filters;
using API.Utils;
using Application.Abstract.Services;
using Application.ResponseResult;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers;

[ApiController]
[Route("api/user")]
public class UserController(
    IUserService userService,
    ResponseResultCreator resultCreator): ControllerBase
{
    [ServiceFilter(typeof(UserRegisterValidateFilter))]
    [HttpPost("signup")]
    public async Task<IActionResult> Register([FromBody] UserRegister userRegister)
    {
        var result = await userService.AddUser(userRegister.UserName,
            userRegister.UserEmail,
            userRegister.Password);

        if (!result.IsOk)
        {
            return resultCreator.CreateAction(result);
        }

        return Ok();
    }
    
    [ServiceFilter(typeof(UserLoginEmailValidateFilter))]
    [HttpPost("login/email")]
    public async Task<IActionResult> LoginByEmail([FromBody] UserLoginEmail userLoginEmail)
    {
        var result = await userService.AuthenticateUserByEmail(
            userLoginEmail.UserEmail,
            userLoginEmail.Password);

        if (!result.IsOk)
        {
            return resultCreator.CreateAction(result);
        }
        var token = ((Result<string>)result).Value;
        
        Response.Cookies.Append("simply-cookies", token ?? "");
        return Ok();
    }
    
}