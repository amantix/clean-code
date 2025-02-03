    using System.Reflection;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using WebAppMarkdown.Contracts;
    using WebAppMarkdown.Services;

    namespace WebAppMarkdown.Controllers;
    
    [ApiController]
    [Route("[controller]")]
    public class ConverterController : Controller
    {
        private readonly MarkdownConverterService _markdownConverterService;
        private readonly DocumentService _documentService;
        private readonly UsersService _usersService;

        public ConverterController(MarkdownConverterService markdownConverterService, DocumentService documentService, 
            UsersService usersService)
        {
            _markdownConverterService = markdownConverterService;
            _documentService = documentService;
            _usersService = usersService;
        }
        
        [Authorize]
        [HttpGet("/index")]
        public IActionResult GetConverterPage() => View("index");
        
        
        [Authorize]
        [HttpPost("convert")]
        public async Task<IActionResult> Convert([FromBody] ConvertContract contract)
        {
            var html = await _markdownConverterService.ConvertMarkdownToHtml(contract.Text);
            return Ok(new {html});
        }

        [Authorize]
        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] SaveContract contract)
        {
            _documentService.Create(_usersService.GetUserIdFromToken(User).Result.Value,
            contract.Text);
            
            return Ok(new {message = "Документ сохранён"});
        }
    }