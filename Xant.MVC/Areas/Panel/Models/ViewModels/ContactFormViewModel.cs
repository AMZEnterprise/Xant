using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xant.Core.Domain;
using Xant.MVC.Models.Constants;

namespace Xant.MVC.Areas.Panel.Models.ViewModels
{
    public class ContactFormViewModel : IEntity,IValidatableObject
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
        [Display(Name = "ایمیل")]
        [MaxLength(256, ErrorMessage = ConstantValidationErrorMessages.MaxLengthMsg)]
        public string Email { get; set; }
        [Display(Name = "تلفن")]
        [MaxLength(20, ErrorMessage = ConstantValidationErrorMessages.MaxLengthMsg)]
        public string PhoneNumber { get; set; }
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

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Email) && string.IsNullOrWhiteSpace(PhoneNumber))
            {
                var errorMessage = string.Format(ConstantValidationErrorMessages.RequiredMsg, "شماره تلفن یا ایمیل");
                yield return new ValidationResult(errorMessage, new[] { nameof(PhoneNumber) });
            }
        }
    }
}
