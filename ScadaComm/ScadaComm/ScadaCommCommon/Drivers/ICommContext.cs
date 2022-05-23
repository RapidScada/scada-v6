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
 * Summary  : Defines functionality to access the Communicator features
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2022
 */

using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Data.Models;
using Scada.Log;
using Scada.Storages;
using System.Collections.Generic;

namespace Scada.Comm.Drivers
{
    /// <summary>
    /// Defines functionality to access the Communicator features.
    /// <para>Определяет функциональность для доступа к функциям Коммуникатора.</para>
    /// </summary>
    public interface ICommContext
    {
        /// <summary>
        /// Gets the application configuration.
        /// </summary>
        CommConfig AppConfig { get; }

        /// <summary>
        /// Gets the application directories.
        /// </summary>
        CommDirs AppDirs { get; }

        /// <summary>
        /// Gets the application storage.
        /// </summary>
        IStorage Storage { get; }

        /// <summary>
        /// Gets the application log.
        /// </summary>
        ILog Log { get; }

        /// <summary>
        /// Gets the cached configuration database.
        /// </summary>
        /// <remarks>Can be null.</remarks>
        ConfigDatabase ConfigDatabase { get; }

        /// <summary>
        /// Gets the application level shared data.
        /// </summary>
        IDictionary<string, object> SharedData { get; }


        /// <summary>
        /// Gets all communication lines.
        /// </summary>
        ILineContext[] GetCommLines();

        /// <summary>
        /// Gets the communication line by line number.
        /// </summary>
        bool GetCommLine(int commLineNum, out ILineContext lineContext);

        /// <summary>
        /// Gets the device by device number.
        /// </summary>
        bool GetDevice(int deviceNum, out DeviceLogic deviceLogic);

        /// <summary>
        /// Sends the telecontrol command to the current application.
        /// </summary>
        void SendCommand(TeleCommand cmd, string source);
    }
}
