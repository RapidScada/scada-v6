using Scada.Client;
using Scada.Lang;
using Scada.Protocol;
using System;
using System.Globalization;
using System.Text;

namespace Scada
{
    partial class ScadaUtils
    {
        /// <summary>
        /// The number format that uses a dot as the decimal separator.
        /// </summary>
        private static readonly NumberFormatInfo DotNfi = new NumberFormatInfo();
        /// <summary>
        /// The number format that uses a comma as the decimal separator.
        /// </summary>
        private static readonly NumberFormatInfo CommaNfi = new NumberFormatInfo { NumberDecimalSeparator = "," };

        /// <summary>
        /// The number of milliseconds in 1 minute.
        /// </summary>
        public const int MsPerMin = 60 * 1000;
        /// <summary>
        /// The number of milliseconds in 1 hour.
        /// </summary>
        public const int MsPerHour = 60 * MsPerMin;
        /// <summary>
        /// The number of milliseconds in 1 day.
        /// </summary>
        public const int MsPerDay = 24 * MsPerMin;

        /// <summary>
        /// The service status names in English.
        /// </summary>
        private static readonly string[] ServiceStatusNamesEn = 
            { "Undefined", "Starting", "Normal", "Error", "Terminating", "Terminated" };
        /// <summary>
        /// The service status names in Russian.
        /// </summary>
        private static readonly string[] ServiceStatusNamesRu = 
            { "не определён", "запуск", "норма", "ошибка", "завершение", "завершён" };

        /// <summary>
        /// The client state names in English.
        /// </summary>
        private static readonly string[] ClientStateNamesEn = 
            { "Disconnected", "Connected", "Logged In", "Error" };
        /// <summary>
        /// The client state names in Russian.
        /// </summary>
        private static readonly string[] ClientStateNamesRu = 
            { "соединение не установлено", "соединение установлено", "вход выполнен", "ошибка" };

        /// <summary>
        /// The file reading result names in English.
        /// </summary>
        private static readonly string[] ReadingResultNamesEn =
            { "Successful", "End of file", "File not found", "File outdated" };
        /// <summary>
        /// The file reading result names in Russian.
        /// </summary>
        private static readonly string[] ReadingResultNamesRu =
            { "Успешно", "Конец файла", "Файл не найден", "Файл устарел" };


        /// <summary>
        /// Removes white space characters from the string.
        /// </summary>
        private static string RemoveWhiteSpace(string s)
        {
            StringBuilder sb = new StringBuilder();

            if (s != null)
            {
                foreach (char c in s)
                {
                    if (!char.IsWhiteSpace(c))
                        sb.Append(c);
                }
            }

            return sb.ToString();
        }


        /// <summary>
        /// Converts the string to a floating-point number.
        /// </summary>
        /// <remarks>The method allows either a period or a comma as a decimal separator.</remarks>
        public static double ParseDouble(string s)
        {
            return double.Parse(s, s.Contains(".") ? DotNfi : CommaNfi);
        }

        /// <summary>
        /// Converts the string to a floating-point number.
        /// </summary>
        /// <remarks>The method allows either a period or a comma as a decimal separator.</remarks>
        public static bool TryParseDouble(string s, out double result)
        {
            return double.TryParse(s, NumberStyles.Float, s.Contains(".") ? DotNfi : CommaNfi, out result);
        }

        /// <summary>
        /// Converts the array of bytes to a hexadecimal string.
        /// </summary>
        public static string BytesToHex(byte[] bytes)
        {
            return BytesToHex(bytes, 0, bytes == null ? 0 : bytes.Length);
        }

