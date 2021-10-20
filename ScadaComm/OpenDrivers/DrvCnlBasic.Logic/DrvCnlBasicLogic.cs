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
 * Summary  : Implements the driver logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Comm.Channels;
using Scada.Comm.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Comm.Drivers.DrvCnlBasic.Logic
{
    /// <summary>
    /// Implements the driver logic.
    /// <para>Реализует логику драйвера.</para>
    /// </summary>
    public class DrvCnlBasicLogic : DriverLogic
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DrvCnlBasicLogic(ICommContext commContext)
            : base(commContext)
        {
        }

        /// <summary>
        /// Gets the driver code.
        /// </summary>
        public override string Code
        {
            get
            {
                return "DrvCnlBasic";
            }
        }

        /// <summary>
        /// Creates a new communication channel.
        /// </summary>
        public override ChannelLogic CreateChannel(ILineContext lineContext, ChannelConfig channelConfig)
        {
            switch (channelConfig.TypeCode)
            {
                case ChannelTypeName.Serial:
                    return new SerialChannelLogic(lineContext, channelConfig);
                default:
                    return null;
            }
        }
    }
}
