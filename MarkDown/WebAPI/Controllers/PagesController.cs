using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebAPI.Filter;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PagesController : Controller
    {
        [ServiceFilter(typeof(AuthCookieFilter))]
        [Authorize]
        [HttpGet("/index")]
        public IActionResult Index() => View("index");

        [HttpGet("/login")]
        public IActionResult Login() => View("login");

        [HttpGet("/reg")]
        public IActionResult SubmitData() => View("reg");
    }
}
