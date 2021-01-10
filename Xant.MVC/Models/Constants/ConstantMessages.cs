using System;

namespace Xant.MVC.Models.Constants
{
    public class ConstantMessages
    {
        public const string IdentityDuplicateUserName = "نام کاربری وارد شده از قبل در سیستم موجود می باشد.";
        public const string IdentityError = "خطایی در پردازش درخواست شما پیش آمده است. لطفا بعدا تلاش کنید.";
        public const string IdentityAccountDisabled = "حساب کاربری شما غیر فعال شده است. لطفا بعدا امتحان کنید.";
        public const string IdentityInvalidUserName = "قالب نام کاربری وارد شده صحیح نمی باشد. مقدار دیگری را امتحان کنید.";
        public const string IdentityEmail = "ایمیل وارد شده از قبل در سیستم موجود می باشد.";
        public const string IdentityPassword = "کلمه عبور وارد شده معتبر نمی باشد. مقدار دیگری را امتحان کنید.";
        public const string IdentityAddToRoleResult = "خطایی در ثبت کاربر به وجود آمده است. لطفا بعدا تلاش کنید.";
        public const string IdentityLoginWrongPassword = "کلمه عبور اشتباه است.";
        public const string VerificationEmailSubject = "بازیابی کلمه عبور | آژانس دیجیتال مارکتینگ ژانت";
        public const string VerificationEmailText = "برای بازیابی کلمه عبور خود بر روی لینک " + " {0}" + " کلیک کنید.";
        public const string IdentityNotRegisteredYet = "شما هنوز عضو وبسایت نشده اید.";
        public const string EmailFailedOnSending  = "خطایی در ارسال ایمیل پیش آمده است. لطفا بعدا تلاش کنید.";
        public const string IdentityPasswordVerifyCode = "کد وارد شده با کد ارسال شده به ایمیل شما مطابقت ندارد.";
        public const string IdentityUpdatePassword = "خطایی در بروزرسانی کلمه عبور شما پیش آمده است. لطفا بعدا امتحان کنید.";
        public const string DuplicateError = "مقدار " + "{0} " + "از قبل موجود است. مقدار دیگری را امتحان کنید.";
    }
}
