using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Xant.MVC.ViewComponents
{
    public class ServicesViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult(View());
        }
    }
}
