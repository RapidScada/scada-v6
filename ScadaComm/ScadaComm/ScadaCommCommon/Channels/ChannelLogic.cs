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
 * Module   : ScadaCommCommon
 * Summary  : Represents the base class for communication channel logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Comm.Config;
using Scada.Comm.Drivers;
using System;

namespace Scada.Comm.Channels
{
    /// <summary>
    /// Represents the base class for communication channel logic.
    /// <para>Представляет базовый класс логики канала связи.</para>
    /// </summary>
    /// <remarks>The base class can be used to create a stub.</remarks>
    public class ChannelLogic
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ChannelLogic(ILineContext lineContext, ChannelConfig channelConfig)
        {
            LineContext = lineContext ?? throw new ArgumentNullException(nameof(lineContext));
            ChannelConfig = channelConfig ?? throw new ArgumentNullException(nameof(channelConfig));
            Title = channelConfig.TypeName;
        }


        /// <summary>
        /// Gets the communication line context.
        /// </summary>
        protected ILineContext LineContext { get; }

        /// <summary>
        /// Gets the communication channel configuration.
        /// </summary>
        protected ChannelConfig ChannelConfig { get; }

        /// <summary>
        /// Gets the communication channel title.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets the current communication channel status as text.
        /// </summary>
        public virtual string StatusText
        {
            get
            {
                return Locale.IsRussian ?
                    "Не определён" :
                    "Undefined";
            }
        }


        /// <summary>
        /// Starts the communication channel.
        /// </summary>
        /// <remarks>In case of an exception, the communication channel is restarted.</remarks>
        public virtual void Start()
        {
        }

        /// <summary>
        /// Stops the communication channel.
        /// </summary>
        public virtual void Stop()
        {
        }

        /// <summary>
        /// Performs actions before polling the specified device.
        /// </summary>
        public virtual void BeforeSession(DeviceLogic deviceLogic)
        {
        }

        /// <summary>
        /// Performs actions after polling the specified device.
        /// </summary>
        public virtual void AfterSession(DeviceLogic deviceLogic)
        {
        }
    }
}
