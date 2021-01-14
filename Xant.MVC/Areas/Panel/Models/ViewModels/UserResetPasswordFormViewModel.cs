using System.ComponentModel.DataAnnotations;
using Xant.MVC.Models.Constants;

namespace Xant.MVC.Areas.Panel.Models.ViewModels
{
    public class UserResetPasswordFormViewModel
    {
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = ConstantValidationErrorMessages.RequiredMsg)]
        [MaxLength(256, ErrorMessage = ConstantValidationErrorMessages.MaxLengthMsg)]
        [EmailAddress(ErrorMessage = ConstantValidationErrorMessages.RegexMsg)]
        public string Email { get; set; }
        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = ConstantValidationErrorMessages.RequiredMsg)]
        [StringLength(30, MinimumLength = 6, ErrorMessage = ConstantValidationErrorMessages.StringLengthMsg)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "تکرار کلمه عبور")]
        [Required(ErrorMessage = ConstantValidationErrorMessages.RequiredMsg)]
        [Compare(nameof(Password), ErrorMessage = ConstantValidationErrorMessages.CompareMsg)]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public string Code { get; set; }
    }
}