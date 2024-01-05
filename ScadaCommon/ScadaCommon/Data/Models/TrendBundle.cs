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
 * Module   : ScadaCommon
 * Summary  : Represents a bundle of trends having a single timeline
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2020
 */

using System;
using System.Collections.Generic;

namespace Scada.Data.Models
{
    /// <summary>
    /// Represents a bundle of trends having a single timeline.
    /// <para>Представляет связку трендов, имеющую единую шкалу времени.</para>
    /// </summary>
    public class TrendBundle
    {
        /// <summary>
        /// Represents a list of channel data.
        /// <para>Представляет список данных каналов.</para>
        /// </summary>
        public class CnlDataList : List<CnlData>
        {
            /// <summary>
            /// Initializes a new instance of the class.
            /// </summary>
            public CnlDataList(int capacity)
                : base(capacity)
            {
            }
        }


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TrendBundle(int cnlCnt, int trendCapacity)
        {
            CnlNums = new int[cnlCnt];
            Timestamps = new List<DateTime>(trendCapacity);
            Trends = new List<CnlDataList>(cnlCnt);

            for (int i = 0; i < cnlCnt; i++)
            {
                Trends.Add(new CnlDataList(trendCapacity));
            }
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TrendBundle(int[] cnlNums, int trendCapacity)
        {
            CnlNums = cnlNums ?? throw new ArgumentNullException(nameof(cnlNums));
            int cnlCnt = CnlNums.Length;

            Timestamps = new List<DateTime>(trendCapacity);
            Trends = new List<CnlDataList>(cnlCnt);

            for (int i = 0; i < cnlCnt; i++)
            {
                Trends.Add(new CnlDataList(trendCapacity));
            }
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TrendBundle(int[] cnlNums, List<DateTime> timestamps)
        {
            CnlNums = cnlNums ?? throw new ArgumentNullException(nameof(cnlNums));
            Timestamps = timestamps ?? throw new ArgumentNullException(nameof(timestamps));

            int cnlCnt = CnlNums.Length;
            int trendCapacity = timestamps.Count;
            Trends = new List<CnlDataList>(cnlCnt);

            for (int i = 0; i < cnlCnt; i++)
            {
                Trends.Add(new CnlDataList(trendCapacity));
            }
        }


        /// <summary>
        /// Gets the channel numbers.
        /// </summary>
        public int[] CnlNums { get; }

        /// <summary>
        /// Gets the ordered timestamps common for all trends.
        /// </summary>
        public List<DateTime> Timestamps { get; }

        /// <summary>
        /// Gets the trends having the same number of points.
        /// </summary>
        public List<CnlDataList> Trends { get; }
    }
}
