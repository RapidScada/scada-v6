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
            CnlData = new CnlData[cnlCnt];
            PrevCnlData = new CnlData[cnlCnt];
            Timestamps = new DateTime[cnlCnt];
            PrevTimestamps = new DateTime[cnlCnt];
        }


        /// <summary>
        /// Gets the current data of the input channels.
        /// </summary>
        public CnlData[] CnlData { get; private set; }

        /// <summary>
        /// Gets the previous data of the input channels.
        /// </summary>
        public CnlData[] PrevCnlData { get; private set; }

        /// <summary>
        /// Gets the current timestamps of the input channels.
        /// </summary>
        public DateTime[] Timestamps { get; private set; }

        /// <summary>
        /// Gets the previous timestamps of the input channels.
        /// </summary>
        public DateTime[] PrevTimestamps { get; private set; }


        /// <summary>
        /// Raises the CnlDataChanged event.
        /// </summary>
        private void OnCnlDataChanged(ref CnlData cnlData, CnlData prevCnlData, DateTime timestamp, DateTime prevTimestamp)
        {
            if (CnlDataChanged != null)
            {
                CnlDataChangedEventArgs eventArgs = new CnlDataChangedEventArgs(cnlData, prevCnlData, timestamp, prevTimestamp);
                CnlDataChanged(this, eventArgs);

                if (cnlData != eventArgs.CnlData)
                    cnlData = eventArgs.CnlData;
            }
        }

        /// <summary>
        /// Sets the current input channel data.
        /// </summary>
        public void SetCurCnlData(DateTime nowDT, int cnlIndex, CnlData cnlData)
        {
            DateTime prevTimestamp = Timestamps[cnlIndex];
            CnlData prevCnlData = CnlData[cnlIndex];
            OnCnlDataChanged(ref cnlData, prevCnlData, nowDT, prevTimestamp);

            PrevTimestamps[cnlIndex] = prevTimestamp;
            PrevCnlData[cnlIndex] = prevCnlData;

            Timestamps[cnlIndex] = nowDT;
            CnlData[cnlIndex] = cnlData;
        }

        /// <summary>
        /// Gets the actual data of the input channel.
        /// </summary>
        CnlData ICalcContext.GetCnlData(int cnlNum)
        {
            return cnlTags.TryGetValue(cnlNum, out CnlTag cnlTag) ?
                CnlData[cnlTag.Index] :
                Data.Models.CnlData.Empty;
        }

        /// <summary>
        /// Gets the previous data of the input channel, if applicable.
        /// </summary>
        CnlData ICalcContext.GetPrevCnlData(int cnlNum)
        {
            return cnlTags.TryGetValue(cnlNum, out CnlTag cnlTag) ?
                PrevCnlData[cnlTag.Index] :
                Data.Models.CnlData.Empty;
        }

        /// <summary>
        /// Gets the actual timestamp of the input channel.
        /// </summary>
        DateTime ICalcContext.GetCnlTime(int cnlNum)
        {
            return cnlTags.TryGetValue(cnlNum, out CnlTag cnlTag) ?
                Timestamps[cnlTag.Index] :
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
                SetCurCnlData(DateTime.UtcNow, cnlTag.Index, cnlData);
        }


        /// <summary>
        /// Occurs when the input channel data changes.
        /// </summary>
        public event EventHandler<CnlDataChangedEventArgs> CnlDataChanged;
    }
}
