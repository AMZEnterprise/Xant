using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xant.MVC.Models.ViewModels;

namespace Xant.MVC.ViewComponents
{
    public class PostCommentViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(PostCommentTreeViewModel postCommentTreeViewModel)
        {
            return await Task.FromResult(View(postCommentTreeViewModel));
        }
    }
}
