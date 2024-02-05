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
 * Module   : ScadaCommEngine
 * Summary  : The class provides helper methods for this library
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2023
 */

using Scada.Lang;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Scada.Comm.Engine
{
    /// <summary>
    /// The class provides helper methods for this library.
    /// <para>Класс, предоставляющий вспомогательные методы для данной библиотеки.</para>
    /// </summary>
    internal static class EngineUtils
    {
        /// <summary>
        /// The application version.
        /// </summary>
        public static readonly string AppVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        /// <summary>
        /// Appends information about the shared data to the string builder.
        /// </summary>
        public static void AppendInfo(this IDictionary<string, object> sharedData, StringBuilder sb)
        {
            if (sharedData != null && sharedData.Count > 0)
            {
                string header = Locale.IsRussian ?
                    $"Общие данные ({sharedData.Count})" :
                    $"Shared Data ({sharedData.Count})";

                sb
                    .AppendLine()
                    .AppendLine(header)
                    .Append('-', header.Length).AppendLine();

                foreach (KeyValuePair<string, object> pair in sharedData)
                {
                    sb
                        .Append(pair.Key)
                        .Append(" = ")
                        .AppendLine(pair.Value?.ToString() ?? "null");
                }
            }
        }
    }
}
