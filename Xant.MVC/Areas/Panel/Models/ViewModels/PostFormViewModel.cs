using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Xant.Core.Domain;
using Xant.MVC.Models.Constants;

namespace Xant.MVC.Areas.Panel.Models.ViewModels
{
    public class PostFormViewModel : IEntity
    {
        public int Id { get; set; }
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = ConstantValidationErrorMessages.RequiredMsg)]
        [MaxLength(256, ErrorMessage = ConstantValidationErrorMessages.MaxLengthMsg)]
        public string Title { get; set; }
        public Guid FilesPathGuid { get; set; }
        [Display(Name = "محتوا")]
        [Required(ErrorMessage = ConstantValidationErrorMessages.RequiredMsg)]
        public string Body { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreateDate { get; set; }
        [Display(Name = "تاریخ آخرین ویرایش")]
        public DateTime LastEditDate { get; set; }
        [Display(Name = "برچسب ها")]
        [MaxLength(1000, ErrorMessage = ConstantValidationErrorMessages.MaxLengthMsg)]
        public string Tags { get; set; }
        [Display(Name = "دسته بندی")]
        public int? PostCategoryId { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        [Display(Name = "نویسنده")]
        public string UserFullName { get; set; }
        [Display(Name = "باز بودن نظرات")]
        public bool IsCommentsOn { get; set; }
        [Display(Name = "تصویر اصلی")]
        public IFormFile File { get; set; }
        public string FilePath { get; set; }
    }
}