        /// <summary>
        /// Converts the array of bytes to a hexadecimal string.
        /// </summary>
        public static string BytesToHex(byte[] bytes, int index, int count)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = index, endIdx = index + count; i < endIdx; i++)
            {
                sb.Append(bytes[i].ToString("X2"));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Converts the array of bytes to a user friendly string.
        /// </summary>
        public static string BytesToString(byte[] bytes, int index, int count,
            bool hexFormat = true, bool skipNonPrinting = false)
        {
            StringBuilder sb = new StringBuilder();

            if (hexFormat)
            {
                for (int i = index, lastIdx = index + count - 1; i <= lastIdx; i++)
                {
                    sb.Append(bytes[i].ToString("X2"));
                    if (i < lastIdx)
                        sb.Append(" ");
                }
            }
            else
            {
                bool notSkip = !skipNonPrinting;
                for (int i = index, endIdx = index + count; i < endIdx; i++)
                {
                    byte b = bytes[i];

                    if (b >= 32)
                    {
                        sb.Append(Encoding.ASCII.GetString(bytes, i, 1));
                    }
                    else if (notSkip)
                    {
                        sb.Append("<");
                        sb.Append(b.ToString("X2"));
                        sb.Append(">");
                    }
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Converts the string of hexadecimal numbers to an array of bytes.
        /// </summary>
        /// <remarks>An empty string is a valid input.</remarks>
        public static bool HexToBytes(string s, int stringIndex, byte[] buffer, int bufferIndex, int byteCount)
        {
            int lastIndex = s == null ? 0 : s.Length - 1;
            int bytesConverted = 0;

            while (stringIndex < lastIndex && bytesConverted < byteCount)
            {
                if (byte.TryParse(s.Substring(stringIndex, 2), NumberStyles.AllowHexSpecifier,
                    CultureInfo.InvariantCulture.NumberFormat, out byte val))
                {
                    buffer[bufferIndex] = val;
                    bufferIndex++;
                    bytesConverted++;
                    stringIndex += 2;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Converts the string of hexadecimal numbers to an array of bytes.
        /// </summary>
        public static bool HexToBytes(string s, out byte[] bytes, bool skipWhiteSpace = false)
        {
            if (skipWhiteSpace)
                s = RemoveWhiteSpace(s);

            int strLen = s == null ? 0 : s.Length;

            if (strLen % 2 == 0)
            {
                int bufLen = strLen / 2;
                bytes = new byte[bufLen];
                return HexToBytes(s, 0, bytes, 0, bufLen);
            }
            else
            {
                bytes = null;
                return false;
            }
        }

        /// <summary>
        /// Converts the string of hexadecimal numbers to an array of bytes.
        /// </summary>
        public static byte[] HexToBytes(string s, bool skipWhiteSpace = false, bool throwOnFail = false)
        {
            if (HexToBytes(s, out byte[] bytes, skipWhiteSpace))
                return bytes;
            else if (throwOnFail)
                throw new FormatException(CommonPhrases.NotHexadecimal);
            else
                return null;
        }

        /// <summary>
        /// Converts the number of ticks to a date and time.
        /// </summary>
        public static DateTime TicksToTime(long ticks)
        {
            return new DateTime(ticks, DateTimeKind.Utc);
        }

        /// <summary>
        /// Converts the number of milliseconds to a date and time.
        /// </summary>
        public static DateTime MillisecondsToTime(long milliseconds)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(milliseconds).UtcDateTime;
        }

        /// <summary>
        /// Gets the number of milliseconds since 1970-01-01.
        /// </summary>
        public static long GetUnixMilliseconds(this DateTime dateTime)
        {
            return new DateTimeOffset(dateTime).ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// Gets the application name.
        /// </summary>
        public static string GetAppName(ServiceApp serviceApp)
        {
            switch (serviceApp)
            {
                case ServiceApp.Server:
                    return CommonPhrases.ServerAppName;

                case ServiceApp.Comm:
                    return CommonPhrases.CommAppName;

                case ServiceApp.Web:
                    return CommonPhrases.WebAppName;

                default:
                    return serviceApp.ToString();
            }
        }

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

        /// <summary>
        /// Converts the specified boolean value to a lowercase string representation.
        /// </summary>
        public static string ToLowerString(this bool b)
        {
            return b ? "true" : "false";
        }

        /// <summary>
        /// Converts the service status to a string.
        /// </summary>
        public static string ToString(this ServiceStatus serviceStatus, bool isRussian)
        {
            return isRussian ?
                ServiceStatusNamesRu[(int)serviceStatus] :
                ServiceStatusNamesEn[(int)serviceStatus];
        }

        /// <summary>
        /// Converts the client state to a string.
        /// </summary>
        public static string ToString(this ClientState clientState, bool isRussian)
        {
            return isRussian ?
                ClientStateNamesRu[(int)clientState] :
                ClientStateNamesEn[(int)clientState];
        }

        /// <summary>
        /// Converts the file reading result to a string.
        /// </summary>
        public static string ToString(this FileReadingResult readingResult, bool isRussian)
        {
            return isRussian ?
                ReadingResultNamesRu[(int)readingResult] :
                ReadingResultNamesEn[(int)readingResult];
        }

        /// <summary>
        /// Converts the string to a service status.
        /// </summary>
        public static ServiceStatus ParseServiceStatus(string s)
        {
            ServiceStatus serviceStatus = ServiceStatus.Undefined;

            if (!string.IsNullOrEmpty(s))
            {
                for (int i = 1, len = ServiceStatusNamesEn.Length; i < len; i++)
                {
                    if (s.Equals(ServiceStatusNamesEn[i], StringComparison.OrdinalIgnoreCase) ||
                        s.Equals(ServiceStatusNamesRu[i], StringComparison.OrdinalIgnoreCase))
                    {
                        serviceStatus = (ServiceStatus)i;
                        break;
                    }
                }
            }

            return serviceStatus;
        }
    }
}
