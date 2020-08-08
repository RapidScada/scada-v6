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
 * Module   : ScadaServerEngine
 * Summary  : Provides data for the CnlDataChanged event
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Data.Models;
using System;

namespace Scada.Server.Engine
{
    /// <summary>
    /// Provides data for the CnlDataChanged event.
    /// <para>Предоставляет данные для события CnlDataChanged.</para>
    /// </summary>
    internal class CnlDataChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CnlDataChangedEventArgs(CnlData cnlData, CnlData prevCnlData, 
            DateTime timestamp, DateTime prevTimestamp)
        {
            CnlData = cnlData;
            PrevCnlData = prevCnlData;
            Timestamp = timestamp;
            PrevTimestamp = prevTimestamp;
        }


        /// <summary>
        /// Gets or sets the actual data of the input channel.
        /// </summary>
        public CnlData CnlData { get; set; }

        /// <summary>
        /// Gets the previous data of the input channel.
        /// </summary>
        public CnlData PrevCnlData { get; private set; }

        /// <summary>
        /// Gets the actual timestamp of the input channel.
        /// </summary>
        public DateTime Timestamp { get; private set; }

        /// <summary>
        /// Gets the previous timestamp of the input channel.
        /// </summary>
        public DateTime PrevTimestamp { get; private set; }
    }
}
