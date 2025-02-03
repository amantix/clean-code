using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppMarkdown.Models;
using WebAppMarkdown.Services;

namespace WebAppMarkdown.Controllers;

public class DashboardController : Controller
{
    
    private readonly UsersService _usersService;
    private readonly DocumentService _documentService;

    public DashboardController(UsersService usersService, DocumentService documentService)
    {
        _usersService = usersService;
        _documentService = documentService;
    }
    
    [HttpGet("/")]
    public IActionResult GetDashboard() => View("dashboard");
    
    // [HttpGet("/documents")] public IActionResult GetDocuments();
    
    [Authorize]
    [HttpGet("/create")] public IActionResult GetCreate() => Redirect("/index");

    [Authorize]
    [HttpGet("/open")]
    public IActionResult GetAllDocuments()
    {
        var userId = _usersService.GetUserIdFromToken(User).Result.Value;
        return RedirectToPage("/All", new { userId = userId });;
    }
    
    [Authorize]
    [HttpGet("/open/{id}")]
    public IActionResult GetDocument(Guid id)
    {
        var userId = _usersService.GetUserIdFromToken(User).Result.Value;
        var doc = _documentService.GetDocumentsByDocId(id).Result;
    
        if (doc.Count != 0 && doc.First().UserId == userId)
        {
            var model = new IndexModel(doc.First()); 
            return View("index", doc); 
        }
        return RedirectToPage("/All", new { userId = userId });
    }

    
    
    
    
    
    [HttpGet("/logout")] public IActionResult Logout()
    {
        Response.Cookies.Delete("auth-cookie");
        return Redirect("/dashboard");
    }
}