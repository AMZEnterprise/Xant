using Microsoft.AspNetCore.Mvc;
using Xant.MVC.Areas.Panel.Controllers;

namespace Xant.MVC.Areas.Panel.Extensions
{
    /// <summary>
    /// Url helper extensions
    /// </summary>
    public static class UrlHelperExtensions
    {
        /// <summary>
        /// User password reset link generator
        /// </summary>
        /// <param name="urlHelper">Url helper</param>
        /// <param name="userId">User id</param>
        /// <param name="code">Reset code</param>
        /// <param name="scheme">Website scheme</param>
        /// <returns>Password reset link</returns>
        public static string ResetPasswordCallbackLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(UsersController.ResetPassword),
                controller: "Users",
                values: new { userId, code },
                protocol: scheme);
        }
    }
}
