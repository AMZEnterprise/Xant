using System.ComponentModel.DataAnnotations;
using Xant.Core.Domain;
using Xant.MVC.Models.Constants;

namespace Xant.MVC.Models.ViewModels
{
    public class ContactFormViewModel : IEntity
    {
        public int Id { get; set; }
        [Display(Name = "نام و نام خانوادگی")]
        [Required(ErrorMessage = ConstantValidationErrorMessages.RequiredMsg)]
        [MaxLength(256, ErrorMessage = ConstantValidationErrorMessages.MaxLengthMsg)]
        public string UserFullName { get; set; }
        [Display(Name = "موضوع")]
        [Required(ErrorMessage = ConstantValidationErrorMessages.RequiredMsg)]
        [MaxLength(256, ErrorMessage = ConstantValidationErrorMessages.MaxLengthMsg)]
        public string Subject { get; set; }
        [Display(Name = "ایمیل یا شماره تلفن")]
        [Required(ErrorMessage = ConstantValidationErrorMessages.RequiredMsg)]
        [MaxLength(256, ErrorMessage = ConstantValidationErrorMessages.MaxLengthMsg)]
        public string EmailOrPhoneNumber { get; set; }
        [Display(Name = "توضیح")]
        [Required(ErrorMessage = ConstantValidationErrorMessages.RequiredMsg)]
        [MaxLength(1000, ErrorMessage = ConstantValidationErrorMessages.MaxLengthMsg)]
        public string Body { get; set; }
    }
}
