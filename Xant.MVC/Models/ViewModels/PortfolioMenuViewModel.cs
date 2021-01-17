using System.Collections.Generic;

namespace Xant.MVC.Models.ViewModels
{
    public class PortfolioMenuViewModel
    {
        public int? PostCategoryId { get; set; }
        public IEnumerable<PostCategoryViewModel> PostCategoryViewModels { get; set; }
    }
}
