using System.ComponentModel.DataAnnotations;
using Xant.MVC.Models.Constants;

namespace Xant.MVC.Areas.Panel.Models.ViewModels
{
    public class PostCommentReplyFormViewModel
    {
        public int? PostId { get; set; }
        public int ParentId { get; set; }
        [Display(Name = "نظر پدر")]
        public string ParentBody { get; set; }
        [Display(Name = "متن")]
        [Required(ErrorMessage = ConstantValidationErrorMessages.RequiredMsg)]
        [MaxLength(1000, ErrorMessage = ConstantValidationErrorMessages.MaxLengthMsg)]
        public string Body { get; set; }
        [Display(Name = "وضعیت")]
        public PostCommentStatusEnumViewModel Status { get; set; }
    }
}
