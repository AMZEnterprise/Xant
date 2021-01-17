using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;

namespace Xant.MVC.Controllers
{
    public class ErrorController : Controller
    {
        [HttpGet("/Error/{errorCode}")]
        [Breadcrumb("ViewData.Title")]
        public IActionResult Index(int errorCode)
        {
            return View(errorCode);
        }
    }
}
