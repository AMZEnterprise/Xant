using System;
using System.ComponentModel.DataAnnotations;
using Xant.MVC.Models.Constants;

namespace Xant.MVC.Areas.Panel.Models.ViewModels
{
    public class ContactFormViewModel
    {
        public int Id { get; set; }
        [Display(Name = "فرستنده")]
        [Required(ErrorMessage = ConstantValidationErrorMessages.RequiredMsg)]
        [MaxLength(256,ErrorMessage = ConstantValidationErrorMessages.MaxLengthMsg)]
        public string UserFullName { get; set; }
        [Display(Name = "موضوع")]
        [Required(ErrorMessage = ConstantValidationErrorMessages.RequiredMsg)]
        [MaxLength(256, ErrorMessage = ConstantValidationErrorMessages.MaxLengthMsg)]
        public string Subject { get; set; }
        [Display(Name = "ایمیل یا تلفن همراه")]
        [Required(ErrorMessage = ConstantValidationErrorMessages.RequiredMsg)]
        [MaxLength(256, ErrorMessage = ConstantValidationErrorMessages.MaxLengthMsg)]
        public string EmailOrPhoneNumber { get; set; }
        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = ConstantValidationErrorMessages.RequiredMsg)]
        [MaxLength(1000, ErrorMessage = ConstantValidationErrorMessages.MaxLengthMsg)]
        public string Body { get; set; }
        [Display(Name = "آیپی فرستنده")]
        public string Ip { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreateDate { get; set; }
        [Display(Name = "تاریخ آخرین ویرایش")]
        public DateTime LastEditDate { get; set; }
    }
}
