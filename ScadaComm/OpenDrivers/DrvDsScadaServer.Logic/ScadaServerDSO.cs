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
 * Module   : DrvDsScadaServer
 * Summary  : Represents data source options
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Config;

namespace Scada.Comm.Drivers.DrvDsScadaServer.Logic
{
    /// <summary>
    /// Represents data source options.
    /// <para>Представляет параметры источника данных.</para>
    /// </summary>
    internal class ScadaServerDSO
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ScadaServerDSO(OptionList options)
        {
            MaxQueueSize = options.GetValueAsInt("MaxQueueSize", 1000);
            MaxCurDataAge = options.GetValueAsInt("MaxCurDataAge", 60);
            DataLifetime = options.GetValueAsInt("DataLifetime", 3600);
            ClientLogEnabled = options.GetValueAsBool("ClientLogEnabled", false);
        }


        /// <summary>
        /// Gets or sets the maximum queue size.
        /// </summary>
        public int MaxQueueSize { get; set; }

        /// <summary>
        /// Gets or sets the maximum time after which the current data is sent as historical, in seconds.
        /// </summary>
        public int MaxCurDataAge { get; set; }

        /// <summary>
        /// Gets or sets the data lifetime in the queue, in seconds.
        /// </summary>
        public int DataLifetime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to write client communication log.
        /// </summary>
        public bool ClientLogEnabled { get; set; }
    }
}
