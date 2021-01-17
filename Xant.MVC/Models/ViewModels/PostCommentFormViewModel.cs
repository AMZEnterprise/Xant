using System.ComponentModel.DataAnnotations;
using Xant.MVC.Models.Constants;

namespace Xant.MVC.Models.ViewModels
{
    public class PostCommentFormViewModel
    {
        public int? ParentId { get; set; }
        public int PostId { get; set; }
        [Display(Name = "نام")]
        [Required(ErrorMessage = ConstantValidationErrorMessages.RequiredMsg)]
        [MaxLength(256, ErrorMessage = ConstantValidationErrorMessages.MaxLengthMsg)]
        public string UserFullName { get; set; }
        [Display(Name = "متن")]
        [Required(ErrorMessage = ConstantValidationErrorMessages.RequiredMsg)]
        [MaxLength(1000, ErrorMessage = ConstantValidationErrorMessages.MaxLengthMsg)]
        public string Body { get; set; }
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = ConstantValidationErrorMessages.RequiredMsg)]
        [MaxLength(256, ErrorMessage = ConstantValidationErrorMessages.MaxLengthMsg)]
        [EmailAddress(ErrorMessage = ConstantValidationErrorMessages.RegexMsg)]
        public string Email { get; set; }
    }
}
