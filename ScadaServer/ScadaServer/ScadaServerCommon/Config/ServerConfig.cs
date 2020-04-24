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
 * Module   : ScadaServerCommon
 * Summary  : Represents server configuration
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Server.Config
{
    /// <summary>
    /// Represents server configuration.
    /// <para>Представляет конфигурацию сервера.</para>
    /// </summary>
    public class ServerConfig
    {
        /// <summary>
        /// The default configuration file name.
        /// </summary>
        public const string DefaultFileName = "ScadaServerConfig.xml";


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ServerConfig()
        {
            ListenerOptions = new ListenerOptions();
        }


        /// <summary>
        /// Gets the listener options.
        /// </summary>
        public ListenerOptions ListenerOptions { get; private set; }


        /// <summary>
        /// Loads the configuration from the specified file.
        /// </summary>
        public bool Load(string fileName, out string errMsg)
        {
            errMsg = "";
            return true;
        }
    }
}
