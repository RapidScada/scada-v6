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
 * Module   : ScadaCommCommon
 * Summary  : The class provides helper methods for the Communicator application and its modules
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2020
 */

namespace Scada.Comm
{
    /// <summary>
    /// The class provides helper methods for the Communicator application and its modules.
    /// <para>Класс, предоставляющий вспомогательные методы для приложения Коммуникатор и его модулей.</para>
    /// </summary>
    public static class CommUtils
    {
        /// <summary>
        /// The application version.
        /// </summary>
        public const string AppVersion = "6.0.0.0";
        /// <summary>
        /// The application log file name.
        /// </summary>
        public const string LogFileName = "ScadaComm.log";
        /// <summary>
        /// The application information file name.
        /// </summary>
        public const string InfoFileName = "ScadaComm.txt";
    }
}
