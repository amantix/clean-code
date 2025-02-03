using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using WebAPI.Contracts;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentController : ControllerBase
    {
        private readonly DocumentService _documentService;
        private readonly UsersService _usersService;

        public DocumentController(
            DocumentService documentService, 
            UsersService usersService)
        {
            _documentService = documentService;
            _usersService = usersService;
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveDocumentAsync([FromBody] DocumentContract document) 
        {
            var userId = await _usersService.GetUserIdByToken(User);
            var userEntity = await _usersService.GetEntityUserById(userId);
            await _documentService.Create(userEntity.Id, document.MdText, document.FileName);
            return Ok();
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllDocumentAsync() 
        {
            var userId = await _usersService.GetUserIdByToken(User);
            var userEntity = await _usersService.GetEntityUserById(userId);
            var result = await _documentService.GetById(userEntity.Id);
            return Ok(result);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteDocumentAsync(string fileName) 
        {
            var userId = await _usersService.GetUserIdByToken(User);
            var userEntity =  await _usersService.GetEntityUserById(userId);
            await _documentService.Delete(userEntity.Id, fileName);
            return Ok();
        }
    }
}
