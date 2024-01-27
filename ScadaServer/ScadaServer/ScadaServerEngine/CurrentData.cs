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
 * Module   : ScadaServerEngine
 * Summary  : Represents current channel data
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2022
 */

using Scada.Data.Models;
using Scada.Server.Archives;
using System;
using System.Collections.Generic;

namespace Scada.Server.Engine
{
    /// <summary>
    /// Represents current channel data.
    /// <para>Представляет текущие данные каналов.</para>
    /// </summary>
    internal class CurrentData : ICurrentData, ICalcContext
    {
        /// <summary>
        /// Represents a method that executes when the current data is changing.
        /// </summary>
        public delegate void DataChangingDelegate(CnlTag cnlTag, ref CnlData cnlData, 
            CnlData prevCnlData, CnlData prevCnlDataDef, bool enableEvents);

        private readonly Dictionary<int, CnlTag> cnlTags;          // the channel tags for archiving
        private readonly DataChangingDelegate dataChangingHandler; // handles data changes


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CurrentData(Dictionary<int, CnlTag> cnlTags, DataChangingDelegate dataChangingHandler)
        {
            this.cnlTags = cnlTags ??
                throw new ArgumentNullException(nameof(cnlTags));
            this.dataChangingHandler = dataChangingHandler ??
                throw new ArgumentNullException(nameof(dataChangingHandler));

            Timestamp = DateTime.MinValue;
            int cnlCnt = cnlTags.Count;
            CnlData = new CnlData[cnlCnt];
            PrevCnlData = new CnlData[cnlCnt];
            PrevCnlDataDef = new CnlData[cnlCnt];
            Timestamps = new DateTime[cnlCnt];
            PrevTimestamps = new DateTime[cnlCnt];
        }


        /// <summary>
        /// Gets or sets the timestamp of the processed data (UTC).
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets the current channel data.
        /// </summary>
        public CnlData[] CnlData { get; }

        /// <summary>
        /// Gets the previous channel data.
        /// </summary>
        public CnlData[] PrevCnlData { get; }

        /// <summary>
        /// Gets the previous channel data with a defined status.
        /// </summary>
        public CnlData[] PrevCnlDataDef { get; }

        /// <summary>
        /// Gets the current channel timestamps.
        /// </summary>
        public DateTime[] Timestamps { get; }

        /// <summary>
        /// Gets the previous channel timestamps.
        /// </summary>
        public DateTime[] PrevTimestamps { get; }

        /// <summary>
        /// Gets a value indicating whether the calculated data is current data.
        /// </summary>
        bool ICalcContext.IsCurrent => true;


        /// <summary>
        /// Gets the current data of the specified channel of the certain kind.
        /// </summary>
        public CnlData GetCurrentData(int cnlNum, CurrentDataKind kind)
        {
            if (cnlTags.TryGetValue(cnlNum, out CnlTag cnlTag))
            {
                switch (kind)
                {
                    case CurrentDataKind.Current:
                        return CnlData[cnlTag.Index];

                    case CurrentDataKind.Previous:
                        return PrevCnlData[cnlTag.Index];

                    case CurrentDataKind.PreviousDefined:
                        return PrevCnlDataDef[cnlTag.Index];
                }
            }

            return Data.Models.CnlData.Empty;
        }

        /// <summary>
        /// Sets the current channel data.
        /// </summary>
        public void SetCurrentData(CnlTag cnlTag, ref CnlData cnlData, DateTime timestamp, bool enableEvents = true)
        {
            int cnlIndex = cnlTag.Index;
            CnlData prevCnlData = CnlData[cnlIndex];
            CnlData prevCnlDataDef = prevCnlData.IsDefined ? prevCnlData : PrevCnlDataDef[cnlIndex];
            dataChangingHandler(cnlTag, ref cnlData, prevCnlData, prevCnlDataDef, enableEvents);

            PrevTimestamps[cnlIndex] = Timestamps[cnlIndex];
            PrevCnlData[cnlIndex] = prevCnlData;

            if (prevCnlData.IsDefined)
                PrevCnlDataDef[cnlIndex] = prevCnlData;

            Timestamps[cnlIndex] = timestamp;
            CnlData[cnlIndex] = cnlData;
        }

        /// <summary>
        /// Gets the index of the specified channel, or -1 if the channel not found.
        /// </summary>
        int ICurrentData.GetCnlIndex(int cnlNum)
        {
            return cnlTags.TryGetValue(cnlNum, out CnlTag cnlTag) 
                ? cnlTag.Index 
                : -1;
        }

        /// <summary>
        /// Gets the actual channel data.
        /// </summary>
        CnlData ICalcContext.GetCnlData(int cnlNum)
        {
            return cnlTags.TryGetValue(cnlNum, out CnlTag cnlTag) 
                ? CnlData[cnlTag.Index] 
                : Data.Models.CnlData.Empty;
        }

        /// <summary>
        /// Gets the previous channel data, if applicable.
        /// </summary>
        CnlData ICalcContext.GetPrevCnlData(int cnlNum)
        {
            return cnlTags.TryGetValue(cnlNum, out CnlTag cnlTag) 
                ? PrevCnlData[cnlTag.Index] 
                : Data.Models.CnlData.Empty;
        }

        /// <summary>
        /// Gets the actual channel timestamp.
        /// </summary>
        DateTime ICalcContext.GetCnlTime(int cnlNum)
        {
            return cnlTags.TryGetValue(cnlNum, out CnlTag cnlTag) 
                ? Timestamps[cnlTag.Index] 
                : DateTime.MinValue;
        }

        /// <summary>
        /// Gets the previous channel timestamp, if applicable.
        /// </summary>
        DateTime ICalcContext.GetPrevCnlTime(int cnlNum)
        {
            return cnlTags.TryGetValue(cnlNum, out CnlTag cnlTag) 
                ? PrevTimestamps[cnlTag.Index] 
                : DateTime.MinValue;
        }

        /// <summary>
        /// Sets the channel data.
        /// </summary>
        void ICalcContext.SetCnlData(int cnlNum, CnlData cnlData)
        {
            if (cnlTags.TryGetValue(cnlNum, out CnlTag cnlTag))
                SetCurrentData(cnlTag, ref cnlData, Timestamp);
        }
    }
}
