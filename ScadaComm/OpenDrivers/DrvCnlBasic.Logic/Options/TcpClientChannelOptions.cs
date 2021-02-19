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
 * Module   : DrvCnlBasic
 * Summary  : Represents TCP client channel options
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Comm.Channels;
using Scada.Config;

namespace Scada.Comm.Drivers.DrvCnlBasic.Logic.Options
{
    /// <summary>
    /// Represents TCP client channel options.
    /// <para>Представляет параметры канала TCP-клиент.</para>
    /// </summary>
    public class TcpClientChannelOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TcpClientChannelOptions(OptionList options)
        {
            Host = options.GetValueAsString("Host");
            TcpPort = options.GetValueAsInt("TcpPort", 502); // Modbus port
            ReconnectAfter = options.GetValueAsInt("ReconnectAfter", 5);
            StayConnected = options.GetValueAsBool("StayConnected", true);
            Behavior = options.GetValueAsEnum("Behavior", ChannelBehavior.Master);
            ConnectionMode = options.GetValueAsEnum("ConnectionMode", ConnectionMode.Individual);
        }


        /// <summary>
        /// Gets or sets the remote host.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the default remote TCP port.
        /// </summary>
        public int TcpPort { get; set; }

        /// <summary>
        /// Gets or sets the reconnect interval in seconds.
        /// </summary>
        public int ReconnectAfter { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to keep a connection open after a session.
        /// </summary>
        public bool StayConnected { get; set; }

        /// <summary>
        /// Gets or sets the channel behavior.
        /// </summary>
        public ChannelBehavior Behavior { get; set; }

        /// <summary>
        /// Gets or sets the connection mode.
        /// </summary>
        public ConnectionMode ConnectionMode { get; set; }
    }
}
