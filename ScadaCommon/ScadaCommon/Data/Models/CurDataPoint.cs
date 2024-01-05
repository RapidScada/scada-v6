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
 * Summary  : Represents a current channel data point
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

namespace Scada.Data.Models
{
    /// <summary>
    /// Represents a current channel data point.
    /// <para>Представляет точку текущих данных канала.</para>
    /// </summary>
    public struct CurDataPoint
    {
        /// <summary>
        /// Initializes a new instance of the structure.
        /// </summary>
        public CurDataPoint(int cnlNum)
        {
            CnlNum = cnlNum;
            Val = 0.0;
            Stat = 0;
        }

        /// <summary>
        /// Initializes a new instance of the structure.
        /// </summary>
        public CurDataPoint(int cnlNum, double val, int stat)
        {
            CnlNum = cnlNum;
            Val = val;
            Stat = stat;
        }

        /// <summary>
        /// Initializes a new instance of the structure.
        /// </summary>
        public CurDataPoint(int cnlNum, CnlData cnlData)
        {
            CnlNum = cnlNum;
            Val = cnlData.Val;
            Stat = cnlData.Stat;
        }


        /// <summary>
        /// Gets or sets the channel number.
        /// </summary>
        public int CnlNum { get; set; }

        /// <summary>
        /// Gets or sets the channel value.
        /// </summary>
        public double Val { get; set; }

        /// <summary>
        /// Gets or sets the channel status.
        /// </summary>
        public int Stat { get; set; }
    }
}
