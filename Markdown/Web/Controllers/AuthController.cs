using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Abstracts;
using Markdown.Validators;
using Markdown.Requests;
using System.Threading.Tasks;
using System.Linq;

namespace Markdown.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController(IUserService userService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest? request)
        {
            if (request == null)
                return BadRequest("Invalid request");

            var validator = new UserValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(errors => errors.ErrorMessage));

            var registerResult = await userService.RegistrationAsync(request.Email!, request.Password!, request.Username!);

            return registerResult.IsSuccess
                ? Ok(new { Token = registerResult.Data })
                : BadRequest(new { Error = registerResult.ErrorMessage });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest? request)
        {
            if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                return Unauthorized();

            var tokenResult = await userService.AuthorizeAsync(request.Email, request.Password);
            if (!tokenResult!.IsSuccess)
                return BadRequest(new { Error = tokenResult.ErrorMessage });
            var user = await userService.GetUserByEmailAsync(request.Email);

            return Ok(new { User = user.Data, Token = tokenResult.Data });
        }

    }
}
