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
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;

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
        /// Generates parts of unique IDs.
        /// </summary>
        private static readonly Random UniqueIDGenerator = new Random();
        /// <summary>
        /// Determines that the application is running on Windows.
        /// </summary>
        public static readonly bool IsRunningOnWin = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        /// <summary>
        /// Determines that the application is running on .NET Core.
        /// </summary>
        public static readonly bool IsRunningOnCore = RuntimeInformation.FrameworkDescription.StartsWith(".NET Core");


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
        /// Removes file name suffixes that follow the first dot.
        /// </summary>
        public static string RemoveFileNameSuffixes(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return "";
            }
            else
            {
                int dotIndex = fileName.IndexOf('.');
                return dotIndex >= 0 ? fileName.Substring(0, dotIndex) : fileName;
            }
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

        /// <summary>
        /// Generates a positive globally unique ID.
        /// </summary>
        public static long GenerateUniqueID()
        {
            long unixTime = DateTimeOffset.Now.ToUnixTimeSeconds();
            return ((unixTime << 32) + UniqueIDGenerator.Next(1, int.MaxValue)) & 0x7FFFFFFFFFFFFFFF;
        }
    }
}
