using System.ComponentModel.DataAnnotations;

namespace Xant.MVC.Areas.Panel.Models.ViewModels
{
    public enum PostCommentStatusEnumViewModel
    {
        [Display(Name = "نامشخص")]
        Unclear = 1000,
        [Display(Name = "رد شده")]
        Rejected = 2000,
        [Display(Name = "تایید شده")]
        Accepted = 3000
    }
}
