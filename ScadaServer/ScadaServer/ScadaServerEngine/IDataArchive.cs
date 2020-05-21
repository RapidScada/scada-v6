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
 * Summary  : Defines functionality of a data archive
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
    /// Defines functionality of a data archive.
    /// <para>Определяет функциональность архива данных.</para>
    /// </summary>
    public interface IDataArchive
    {
        /// <summary>
        /// Gets the required cleanup period for outdated data.
        /// </summary>
        TimeSpan CleanupPeriod { get; }


        /// <summary>
        /// Gets the trend of the specified input channel.
        /// </summary>
        Trend GetTrend(int cnlNum, DateTime startTime, DateTime endTime);

        /// <summary>
        /// Gets the trends of the specified input channels.
        /// </summary>
        TrendBundle GetTrends(IList<int> cnlNums, DateTime startTime, DateTime endTime);

        /// <summary>
        /// Gets the slice of the specified input channels at the timestamp.
        /// </summary>
        object GetSlice(IList<int> cnlNums, DateTime timestamp);

        /// <summary>
        /// Writes the slice of data having the single timestamp.
        /// </summary>
        void WriteSlice(DateTime timestamp, object slice);

        /// <summary>
        /// Deletes the outdates data from the archive.
        /// </summary>
        void DeleteOutdatedData();
    }
}
