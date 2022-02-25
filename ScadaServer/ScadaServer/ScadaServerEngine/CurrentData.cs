/*
 * Copyright 2022 Rapid Software LLC
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
using System.Linq;

namespace Scada.Server.Engine
{
    /// <summary>
    /// Represents current channel data.
    /// <para>Представляет текущие данные каналов.</para>
    /// </summary>
    internal class CurrentData : ICurrentData, ICalcContext
    {
        private readonly ICnlDataChangeHandler cnlDataChangeHandler; // handles data changes
        private readonly Dictionary<int, CnlTag> cnlTags;            // the channel tags for archiving
        private CnlData[] cnlDataCopy;                               // the copy of channel data


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CurrentData(ICnlDataChangeHandler cnlDataChangeHandler, Dictionary<int, CnlTag> cnlTags)
        {
            this.cnlDataChangeHandler = cnlDataChangeHandler ?? 
                throw new ArgumentNullException(nameof(cnlDataChangeHandler));
            this.cnlTags = cnlTags ?? 
                throw new ArgumentNullException(nameof(cnlTags));

            cnlDataCopy = null;
            Timestamp = DateTime.MinValue;
            CnlNums = cnlTags.Select(t => t.Value.CnlNum).ToArray();

            int cnlCnt = cnlTags.Count;
            CnlData = new CnlData[cnlCnt];
            PrevCnlData = new CnlData[cnlCnt];
            PrevCnlDataDef = new CnlData[cnlCnt];
            Timestamps = new DateTime[cnlCnt];
            PrevTimestamps = new DateTime[cnlCnt];
        }


        /// <summary>
        /// Gets or sets the current timestamp (UTC).
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets the channel numbers for archiving.
        /// </summary>
        public int[] CnlNums { get; }

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
        /// Sets the current channel data.
        /// </summary>
        public void SetCurCnlData(CnlTag cnlTag, ref CnlData cnlData, DateTime nowDT, bool enableEvents = true)
        {
            int cnlIndex = cnlTag.Index;
            CnlData prevCnlData = CnlData[cnlIndex];
            CnlData prevCnlDataDef = prevCnlData.IsDefined ? prevCnlData : PrevCnlDataDef[cnlIndex];
            cnlDataChangeHandler.HandleCurDataChanged(cnlTag, ref cnlData, prevCnlData, prevCnlDataDef, enableEvents);

            PrevTimestamps[cnlIndex] = Timestamps[cnlIndex];
            PrevCnlData[cnlIndex] = prevCnlData;

            if (prevCnlData.IsDefined)
                PrevCnlDataDef[cnlIndex] = prevCnlData;

            Timestamps[cnlIndex] = nowDT;
            CnlData[cnlIndex] = cnlData;
        }

        /// <summary>
        /// Prepares the instance for a new iteration of data processing.
        /// </summary>
        public void PrepareIteration(DateTime nowDT)
        {
            cnlDataCopy = null;
            Timestamp = nowDT;
        }

        /// <summary>
        /// Gets the index of the specified channel, or -1 if the channel not found.
        /// </summary>
        int ICurrentData.GetCnlIndex(int cnlNum)
        {
            return cnlTags.TryGetValue(cnlNum, out CnlTag cnlTag) ? cnlTag.Index : -1;
        }

        /// <summary>
        /// Creates a copy of the channel data.
        /// </summary>
        CnlData[] ICurrentData.CloneCnlData()
        {
            if (cnlDataCopy == null)
                cnlDataCopy = (CnlData[])CnlData.Clone();
            return cnlDataCopy;
        }

        /// <summary>
        /// Gets the actual channel data.
        /// </summary>
        CnlData ICalcContext.GetCnlData(int cnlNum)
        {
            return cnlTags.TryGetValue(cnlNum, out CnlTag cnlTag) ?
                CnlData[cnlTag.Index] :
                Data.Models.CnlData.Empty;
        }

        /// <summary>
        /// Gets the previous channel data, if applicable.
        /// </summary>
        CnlData ICalcContext.GetPrevCnlData(int cnlNum)
        {
            return cnlTags.TryGetValue(cnlNum, out CnlTag cnlTag) ?
                PrevCnlData[cnlTag.Index] :
                Data.Models.CnlData.Empty;
        }

        /// <summary>
        /// Gets the actual channel timestamp.
        /// </summary>
        DateTime ICalcContext.GetCnlTime(int cnlNum)
        {
            return cnlTags.TryGetValue(cnlNum, out CnlTag cnlTag) ?
                Timestamps[cnlTag.Index] :
                DateTime.MinValue;
        }

        /// <summary>
        /// Gets the previous channel timestamp, if applicable.
        /// </summary>
        DateTime ICalcContext.GetPrevCnlTime(int cnlNum)
        {
            return cnlTags.TryGetValue(cnlNum, out CnlTag cnlTag) ?
                PrevTimestamps[cnlTag.Index] :
                DateTime.MinValue;
        }

        /// <summary>
        /// Sets the channel data.
        /// </summary>
        void ICalcContext.SetCnlData(int cnlNum, CnlData cnlData)
        {
            if (cnlTags.TryGetValue(cnlNum, out CnlTag cnlTag))
                SetCurCnlData(cnlTag, ref cnlData, Timestamp);
        }
    }
}
