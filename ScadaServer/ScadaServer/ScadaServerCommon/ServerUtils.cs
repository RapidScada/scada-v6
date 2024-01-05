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
 * Module   : ScadaServerCommon
 * Summary  : The class provides helper methods for the Server application and its modules
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2020
 */

namespace Scada.Server
{
    /// <summary>
    /// The class provides helper methods for the Server application and its modules.
    /// <para>Класс, предоставляющий вспомогательные методы для приложения Сервер и его модулей.</para>
    /// </summary>
    public static class ServerUtils
    {
        /// <summary>
        /// The application log file name.
        /// </summary>
        public const string LogFileName = "ScadaServer.log";
        /// <summary>
        /// The application information file name.
        /// </summary>
        public const string InfoFileName = "ScadaServer.txt";
        /// <summary>
        /// The maximum number of archives.
        /// </summary>
        public const int MaxArchiveCount = 31;

        /// <summary>
        /// Gets the archive title.
        /// </summary>
        public static string GetArchiveTitle(string code, string name)
        {
            return $"[{code}] {name}";
        }
    }
}
