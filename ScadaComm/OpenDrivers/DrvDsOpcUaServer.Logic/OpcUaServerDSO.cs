/*
 * Copyright 2021 Rapid Software LLC
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
 * Module   : DrvDsOpcUaServer
 * Summary  : Represents data source options
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Scada.Config;
using System.Collections.Generic;

namespace Scada.Comm.Drivers.DrvDsOpcUaServer.Logic
{
    /// <summary>
    /// Represents data source options.
    /// <para>Представляет параметры источника данных.</para>
    /// </summary>
    internal class OpcUaServerDSO
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public OpcUaServerDSO(OptionList options)
        {
            AutoAccept = options.GetValueAsBool("AutoAccept");
            Username = options.GetValueAsString("Username");
            Password = ScadaUtils.Decrypt(options.GetValueAsString("Password"));
            ConfigFileName = options.GetValueAsString("ConfigFileName");
            DeviceFilter = new List<int>();
            DeviceFilter.AddRange(ScadaUtils.ParseRange(options.GetValueAsString("DeviceFilter"), true, true));
        }


        /// <summary>
        /// Gets a value indicating whether to automatically accept client certificates.
        /// </summary>
        public bool AutoAccept { get; private set; }

        /// <summary>
        /// Gets or sets the server username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the server password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the server configuration filename.
        /// </summary>
        public string ConfigFileName { get; set; }

        /// <summary>
        /// Gets the device IDs that filter data sent to the server.
        /// </summary>
        public List<int> DeviceFilter { get; private set; }
    }
}
