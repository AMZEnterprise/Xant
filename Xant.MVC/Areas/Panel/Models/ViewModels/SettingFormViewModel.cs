using System.ComponentModel.DataAnnotations;
using Xant.Core.Domain;
using Xant.MVC.Models.Constants;

namespace Xant.MVC.Areas.Panel.Models.ViewModels
{
    public class SettingFormViewModel : IEntity
    {
        public int Id { get; set; }
        [Display(Name = "تلفن ۱")]
        [MaxLength(20,ErrorMessage = ConstantValidationErrorMessages.MaxLengthMsg)]
        public string Phone1 { get; set; }
        [Display(Name = "تلفن ۲")]
        [MaxLength(20, ErrorMessage = ConstantValidationErrorMessages.MaxLengthMsg)]
        public string Phone2 { get; set; }
        [Display(Name = "ایمیل ۱")]
        [MaxLength(256, ErrorMessage = ConstantValidationErrorMessages.MaxLengthMsg)]
        public string Email1 { get; set; }
        [Display(Name = "ایمیل ۲")]
        [MaxLength(256, ErrorMessage = ConstantValidationErrorMessages.MaxLengthMsg)]
        public string Email2 { get; set; }
        [Display(Name = "آدرس ۱")]
        [MaxLength(1000, ErrorMessage = ConstantValidationErrorMessages.MaxLengthMsg)]
        public string Address1 { get; set; }
        [Display(Name = "آدرس ۲")]
        [MaxLength(1000, ErrorMessage = ConstantValidationErrorMessages.MaxLengthMsg)]
        public string Address2 { get; set; }
        [Display(Name = "اینستاگرام")]
        [MaxLength(256, ErrorMessage = ConstantValidationErrorMessages.MaxLengthMsg)]
        public string Instagram { get; set; }
        [Display(Name = "تلگرام")]
        [MaxLength(256, ErrorMessage = ConstantValidationErrorMessages.MaxLengthMsg)]
        public string Telegram { get; set; }
        [Display(Name = "گوگل پلاس")]
        [MaxLength(256, ErrorMessage = ConstantValidationErrorMessages.MaxLengthMsg)]
        public string GooglePlus { get; set; }
        [Display(Name = "فیسبوک")]
        [MaxLength(256, ErrorMessage = ConstantValidationErrorMessages.MaxLengthMsg)]
        public string FaceBook { get; set; }
        [Display(Name = "لینکدین")]
        [MaxLength(256, ErrorMessage = ConstantValidationErrorMessages.MaxLengthMsg)]
        public string LinkedIn { get; set; }
        [Display(Name = "یوتیوب")]
        [MaxLength(256, ErrorMessage = ConstantValidationErrorMessages.MaxLengthMsg)]
        public string Youtube { get; set; }
        [Display(Name = "آپارات")]
        [MaxLength(256, ErrorMessage = ConstantValidationErrorMessages.MaxLengthMsg)]
        public string Aparat { get; set; }
        [Display(Name = "گیت هاب")]
        [MaxLength(256, ErrorMessage = ConstantValidationErrorMessages.MaxLengthMsg)]
        public string GitHub { get; set; }
    }
}
