using System;
using System.ComponentModel.DataAnnotations;
using Xant.Core.Domain;

namespace Xant.MVC.Areas.Panel.Models.ViewModels
{
    public class PostIndexViewModel : IEntity
    {
        public int Id { get; set; }
        [Display(Name = "عنوان")]
        public string Title { get; set; }
        [Display(Name = "دسته بندی")]
        public string PostCategoryTitle { get; set; }
        [Display(Name = "محتوا")]
        public string Body { get; set; }
        [Display(Name = "برچسب ها")]
        public string Tags { get; set; }
        [Display(Name = "نویسنده")]
        public string UserFullName { get; set; }
        [Display(Name = "باز بودن نظرات")]
        public bool IsCommentsOn { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreateDate { get; set; }
        [Display(Name = "تاریخ آخرین ویرایش")]
        public DateTime LastEditDate { get; set; }
    }
}
