namespace Xant.MVC.Utility
{
    /// <summary>
    /// Social medial link generator is used for share link on posts.
    /// </summary>
    public static class SocialMediaLinkGenerator
    {
        /// <summary>
        /// Generate share link for Facebook
        /// </summary>
        /// <param name="url">url</param>
        /// <returns>return Facebook link</returns>
        public static string GenerateFacebookLink(string url)
        {
            return $"https://www.facebook.com/sharer/sharer.php?u={url}";
        }

        /// <summary>
        /// Generate share link for Telegram
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="text">text</param>
        /// <returns>return Telegram link</returns>
        public static string GenerateTelegramLink(string url, string text)
        {
            return $"https://t.me/share/url?url={url}&text={text}";
        }

        /// <summary>
        /// Generate share link for Twitter
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="text">text</param>
        /// <returns>return Twitter link</returns>
        public static string GenerateTwitterLink(string url, string text)
        {
            return $"https://twitter.com/intent/tweet?url={url}&text={text}";
        }

        /// <summary>
        /// Generate share link for GooglePlus
        /// </summary>
        /// <param name="url">url</param>
        /// <returns>return GooglePlus link</returns>
        public static string GenerateGooglePlusLink(string url)
        {
            return $"https://plus.google.com/share?url={url}";
        }

        /// <summary>
        /// Generate share link for WhatsApp
        /// </summary>
        /// <param name="url">url</param>
        /// <returns>return WhatsApp link</returns>
        public static string GenerateWhatsAppLink(string url)
        {
            return $"whatsapp://send?text={url}";
        }
    }
}
