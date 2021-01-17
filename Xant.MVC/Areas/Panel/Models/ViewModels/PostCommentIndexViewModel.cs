using System;
using System.ComponentModel.DataAnnotations;
using Xant.Core.Domain;

namespace Xant.MVC.Areas.Panel.Models.ViewModels
{
    public class PostCommentIndexViewModel : IEntity
    {
        public int Id { get; set; }
        [Display(Name = "فرستنده")]
        public string UserFullName { get; set; }
        [Display(Name = "عنوان مطلب")]
        public string PostTitle { get; set; }
        [Display(Name = "متن")]
        public string Body { get; set; }
        [Display(Name = "ایمیل")]
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
    }
}
