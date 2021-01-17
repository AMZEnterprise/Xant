using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Xant.Core;
using Xant.Core.Domain;
using Xant.MVC.Models.ViewModels;

namespace Xant.MVC.ViewComponents
{
    public class PortfolioMenuViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public PortfolioMenuViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync(PortfolioMenuViewModel portfolioMenuViewModel)
        {
            portfolioMenuViewModel.PostCategoryViewModels = await _unitOfWork
                .PostCategoryRepository
                .GetAll(PostCategoryType.Portfolio)
                .Include(x => x.Posts)
                .Where(x => x.Posts.Count > 0)
                .Select(x => new PostCategoryViewModel()
                {
                    Id = x.Id,
                    Title = x.Title
                }).ToListAsync();

            return View(portfolioMenuViewModel);
        }
    }
}
