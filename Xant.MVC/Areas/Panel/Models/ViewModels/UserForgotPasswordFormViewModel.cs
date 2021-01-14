using System.ComponentModel.DataAnnotations;
using Xant.MVC.Models.Constants;

namespace Xant.MVC.Areas.Panel.Models.ViewModels
{
    public class UserForgotPasswordFormViewModel
    {
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = ConstantValidationErrorMessages.RequiredMsg)]
        [MaxLength(256, ErrorMessage = ConstantValidationErrorMessages.MaxLengthMsg)]
        [EmailAddress(ErrorMessage = ConstantValidationErrorMessages.RegexMsg)]
        public string Email { get; set; }
    }
}
