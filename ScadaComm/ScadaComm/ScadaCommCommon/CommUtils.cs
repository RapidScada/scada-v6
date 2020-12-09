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

using System.Text;

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
        /// <summary>
        /// The client communication log file name.
        /// </summary>
        public const string ClientLogFileName = "ScadaComm_Client.log";

        /// <summary>
        /// Gets the communication line title.
        /// </summary>
        public static string GetLineTitle(int commLineNum, string name)
        {
            return $"[{commLineNum}] {name}";
        }
        
        /// <summary>
        /// Gets the device title.
        /// </summary>
        public static string GetDeviceTitle(int deviceNum, string name)
        {
            return $"[{deviceNum}] {name}";
        }

        /// <summary>
        /// Gets the name of the communication line log file.
        /// </summary>
        public static string GetLineLogFileName(int commLineNum, string extenstion)
        {
            return "line" + commLineNum.ToString("D3") + extenstion;
        }

        /// <summary>
        /// Gets the name of the device log file.
        /// </summary>
        public static string GetDeviceLogFileName(int deviceNum, string extenstion)
        {
            return "device" + deviceNum.ToString("D3") + extenstion;
        }

        /// <summary>
        /// Determines whether the end of the string builder matches the specified string.
        /// </summary>
        public static bool EndsWith(this StringBuilder sb, string value)
        {
            int sbInd = sb.Length - 1;
            int valLen = value.Length;
            int valInd = valLen - 1;

            for (int i = 0; i < valLen; i++)
            {
                if (sbInd <= 0)
                    return false;

                if (sb[sbInd] != value[valInd])
                    return false;

                sbInd--;
                valInd--;
            }

            return true;
        }
    }
}
