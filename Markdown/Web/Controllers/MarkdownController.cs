using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Services.Abstracts;
using Web.Requests;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarkdownController(IMarkdownService mdService) : ControllerBase
    {
        [HttpPost("convert")]
        public async Task<IActionResult> GetHtml([FromBody] MarkdownRequest request)
        {
            var htmlResult = await mdService.ConvertToHtmlAsync(request.RawText!);

            return htmlResult.IsSuccess
                ? Ok(new { Html = htmlResult.Data })
                : BadRequest(new { Error = htmlResult.ErrorMessage });
        }

        
    }
}
