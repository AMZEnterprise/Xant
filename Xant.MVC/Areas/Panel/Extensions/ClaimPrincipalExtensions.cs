using System;
using System.Security.Claims;

namespace Xant.MVC.Areas.Panel.Extensions
{
    /// <summary>
    /// Claim principal extensions
    /// </summary>
    public static class ClaimPrincipalExtensions
    {
        /// <summary>
        /// Get current user id
        /// </summary>
        /// <param name="principal">claim principal</param>
        /// <returns></returns>
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
