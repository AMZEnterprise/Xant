using System;
using System.ComponentModel.DataAnnotations;

namespace Xant.MVC.Areas.Panel.Models.ViewModels
{
    public class UserIndexViewModel
    {
        public string Id { get; set; }
        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }
        [Display(Name = "نام و نام خانوادگی")]
        public string UserFullName { get; set; }
        [Display(Name = "ایمیل")]
        public string Email { get; set; }
        [Display(Name = "فعال بودن")]
        public bool IsActive { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreateDate { get; set; }
        [Display(Name = "تاریخ آخرین ویرایش")]
        public DateTime LastEditDate { get; set; }
    } 
}
