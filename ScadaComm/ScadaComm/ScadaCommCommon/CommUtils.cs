/*
 * Copyright 2022 Rapid Software LLC
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
 * Modified : 2022
 */

using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Data.Entities;
using System.Collections.Generic;
using System.Linq;
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
        /// The device status names in English.
        /// </summary>
        private static readonly string[] DeviceStatusNamesEn = { "Undefined", "Normal", "Error" };
        /// <summary>
        /// The device status names in Russian.
        /// </summary>
        private static readonly string[] DeviceStatusNamesRu = { "не определён", "норма", "ошибка" };

        /// <summary>
        /// The application log file name.
        /// </summary>
        public const string LogFileName = "ScadaComm.log";
        /// <summary>
        /// The application information file name.
        /// </summary>
        public const string InfoFileName = "ScadaComm.txt";
        /// <summary>
        /// The predefined tag code for device status.
        /// </summary>
        public const string StatusTagCode = "Status";
        /// <summary>
        /// The new line characters.
        /// </summary>
        public static char[] NewLineChars = new char[] { '\r', '\n' };

        /// <summary>
        /// Gets the communication line title.
        /// </summary>
        public static string GetLineTitle(int commLineNum, string name)
        {
            return $"[{commLineNum}] {name}";
        }
        
        /// <summary>
        /// Gets the communication line title.
        /// </summary>
        public static string GetLineTitle(LineConfig lineConfig)
        {
            return GetLineTitle(lineConfig.CommLineNum, lineConfig.Name);
        }

        /// <summary>
        /// Gets the communication line title.
        /// </summary>
        public static string GetLineTitle(CommLine commLine)
        {
            return GetLineTitle(commLine.CommLineNum, commLine.Name);
        }

        /// <summary>
        /// Gets the device title.
        /// </summary>
        public static string GetDeviceTitle(int deviceNum, string name)
        {
            return $"[{deviceNum}] {name}";
        }

        /// <summary>
        /// Gets the device title.
        /// </summary>
        public static string GetDeviceTitle(DeviceConfig deviceConfig)
        {
            return GetDeviceTitle(deviceConfig.DeviceNum, deviceConfig.Name);
        }

        /// <summary>
        /// Gets the device title.
        /// </summary>
        public static string GetDeviceTitle(Device device)
        {
            return GetDeviceTitle(device.DeviceNum, device.Name);
        }

        /// <summary>
        /// Gets the data source title.
        /// </summary>
        public static string GetDataSourceTitle(string code, string name)
        {
            return $"[{code}] {name}";
        }

        /// <summary>
        /// Gets the short name of the communication line log file.
        /// </summary>
        public static string GetLineLogFileName(int commLineNum, string extenstion)
        {
            return "line" + commLineNum.ToString("D3") + extenstion;
        }

        /// <summary>
        /// Gets the short name of the device log file.
        /// </summary>
        public static string GetDeviceLogFileName(int deviceNum, string extenstion)
        {
            return "device" + deviceNum.ToString("D3") + extenstion;
        }

        /// <summary>
        /// Converts the device status to a string.
        /// </summary>
        public static string ToString(this DeviceStatus deviceStatus, bool isRussian)
        {
            return isRussian ?
                DeviceStatusNamesRu[(int)deviceStatus] :
                DeviceStatusNamesEn[(int)deviceStatus];
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

        /// <summary>
        /// Gets a flatten list of the channel prototypes.
        /// </summary>
        public static List<CnlPrototype> GetCnlPrototypes(this List<CnlPrototypeGroup> cnlPrototypeGroups)
        {
            return cnlPrototypeGroups.SelectMany(group => group.CnlPrototypes).ToList();
        }
    }
}
