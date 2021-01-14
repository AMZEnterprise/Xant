using System.ComponentModel.DataAnnotations;
using Xant.MVC.Models.Constants;

namespace Xant.MVC.Areas.Panel.Models.ViewModels
{
    public class UserLoginFormViewModel
    {
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = ConstantValidationErrorMessages.RequiredMsg)]
        [StringLength(30, MinimumLength = 6, ErrorMessage = ConstantValidationErrorMessages.StringLengthMsg)]
        public string Username { get; set; }
        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = ConstantValidationErrorMessages.RequiredMsg)]
        [StringLength(30, MinimumLength = 6, ErrorMessage = ConstantValidationErrorMessages.StringLengthMsg)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "مرا به خاطر بسپار")]
        public bool RememberMe { get; set; }
    }
}
