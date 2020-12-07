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
 * Module   : DrvCnlBasic
 * Summary  : Implements serial port channel logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Comm.Channels;
using Scada.Comm.Config;
using Scada.Comm.Drivers.DrvCnlBasic.Logic.Options;

namespace Scada.Comm.Drivers.DrvCnlBasic.Logic
{
    /// <summary>
    /// Implements serial port channel logic.
    /// <para>Реализует логику канала последовательного порта.</para>
    /// </summary>
    internal class SerialChannelLogic : ChannelLogic
    {
        private readonly SerialChannelOptions options; // the channel options


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public SerialChannelLogic(ILineContext lineContext, ChannelConfig channelConfig)
            : base(lineContext, channelConfig)
        {
            options = new SerialChannelOptions(channelConfig.CustomOptions);
        }


        /// <summary>
        /// Gets the channel behavior.
        /// </summary>
        protected override ChannelBehavior Behavior
        {
            get
            {
                return options.Behavior;
            }
        }


        /// <summary>
        /// Opens the serial port.
        /// </summary>
        protected void OpenSerialPort()
        {
            Log.WriteLine();
            /*Log.WriteLine(Locale.IsRussian ?
                "{0} Открытие последовательного порта {1}" :
                "{0} Open serial port {1}", CommUtils.GetNowDT(), serialConn.SerialPort.PortName);
            serialConn.Open();*/
        }
    }
}
