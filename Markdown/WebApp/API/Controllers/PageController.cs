using Microsoft.AspNetCore.Mvc;


namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class PageController() : ControllerBase
{
    [HttpGet("")]
    public IActionResult Index()
    {
        return File("MainPage/index.html", "text/html");
    }

    [HttpGet("Convertor")]
    public IActionResult ShowConvertorPage()
    {
        return File("ConvertorPage/index.html", "text/html");
    }
}