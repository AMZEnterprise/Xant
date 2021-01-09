using System;

namespace Xant.MVC.Utility
{
    /// <summary>
    /// Date utility used to format dates to persian. Use it for views only.
    /// </summary>
    public static class FriendlyDate
    {
        /// <summary>
        /// Format date to "yyyy/MM/dd" in persian
        /// </summary>
        /// <param name="date">english date</param>
        /// <returns>returns "yyyy/MM/dd" date in persian</returns>
        public static string ShortDate(DateTime date)
        {
            return date.ToPersianDateTime().ToString("yyyy/MM/dd");
        }

        /// <summary>
        /// Format date to "yyyy/MM/d hh:mm:ss" in persian
        /// </summary>
        /// <param name="date">>english date</param>
        /// <returns>returns "yyyy/MM/d hh:mm:ss" date in persian</returns>
        public static string LongDate(DateTime date)
        {
            return date.ToPersianDigitalDateTimeString();
        }

        /// <summary>
        ///  Format date to "hh:mm" time
        /// </summary>
        /// <param name="date">english date</param>
        /// <returns>returns "hh:mm" time</returns>
        public static string Time(DateTime date)
        {
            return date.ToPersianDateTime().ToString("hh:mm");
        }
    }
}
