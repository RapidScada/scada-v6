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
 * Summary  : Represents a trend of channel data
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2006
 * Modified : 2020
 */

using System.Collections.Generic;

namespace Scada.Data.Models
{
    /// <summary>
    /// Represents a trend of channel data.
    /// <para>Представляет тренд данных канала.</para>
    /// </summary>
    public class Trend
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Trend(int cnlNum, int capacity)
        {
            CnlNum = cnlNum;
            Points = new List<TrendPoint>(capacity);
        }


        /// <summary>
        /// Gets the channel number of the trend.
        /// </summary>
        public int CnlNum { get; }

        /// <summary>
        /// Gets the trend points ordered by timestamp.
        /// </summary>
        public List<TrendPoint> Points { get; }
    }
}
