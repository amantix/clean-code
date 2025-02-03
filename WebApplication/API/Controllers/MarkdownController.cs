using API.DTO;
using API.Utils;
using Application.Abstract.Services;
using Application.ResponseResult;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("md")]
public class MarkdownController(
    IMdService mdService, 
    ResponseResultCreator resultCreator): ControllerBase
{
    [HttpPost("convert")]
    public async Task<IActionResult> ConvertMarkdown([FromBody] MarkdownContent markdown)
    {
        var result = await mdService.GetHtml(markdown.Text);

        if (!result.IsOk)
        {
            return resultCreator.CreateAction(result);
        }
        
        var html = ((Result<string>)result).Value;
        return Ok(html);
    }
}