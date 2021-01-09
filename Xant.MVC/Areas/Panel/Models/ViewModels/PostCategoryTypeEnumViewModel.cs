using System.ComponentModel.DataAnnotations;

namespace Xant.MVC.Areas.Panel.Models.ViewModels
{
    public enum PostCategoryTypeEnumViewModel
    {
        [Display(Name = "وبلاگ")]
        Blog = 1000,
        [Display(Name = "نمونه کار")]
        Portfolio = 2000
    }
}
