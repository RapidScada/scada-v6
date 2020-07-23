using System;
using System.Collections.Generic;
using System.Text;

namespace Scada
{
    partial class ScadaUtils
    {
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
                        sb.Append(Encoding.Default.GetString(bytes, i, 1));
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

            return bytesConverted > 0;
        }

        /// <summary>
        /// Converts the string of hexadecimal numbers to an array of bytes.
        /// </summary>
        public static bool HexToBytes(string s, out byte[] bytes, bool skipWhiteSpace = false)
        {
            if (skipWhiteSpace)
                s = RemoveWhiteSpace(s);

            int strLen = s == null ? 0 : s.Length;
            int bufLen = strLen / 2;
            bytes = new byte[bufLen];
            return HexToBytes(s, 0, bytes, 0, bufLen);
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
