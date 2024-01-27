/*
 * Copyright 2024 Rapid Software LLC
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
 * Summary  : Represents the base class for communication channel user interface
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2021
 */

using Scada.Comm.Config;
using Scada.Comm.Drivers;
using System;

namespace Scada.Comm.Channels
{
    /// <summary>
    /// Represents the base class for communication channel user interface.
    /// <para>Представляет базовый класс пользовательского интерфейса канала связи.</para>
    /// </summary>
    public abstract class ChannelView : LibraryView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ChannelView(DriverView parentView, ChannelConfig channelConfig)
            : base(parentView)
        {
            AppConfig = parentView.AppConfig;
            ChannelConfig = channelConfig ?? throw new ArgumentNullException(nameof(channelConfig));
        }


        /// <summary>
        /// Gets the application configuration.
        /// </summary>
        /// <remarks>Do not modify the configuration.</remarks>
        public CommConfig AppConfig { get; }

        /// <summary>
        /// Gets the communication channel configuration.
        /// </summary>
        public ChannelConfig ChannelConfig { get; }
    }
}
