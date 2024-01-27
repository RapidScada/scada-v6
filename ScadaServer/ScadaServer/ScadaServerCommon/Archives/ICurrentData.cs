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
 * Module   : ScadaServerCommon
 * Summary  : Defines functionality to access current channel data
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2022
 */

using Scada.Data.Models;
using System;

namespace Scada.Server.Archives
{
    /// <summary>
    /// Defines functionality to access current channel data.
    /// <para>Определяет функциональность для доступа к текущим данным каналов.</para>
    /// </summary>
    public interface ICurrentData
    {
        /// <summary>
        /// Gets the timestamp of the processed data (UTC).
        /// </summary>
        DateTime Timestamp { get; }

        /// <summary>
        /// Gets the current channel data.
        /// </summary>
        CnlData[] CnlData { get; }

        /// <summary>
        /// Gets the previous channel data.
        /// </summary>
        CnlData[] PrevCnlData { get; }

        /// <summary>
        /// Gets the current channel timestamps.
        /// </summary>
        DateTime[] Timestamps { get; }

        /// <summary>
        /// Gets the previous channel timestamps.
        /// </summary>
        DateTime[] PrevTimestamps { get; }


        /// <summary>
        /// Gets the index of the specified channel, or -1 if the channel not found.
        /// </summary>
        int GetCnlIndex(int cnlNum);
    }
}
