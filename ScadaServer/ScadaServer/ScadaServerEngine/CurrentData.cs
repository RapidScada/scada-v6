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
 * Summary  : Represents current data of the input channels
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
    /// Represents current data of the input channels.
    /// <para>Представляет текущие данные входных каналов.</para>
    /// </summary>
    internal class CurrentData
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CurrentData(int cnlCnt)
        {
            CurCnlData = new CnlData[cnlCnt];
            PrevCnlData = new CnlData[cnlCnt];
            CurTimestamps = new DateTime[cnlCnt];
            PrevTimestamps = new DateTime[cnlCnt];
        }

        
        /// <summary>
        /// Gets the current channel data.
        /// </summary>
        public CnlData[] CurCnlData { get; private set; }
        
        /// <summary>
        /// Gets the previous channel data.
        /// </summary>
        public CnlData[] PrevCnlData { get; private set; }

        /// <summary>
        /// Gets the timestamps of the current channel data.
        /// </summary>
        public DateTime[] CurTimestamps { get; private set; }

        /// <summary>
        /// Gets the timestamps of the previous channel data.
        /// </summary>
        public DateTime[] PrevTimestamps { get; private set; }
    }
}
