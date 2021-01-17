using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Xant.MVC.Models.ViewModels
{
    public class BlogSidebarViewModel
    {
        public IEnumerable<PostViewModel> PostViewModels { get; set; }
        public IEnumerable<PostCategoryViewModel> PostCategoryViewModels { get; set; }
        public IEnumerable<string> PostTags { get; set; }
        [Display(Name = "جستجوی مطلب ...")]
        public string SearchString { get; set; }
    }
}
