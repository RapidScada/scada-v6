/*
 * Copyright 2020 Mikhail Shiryaev
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * 
 * Product  : Rapid SCADA
 * Module   : ScadaData
 * Summary  : The class provides helper methods for the entire software package
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2007
 * Modified : 2020
 */

using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Scada
{
    /// <summary>
    /// The class provides helper methods for the entire software package.
    /// <para>Класс, предоставляющий вспомогательные методы для всего программного комплекса.</para>
    /// </summary>
    public static partial class ScadaUtils
    {
        /// <summary>
        /// The file name of the common configuration.
        /// </summary>
        public const string ScadaConfigFileName = "ScadaConfig.xml";
        /// <summary>
        /// The delay in thread iteration to save resources, ms.
        /// </summary>
        public const int ThreadDelay = 100;
        /// <summary>
        /// Determines that the application is running on Windows.
        /// </summary>
        public static readonly bool IsRunningOnWin = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        /// <summary>
        /// Determines that the application is running on .NET Core.
        /// </summary>
        public static readonly bool IsRunningOnCore = RuntimeInformation.FrameworkDescription.StartsWith(".NET Core");


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
        /// Adds a directory separator to the directory name if necessary.
        /// </summary>
        public static string NormalDir(string path)
        {
            path = path == null ? "" : path.Trim();
            int lastIndex = path.Length - 1;

            if (lastIndex >= 0 && 
                path[lastIndex] != Path.DirectorySeparatorChar &&
                path[lastIndex] != Path.AltDirectorySeparatorChar)
            {
                path += Path.DirectorySeparatorChar;
            }

            return path;
        }

        /// <summary>
        /// Normalizes path separators according to the operating system.
        /// </summary>
        public static string NormalPathSeparators(string path)
        {
            return path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
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
        /// Identifies a nullable type.
        /// </summary>
        public static bool IsNullable(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// Makes a full copy of the specified object.
        /// </summary>
        /// <remarks>A cloned object and its children must have the Serializable attribute.</remarks>
        public static object DeepClone(object obj, SerializationBinder binder = null)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                if (binder != null)
                    bf.Binder = binder;
                bf.Serialize(ms, obj);

                ms.Position = 0;
                return bf.Deserialize(ms);
            }
        }

        /// <summary>
        /// Makes a full copy of the specified object.
        /// </summary>
        public static T DeepClone<T>(T obj, SerializationBinder binder = null)
        {
            return (T)DeepClone((object)obj, binder);
        }
    }
}
