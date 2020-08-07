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
using System.Collections.Generic;

namespace Scada.Server.Engine
{
    /// <summary>
    /// Represents current data of the input channels.
    /// <para>Представляет текущие данные входных каналов.</para>
    /// </summary>
    internal class CurrentData : ICalcContext
    {
        private readonly Dictionary<int, CnlTag> cnlTags; // the metadata about the input channels


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CurrentData(Dictionary<int, CnlTag> cnlTags)
        {
            this.cnlTags = cnlTags ?? throw new ArgumentNullException("cnlTags");

            int cnlCnt = cnlTags.Count;
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


        /// <summary>
        /// Sets the current input channel data.
        /// </summary>
        public void SetCurCnlData(DateTime nowDT, int cnlIndex, CnlData cnlData)
        {
            PrevTimestamps[cnlIndex] = CurTimestamps[cnlIndex];
            PrevCnlData[cnlIndex] = CurCnlData[cnlIndex];

            CurTimestamps[cnlIndex] = nowDT;
            CurCnlData[cnlIndex] = cnlData;
        }

        /// <summary>
        /// Gets the actual data of the input channel.
        /// </summary>
        CnlData ICalcContext.GetCnlData(int cnlNum)
        {
            return cnlTags.TryGetValue(cnlNum, out CnlTag cnlTag) ?
                CurCnlData[cnlTag.Index] :
                CnlData.Empty;
        }

        /// <summary>
        /// Gets the previous data of the input channel, if applicable.
        /// </summary>
        CnlData ICalcContext.GetPrevCnlData(int cnlNum)
        {
            return cnlTags.TryGetValue(cnlNum, out CnlTag cnlTag) ?
                PrevCnlData[cnlTag.Index] :
                CnlData.Empty;
        }

        /// <summary>
        /// Gets the actual timestamp of the input channel.
        /// </summary>
        DateTime ICalcContext.GetCnlTime(int cnlNum)
        {
            return cnlTags.TryGetValue(cnlNum, out CnlTag cnlTag) ?
                CurTimestamps[cnlTag.Index] :
                DateTime.MinValue;
        }

        /// <summary>
        /// Gets the previous timestamp of the input channel, if applicable.
        /// </summary>
        DateTime ICalcContext.GetPrevCnlTime(int cnlNum)
        {
            return cnlTags.TryGetValue(cnlNum, out CnlTag cnlTag) ?
                PrevTimestamps[cnlTag.Index] :
                DateTime.MinValue;
        }

        /// <summary>
        /// Sets the input channel data.
        /// </summary>
        void ICalcContext.SetCnlData(int cnlNum, CnlData cnlData)
        {
            if (cnlTags.TryGetValue(cnlNum, out CnlTag cnlTag))
            {
                SetCurCnlData(DateTime.UtcNow, cnlTag.Index, cnlData);
                // TODO: generate events
            }
        }
    }
}
