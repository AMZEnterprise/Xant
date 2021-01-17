using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;

namespace Xant.MVC.Controllers
{
    public class ServicesController : Controller
    {
        [Breadcrumb("ViewData.Title")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
