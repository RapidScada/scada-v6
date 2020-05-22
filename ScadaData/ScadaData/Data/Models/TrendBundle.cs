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
 * Module   : ScadaData
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
        /// Represents a list of data points.
        /// <para>Представляет список точек данных.</para>
        /// </summary>
        public class DataPointList: List<CnlData>
        {
        }


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TrendBundle()
            : this(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TrendBundle(int capacity)
        {
            Timestamps = new List<DateTime>(capacity);
            Trends = new List<DataPointList>();
        }


        /// <summary>
        /// Gets the timestamps common for all trends.
        /// </summary>
        public List<DateTime> Timestamps { get; protected set; }

        /// <summary>
        /// Gets the trends having the same number of points.
        /// </summary>
        public List<DataPointList> Trends { get; protected set; }
    }
}
