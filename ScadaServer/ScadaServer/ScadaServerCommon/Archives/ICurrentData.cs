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
 * Module   : ScadaServerCommon
 * Summary  : Defines functionality to access current data of the input channels
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Data.Models;
using System;

namespace Scada.Server.Archives
{
    /// <summary>
    /// Defines functionality to access current data of the input channels.
    /// <para>Определяет функциональность для доступа к текущим данным входных каналов.</para>
    /// </summary>
    public interface ICurrentData
    {
        /// <summary>
        /// Gets the current timestamp.
        /// </summary>
        DateTime Timestamp { get; }

        /// <summary>
        /// Gets the input channel numbers.
        /// </summary>
        int[] CnlNums { get; }

        /// <summary>
        /// Gets the current data of the input channels.
        /// </summary>
        CnlData[] CnlData { get; }

        /// <summary>
        /// Gets the previous data of the input channels.
        /// </summary>
        CnlData[] PrevCnlData { get; }

        /// <summary>
        /// Gets the current timestamps of the input channels.
        /// </summary>
        DateTime[] Timestamps { get; }

        /// <summary>
        /// Gets the previous timestamps of the input channels.
        /// </summary>
        DateTime[] PrevTimestamps { get; }


        /// <summary>
        /// Creates a copy of the input channel data.
        /// </summary>
        /// <remarks>Use this method to avoid blocking the current data instance in case of long processing.</remarks>
        CnlData[] CloneCnlData();
    }
}
