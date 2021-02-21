using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;

namespace Xant.MVC.Controllers
{
    public class AboutUsController : Controller
    {
        [Breadcrumb("درباره ما")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
