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
 * Module   : ScadaCommEngine
 * Summary  : Represents a wrapper for safely calling methods of communication channel
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Comm.Channels;
using Scada.Comm.Devices;
using Scada.Comm.Lang;
using Scada.Log;
using System;

namespace Scada.Comm.Engine
{
    /// <summary>
    /// Represents a wrapper for safely calling methods of communication channel.
    /// <param>Представляет обёртку для безопасного выполнения методов канала связи.</param>
    /// </summary>
    internal class ChannelWrapper
    {
        private readonly ILog log; // the communication line log


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ChannelWrapper(ChannelLogic channelLogic, ILog log)
        {
            ChannelLogic = channelLogic ?? throw new ArgumentNullException(nameof(channelLogic));
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }


        /// <summary>
        /// Gets the communication channel logic.
        /// </summary>
        public ChannelLogic ChannelLogic { get; }


        /// <summary>
        /// Calls the Start method of the communication channel.
        /// </summary>
        public bool Start()
        {
            try
            {
                ChannelLogic.Start();
                return true;
            }
            catch (ScadaException ex)
            {
                log.WriteError(ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                log.WriteError(ex, CommPhrases.ErrorInChannel, nameof(Start), ChannelLogic.Title);
                return false;
            }
        }

        /// <summary>
        /// Calls the Stop method of the communication channel.
        /// </summary>
        public void Stop()
        {
            try
            {
                ChannelLogic.Stop();
            }
            catch (ScadaException ex)
            {
                log.WriteError(ex.Message);
            }
            catch (Exception ex)
            {
                log.WriteError(ex, CommPhrases.ErrorInChannel, nameof(Stop), ChannelLogic.Title);
            }
        }

        /// <summary>
        /// Calls the BeforeSession method of the communication channel.
        /// </summary>
        public void BeforeSession(DeviceLogic deviceLogic)
        {
            try
            {
                ChannelLogic.BeforeSession(deviceLogic);
            }
            catch (ScadaException ex)
            {
                log.WriteError(ex.Message);
            }
            catch (Exception ex)
            {
                log.WriteError(ex, CommPhrases.ErrorInChannel, nameof(BeforeSession), ChannelLogic.Title);
            }
        }

        /// <summary>
        /// Calls the AfterSession method of the communication channel.
        /// </summary>
        public void AfterSession(DeviceLogic deviceLogic)
        {
            try
            {
                ChannelLogic.AfterSession(deviceLogic);
            }
            catch (ScadaException ex)
            {
                log.WriteError(ex.Message);
            }
            catch (Exception ex)
            {
                log.WriteError(ex, CommPhrases.ErrorInChannel, nameof(AfterSession), ChannelLogic.Title);
            }
        }
    }
}
