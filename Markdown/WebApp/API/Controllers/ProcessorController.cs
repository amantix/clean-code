using API.Filters;
using Microsoft.AspNetCore.Mvc;
using API.Requests;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;


namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class ProcessorController(IMdService mdService) : ControllerBase
{
    [Authorize]
    [ServiceFilter(typeof(UserExistFilter))]
    [HttpPost("Convert")]
    public async Task<IActionResult> Convert([FromBody] MdRequest request)
    {
        var htmlRenderResult = await mdService.RenderHtml(request.MdText!);

        if (!htmlRenderResult.IsSuccess)
        {
            return BadRequest(htmlRenderResult.Message);
        }
        
        return Ok(new { Html = htmlRenderResult.Data });
    }
}