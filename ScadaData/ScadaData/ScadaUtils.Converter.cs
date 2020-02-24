using System;
using System.Collections.Generic;
using System.Text;

namespace Scada
{
    partial class ScadaUtils
    {
        /// <summary>
        /// Converts the specified value to a string representation using the selected culture.
        /// </summary>
        public static string ToLocalizedString(this DateTime dateTime)
        {
            return dateTime.ToLocalizedDateString() + " " + dateTime.ToLocalizedTimeString();
        }

        /// <summary>
        /// Converts the specified value to a string representation of the date using the selected culture.
        /// </summary>
        public static string ToLocalizedDateString(this DateTime dateTime)
        {
            return dateTime.ToString("d", Locale.Culture);
        }

        /// <summary>
        /// Converts the specified value to a string representation of the time using the selected culture.
        /// </summary>
        public static string ToLocalizedTimeString(this DateTime dateTime)
        {
            return dateTime.ToString("T", Locale.Culture);
        }
    }
}
