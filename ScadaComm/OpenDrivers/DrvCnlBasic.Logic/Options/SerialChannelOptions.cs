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
 * Summary  : Represents options of a serial port channel
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Comm.Channels;
using Scada.Config;
using System.IO.Ports;

namespace Scada.Comm.Drivers.DrvCnlBasic.Logic.Options
{
    /// <summary>
    /// Represents options of a serial port channel.
    /// <para>Представляет параметры канала последовательного порта.</para>
    /// </summary>
    public class SerialChannelOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public SerialChannelOptions(OptionList options)
        {
            PortName = options.GetValueAsString("PortName", "COM1");
            BaudRate = options.GetValueAsInt("BaudRate", 9600);
            Parity = options.GetValueAsEnum("Parity", Parity.None);
            DataBits = options.GetValueAsInt("DataBits", 8);
            StopBits = options.GetValueAsEnum("StopBits", StopBits.One);
            DtrEnable = options.GetValueAsBool("DtrEnable", false);
            RtsEnable = options.GetValueAsBool("RtsEnable", false);
            Behavior = options.GetValueAsEnum("Behavior", ChannelBehavior.Master);
        }


        /// <summary>
        /// Gets or sets the serial port name.
        /// </summary>
        public string PortName { get; set; }

        /// <summary>
        /// Gets or sets the port baud rate.
        /// </summary>
        public int BaudRate { get; set; }

        /// <summary>
        /// Gets or sets the port parity.
        /// </summary>
        public Parity Parity { get; set; }

        /// <summary>
        /// Gets or sets the standard length of data bits per byte.
        /// </summary>
        public int DataBits { get; set; }

        /// <summary>
        /// Gets or sets the standard number of stopbits per byte.
        /// </summary>
        public StopBits StopBits { get; set; }

        /// <summary>
        /// Gets or sets a value that enables the Data Terminal Ready (DTR) signal.
        /// </summary>
        public bool DtrEnable { get; set; }

        /// <summary>
        /// Gets or sets a value that enables the Request to Send (RTS) signal.
        /// </summary>
        public bool RtsEnable { get; set; }

        /// <summary>
        /// Gets or sets the channel behavior.
        /// </summary>
        public ChannelBehavior Behavior { get; set; }
    }
}
