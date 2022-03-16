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
 * Summary  : Defines functionality to access the communication line features
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2022
 */

using Scada.Comm.Channels;
using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Data.Models;
using Scada.Log;
using System;
using System.Collections.Generic;

namespace Scada.Comm.Drivers
{
    /// <summary>
    /// Defines functionality to access the communication line features.
    /// <para>Определяет функциональность для доступа к функциям линии связи.</para>
    /// </summary>
    public interface ILineContext
    {
        /// <summary>
        /// Gets the communication line configuration.
        /// </summary>
        LineConfig LineConfig { get; }

        /// <summary>
        /// Gets the communication line number.
        /// </summary>
        int CommLineNum { get; }

        /// <summary>
        /// Gets the communication line title.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Gets the communication line log.
        /// </summary>
        ILog Log { get; }

        /// <summary>
        /// Gets the shared data of the communication line.
        /// </summary>
        IDictionary<string, object> SharedData { get; }

        /// <summary>
        /// Get the communication channel.
        /// </summary>
        ChannelLogic Channel { get; }


        /// <summary>
        /// Selects all devices on the communication line.
        /// </summary>
        IEnumerable<DeviceLogic> SelectDevices();

        /// <summary>
        /// Selects devices on the communication line that satisfy the condition.
        /// </summary>
        IEnumerable<DeviceLogic> SelectDevices(Func<DeviceLogic, bool> predicate);

        /// <summary>
        /// Gets the device by device number.
        /// </summary>
        bool GetDevice(int deviceNum, out DeviceLogic deviceLogic);

        /// <summary>
        /// Gets the device by numeric address.
        /// </summary>
        bool GetDeviceByAddress(int numAddress, out DeviceLogic deviceLogic);

        /// <summary>
        /// Gets the device by string address.
        /// </summary>
        bool GetDeviceByAddress(string strAddress, out DeviceLogic deviceLogic);

        /// <summary>
        /// Enqueues the telecontrol command.
        /// </summary>
        void EnqueueCommand(TeleCommand cmd);
    }
}
