using MarkDown.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Contracts;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class MarkDownController : ControllerBase
    {
        private readonly TextService _textService;

        public MarkDownController(TextService textService) 
        {
            _textService = textService;
        }

        [HttpPost("convert")]
        public async Task<IActionResult> GetMarkDownText([FromBody] TextContract markdownText)
        {
            if (markdownText == null || String.IsNullOrEmpty(markdownText.Text)) 
            {
                throw new Exception("Вы отправили пустой текст"); 
            }

            var result = await _textService.RenderText(markdownText.Text);
            return Ok(new { result });
        }
    }
}
