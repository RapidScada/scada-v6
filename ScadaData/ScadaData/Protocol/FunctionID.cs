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
 * Summary  : Specifies the function IDs of the application protocol
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

using System.Collections.Generic;

namespace Scada.Protocol
{
    /// <summary>
    /// Specifies the function IDs of the application protocol.
    /// <para>Задает идентификаторы функций протокола приложения.</para>
    /// </summary>
    public static class FunctionID
    {
        public const ushort GetSessionInfo = 0x0001;
        public const ushort Login = 0x0002;
        public const ushort GetStatus = 0x0003;
        public const ushort TerminateSession = 0x0004;

        public const ushort GetFileInfo = 0x0101;
        public const ushort DownloadFile = 0x0102;
        public const ushort UploadFile = 0x0103;

        public const ushort GetCurrentData = 0x0201;
        public const ushort GetTrends = 0x0202;
        public const ushort GetSlice = 0x0203;
        public const ushort WriteCurrentData = 0x0204;
        public const ushort WriteHistoricalData = 0x0205;

        public const ushort GetEventByID = 0x0301;
        public const ushort GetEvents = 0x0302;
        public const ushort WriteEvent = 0x0303;
        public const ushort AckEvent = 0x0304;

        public const ushort SendCommand = 0x0401;
        public const ushort GetCommand = 0x0402;
        public const ushort DisableGettingCommands = 0x0403;

        private static readonly Dictionary<ushort, string> FunctionNames = new Dictionary<ushort, string>
        {
            { GetSessionInfo, "GetSessionInfo" },
            { Login, "Login" },
            { GetStatus, "GetStatus" },
            { TerminateSession, "TerminateSession" },
            { GetFileInfo, "GetFileInfo" },
            { DownloadFile, "DownloadFile" },
            { UploadFile, "UploadFile" },
            { GetCurrentData, "GetCurrentData" },
            { GetTrends, "GetTrends" },
            { GetSlice, "GetSlice" },
            { WriteCurrentData, "WriteCurrentData" },
            { WriteHistoricalData, "WriteHistoricalData" },
            { GetEventByID, "GetEventByID" },
            { GetEvents, "GetEvents" },
            { WriteEvent, "WriteEvent" },
            { AckEvent, "AckEvent" },
            { SendCommand, "SendCommand" },
            { GetCommand, "GetCommand" },
            { DisableGettingCommands, "DisableGettingCommands" }
        };

        /// <summary>
        /// Gets the function name by ID.
        /// </summary>
        public static string GetName(ushort functionID)
        {
            return FunctionNames.TryGetValue(functionID, out string name) ?
                name : "Unknown";
        }

        /// <summary>
        /// Determines whether the function requires a client to be logged in.
        /// </summary>
        public static bool RequiresLoggedIn(uint functionID)
        {
            return functionID > TerminateSession;
        }
    }
}
