using System;
using System.ComponentModel.DataAnnotations;
using Xant.Core.Domain;
using Xant.MVC.Models.Constants;

namespace Xant.MVC.Areas.Panel.Models.ViewModels
{
    public class PostCommentFormViewModel : IEntity
    {
        public int Id { get; set; }
        [Display(Name = "فرستنده")]
        [Required(ErrorMessage = ConstantValidationErrorMessages.RequiredMsg)]
        [MaxLength(256, ErrorMessage = ConstantValidationErrorMessages.MaxLengthMsg)]
        public string UserFullName { get; set; }
        [Display(Name = "مطلب")]
        public string PostTitle { get; set; }
        [Display(Name = "متن")]
        [Required(ErrorMessage = ConstantValidationErrorMessages.RequiredMsg)]
        [MaxLength(1000, ErrorMessage = ConstantValidationErrorMessages.MaxLengthMsg)]
        public string Body { get; set; }
        [Display(Name = "متن نظر پدر")]
        public string ParentBody { get; set; }
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = ConstantValidationErrorMessages.RequiredMsg)]
        [MaxLength(256, ErrorMessage = ConstantValidationErrorMessages.MaxLengthMsg)]
        public string Email { get; set; }
        [Display(Name = "وضعیت")]
        public PostCommentStatusEnumViewModel Status { get; set; }
        [Display(Name = "آیپی فرستنده")]
        public string Ip { get; set; }
        [Display(Name = "ویرایش شده")]
        public bool IsEdited { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreateDate { get; set; }
        [Display(Name = "تاریخ آخرین ویرایش")]
        public DateTime LastEditDate { get; set; }
        public int? ParentId { get; set; }
        public int PostId { get; set; }
        public string PostUserId { get; set; }
    }
}