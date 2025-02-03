using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
public class PageController: ControllerBase
{
    [HttpGet("/welcome")]
    public IActionResult GetWelcome()
    {
        return File("index.html", "text/html");
    }
    
    [HttpGet("/{documentId:guid}")]
    public IActionResult GetDocument()
    {
        return File("editor.html", "text/html");
    }
    
    [Authorize]
    [HttpGet("/home")]
    public IActionResult GetHome()
    {
        return File("home.html", "text/html");
    }
}