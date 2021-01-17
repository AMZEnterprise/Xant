using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;

namespace Xant.MVC.Controllers
{
    public class HomeController : Controller
    {
        [DefaultBreadcrumb("صفحه اصلی")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
