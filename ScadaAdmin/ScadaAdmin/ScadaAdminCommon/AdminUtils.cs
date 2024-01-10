/*
 * Copyright 2024 Rapid Software LLC
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
 * Module   : ScadaAdminCommon
 * Summary  : The class provides helper methods for the Administrator application and its extensions
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2022
 */

using System.IO;
using System.Reflection;

namespace Scada.Admin
{
    /// <summary>
    /// The class provides helper methods for the Administrator application and its extensions.
    /// <para>Класс, предоставляющий вспомогательные методы для приложения Администратор и его расширений.</para>
    /// </summary>
    public static class AdminUtils
    {
        /// <summary>
        /// The application version.
        /// </summary>
        public static readonly string AppVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        /// <summary>
        /// The application log file name.
        /// </summary>
        public const string LogFileName = "ScadaAdmin.log";
        /// <summary>
        /// The extension of a project file.
        /// </summary>
        public const string ProjectExt = ".rsproj";
        /// <summary>
        /// The short name of the default project directory.
        /// </summary>
        public const string ProjectDir = "ScadaProjects";

        /// <summary>
        /// Validates the name of a project item.
        /// </summary>
        public static bool NameIsValid(string name)
        {
            return !(string.IsNullOrWhiteSpace(name) || name.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0 ||
                name.Contains(Path.DirectorySeparatorChar) || name.Contains(Path.AltDirectorySeparatorChar));
        }

        /// <summary>
        /// Gets a string representation of the checkbox value.
        /// </summary>
        public static string GetCheckedString(bool value)
        {
            return value ? "[v]" : "[  ]";
        }
    }
}
