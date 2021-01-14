using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Xant.MVC.Models.Constants;

namespace Xant.MVC.Areas.Panel.Models.ViewModels
{
    public class UserCreateFormViewModel
    {
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = ConstantValidationErrorMessages.RequiredMsg)]
        [StringLength(30, MinimumLength = 6, ErrorMessage = ConstantValidationErrorMessages.StringLengthMsg)]
        public string UserName { get; set; }
        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = ConstantValidationErrorMessages.RequiredMsg)]
        [StringLength(30, MinimumLength = 6, ErrorMessage = ConstantValidationErrorMessages.StringLengthMsg)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "تکرار کلمه عبور")]
        [Required(ErrorMessage = ConstantValidationErrorMessages.RequiredMsg)]
        [Compare(nameof(Password), ErrorMessage = ConstantValidationErrorMessages.CompareMsg)]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
        [Display(Name = "نام")]
        [Required(ErrorMessage = ConstantValidationErrorMessages.RequiredMsg)]
        [StringLength(256,MinimumLength = 3,ErrorMessage = ConstantValidationErrorMessages.StringLengthMsg)]
        public string FirstName { get; set; }
        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = ConstantValidationErrorMessages.RequiredMsg)]
        [StringLength(256, MinimumLength = 3, ErrorMessage = ConstantValidationErrorMessages.StringLengthMsg)]
        public string LastName { get; set; }
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = ConstantValidationErrorMessages.RequiredMsg)]
        [MaxLength(256, ErrorMessage = ConstantValidationErrorMessages.MaxLengthMsg)]
        [EmailAddress(ErrorMessage = ConstantValidationErrorMessages.RegexMsg)]
        public string Email { get; set; }
        [Display(Name = "بیوگرافی")]
        [MaxLength(1000,ErrorMessage = ConstantValidationErrorMessages.MaxLengthMsg)]
        public string Biography { get; set; }
        [Display(Name = "فعال بودن")]
        public bool IsActive { get; set; }
        [Display(Name = "تصویر پروفایل")]
        public IFormFile File { get; set; }
        public string FilePath { get; set; }
        [Display(Name = "نقش کاربری")]
        public UserRoleEnumViewModel Role { get; set; }
    }
}