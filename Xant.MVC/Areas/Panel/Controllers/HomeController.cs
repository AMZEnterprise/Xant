using Microsoft.AspNetCore.Mvc;
using Xant.MVC.Models.Constants;

namespace Xant.MVC.Areas.Panel.Controllers
{
    [Area(ConstantAreas.Panel)]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
