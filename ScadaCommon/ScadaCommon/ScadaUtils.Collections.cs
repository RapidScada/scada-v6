using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Scada
{
    partial class ScadaUtils
    {
        /// <summary>
        /// The array separator used for parsing.
        /// </summary>
        private static readonly char[] ParseArraySeparator = { ';', ',', ' ' };
        /// <summary>
        /// The array separator used for display.
        /// </summary>
        private static readonly string DisplayArraySeparator = ",";
        /// <summary>
        /// The range separator used for parsing.
        /// </summary>
        private static readonly char[] ParseRangeSeparator = new char[] { ',' };
        /// <summary>
        /// The range separator used for display.
        /// </summary>
        private const string DisplayRangeSeparator = ", ";
        /// <summary>
        /// The range dash used for parsing and display.
        /// </summary>
        private const char RangeDash = '-';


        /// <summary>
        /// Retrieves arguments from the specified string.
        /// </summary>
        public static IDictionary<string, string> ParseArgs(string s, char separator = '\n')
        {
            // string example:
            // argument1 = val1
            // argument2 = val2
            Dictionary<string, string> args = new Dictionary<string, string>();
            string[] parts = (s ?? "").Split(separator);

            foreach (string part in parts)
            {
                string key;
                string val;
                int idx = part.IndexOf("=");

                if (idx >= 0)
                {
                    key = part.Substring(0, idx).Trim();
                    val = part.Substring(idx + 1).Trim();
                }
                else
                {
                    key = part.Trim();
                    val = "";
                }

                args[key] = val;
            }

            return args;
        }

        /// <summary>
        /// Gets the value associated with the specified key as a string.
        /// </summary>
        public static string GetValueAsString(this IDictionary<string, string> dictionary,
            string key, string defaultValue = "")
        {
            return dictionary.TryGetValue(key, out string val) ? val : defaultValue;
        }

        /// <summary>
        /// Gets the value associated with the specified key as a boolean.
        /// </summary>
        public static bool GetValueAsBool(this IDictionary<string, string> dictionary,
            string key, bool defaultValue = false)
        {
            try
            {
                return dictionary.TryGetValue(key, out string valStr) ? bool.Parse(valStr) : defaultValue;
            }
            catch (FormatException)
            {
                throw NewFormatException(key);
            }
        }

        /// <summary>
        /// Gets the value associated with the specified key as an integer.
        /// </summary>
        public static int GetValueAsInt(this IDictionary<string, string> dictionary,
            string key, int defaultValue = 0)
        {
            try
            {
                return dictionary.TryGetValue(key, out string valStr) ? int.Parse(valStr) : defaultValue;
            }
            catch (FormatException)
            {
                throw NewFormatException(key);
            }
        }

        /// <summary>
        /// Gets the value associated with the specified key as a double.
        /// </summary>
        public static double GetValueAsDouble(this IDictionary<string, string> dictionary,
            string key, double defaultValue = 0)
        {
            try
            {
                return dictionary.TryGetValue(key, out string valStr) ?
                    double.Parse(valStr, NumberStyles.Float, NumberFormatInfo.InvariantInfo) : defaultValue;
            }
            catch (FormatException)
            {
                throw NewFormatException(key);
            }
        }

        /// <summary>
        /// Gets the value associated with the specified key as a date and time.
        /// </summary>
        public static DateTime GetValueAsDateTime(this IDictionary<string, string> dictionary,
            string key, DateTime defaultValue)
        {
            try
            {
                return dictionary.TryGetValue(key, out string valStr) ?
                    DateTime.Parse(valStr, DateTimeFormatInfo.InvariantInfo) : defaultValue;
            }
            catch (FormatException)
            {
                throw NewFormatException(key);
            }
        }

        /// <summary>
        /// Gets the value associated with the specified key as a date and time.
        /// </summary>
        public static DateTime GetValueAsDateTime(this IDictionary<string, string> dictionary, 
            string key, DateTimeKind kind)
        {
            return DateTime.SpecifyKind(dictionary.GetValueAsDateTime(key, DateTime.MinValue), kind);
        }

        /// <summary>
        /// Gets the value associated with the specified key as an enumeration element.
        /// </summary>
        public static T GetValueAsEnum<T>(this IDictionary<string, string> dictionary,
            string key, T defaultValue = default) where T : struct
        {
            try
            {
                return dictionary.TryGetValue(key, out string valStr) ?
                    (T)Enum.Parse(typeof(T), valStr, true) : defaultValue;
            }
            catch (FormatException)
            {
                throw NewFormatException(key);
            }
        }

        /// <summary>
        /// Gets the value associated with the specified key as an instance of the particular type.
        /// </summary>
        public static bool TryGetValueOfType<T>(this IDictionary<string, object> dictionary, 
            string key, out T value)
        {
            if (dictionary.TryGetValue(key, out object obj) && obj is T val)
            {
                value = val;
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }

        /// <summary>
        /// Converts the string representation of an array of integers to its array equivalent.
        /// </summary>
        public static int[] ParseIntArray(string s)
        {
            try
            {
                string[] elems = (s ?? "").Split(ParseArraySeparator, StringSplitOptions.RemoveEmptyEntries);
                int len = elems.Length;
                int[] arr = new int[len];

                for (int i = 0; i < len; i++)
                {
                    arr[i] = int.Parse(elems[i]);
                }

                return arr;
            }
            catch (FormatException ex)
            {
                throw new FormatException("The specified string is not an array of integers.", ex);
            }
        }

        /// <summary>
        /// Converts the string representation of a set of integers to its hash set equivalent.
        /// </summary>
        public static HashSet<int> ParseIntSet(string s)
        {
            try
            {
                string[] elems = (s ?? "").Split(ParseArraySeparator, StringSplitOptions.RemoveEmptyEntries);
                int len = elems.Length;
                HashSet<int> hashSet = new HashSet<int>();

                for (int i = 0; i < len; i++)
                {
                    hashSet.Add(int.Parse(elems[i]));
                }

                return hashSet;
            }
            catch (FormatException ex)
            {
                throw new FormatException("The specified string is not a set of integers.", ex);
            }
        }

        /// <summary>
        /// Converts the string representation of an integer range to a collection.
        /// </summary>
        public static IList<int> ParseRange(string s, bool allowEmpty, bool distinct)
        {
            if (ParseRange(s, allowEmpty, distinct, out IList<int> list))
                return list;
            else
                throw new FormatException("The specified string is not a valid range of integers.");
        }

        /// <summary>
        /// Converts the string representation of an integer range to a collection.
        /// </summary>
        /// <remarks>Example: 1-5, 10</remarks>
        public static bool ParseRange(string s, bool allowEmpty, bool distinct, out IList<int> list)
        {
            string[] parts = (s ?? "").Split(ParseRangeSeparator, StringSplitOptions.RemoveEmptyEntries);
            List<int> numList = new List<int>();
            HashSet<int> numSet = distinct ? new HashSet<int>() : null;

            foreach (string part in parts)
            {
                if (!string.IsNullOrWhiteSpace(part))
                {
                    int dashInd = part.IndexOf(RangeDash);

                    if (dashInd >= 0)
                    {
                        // two numbers separated by a dash
                        string s1 = part.Substring(0, dashInd);
                        string s2 = part.Substring(dashInd + 1);

                        if (int.TryParse(s1, out int n1) && int.TryParse(s2, out int n2))
                        {
                            for (int n = n1; n <= n2; n++)
                            {
                                if (numSet == null || numSet.Add(n))
                                    numList.Add(n);
                            }
                        }
                        else
                        {
                            list = null;
                            return false;
                        }
                    }
                    else
                    {
                        // single number
                        if (int.TryParse(part, out int n))
                        {
                            if (numSet == null || numSet.Add(n))
                                numList.Add(n);
                        }
                        else
                        {
                            list = null;
                            return false;
                        }
                    }
                }
            }

            if (allowEmpty || numList.Count > 0)
            {
                numList.Sort();
                list = numList;
                return true;
            }
            else
            {
                list = null;
                return false;
            }
        }

        /// <summary>
        /// Converts the collection to a short string representation using range format.
        /// </summary>
        /// <remarks>Example: 1-5, 10</remarks>
        public static string ToRangeString(this IEnumerable<int> collection)
        {
            if (collection == null)
                return "";

            List<int> list = new List<int>(collection);
            list.Sort();

            StringBuilder sb = new StringBuilder();
            int prevNum = int.MinValue; // the previous number
            int bufNum = int.MinValue;  // the number in the buffer for output

            for (int i = 0, last = list.Count - 1; i <= last; i++)
            {
                int curNum = list[i];

                if (bufNum == int.MinValue)
                    bufNum = curNum;

                if (prevNum == curNum - 1)
                {
                    if (i < last)
                        bufNum = curNum;
                    else
                        sb.Append(RangeDash).Append(curNum);
                }
                else
                {
                    if (bufNum != curNum)
                        sb.Append(RangeDash).Append(bufNum);

                    if (i > 0)
                        sb.Append(DisplayRangeSeparator);

                    sb.Append(curNum);
                    bufNum = int.MinValue;
                }

                prevNum = curNum;
            }

            return sb.ToString();
        }

        /// <summary>
        /// Converts the collection to a long string representation.
        /// </summary>
        public static string ToLongString(this IEnumerable<int> collection)
        {
            return collection == null ? "" : string.Join(DisplayArraySeparator, collection);
        }

        /// <summary>
        /// Determines whether two sequences are equal.
        /// </summary>
        public static bool SequenceEqual<T>(IEnumerable<T> a, IEnumerable<T> b)
        {
            if (a == b)
                return true;
            else if (a == null || b == null)
                return false;
            else
                return Enumerable.SequenceEqual(a, b);
        }

        /// <summary>
        /// Adds elements to the current dictionary from the other dictionary.
        /// </summary>
        public static void MergeWith<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, 
            IDictionary<TKey, TValue> otherDictionary, bool overwriteExisting)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));
            if (otherDictionary == null)
                return;

            if (overwriteExisting)
            {
                foreach (KeyValuePair<TKey, TValue> pair in otherDictionary)
                {
                    dictionary[pair.Key] = pair.Value;
                }
            }
            else
            {
                foreach (KeyValuePair<TKey, TValue> pair in otherDictionary)
                {
                    if (!dictionary.ContainsKey(pair.Key))
                        dictionary.Add(pair.Key, pair.Value);
                }
            }
        }
    }
}
