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
 * Module   : ModArcBasic
 * Summary  : Implements the historical data archive logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Config;
using Scada.Data.Models;
using Scada.Server.Archives;
using Scada.Server.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Server.Modules.ModArcBasic.Logic
{
    /// <summary>
    /// Implements the historical data archive logic.
    /// <para>Реализует логику архива исторических данных.</para>
    /// </summary>
    internal class BasicHAL : HistoricalArchiveLogic
    {
        /// <summary>
        /// Represents archive options.
        /// </summary>
        private class ArchiveOptions
        {
            /// <summary>
            /// Initializes a new instance of the class.
            /// </summary>
            public ArchiveOptions(CustomOptions options)
            {
                IsCopy = options.GetValueAsBool("IsCopy");
                WritingPeriod = options.GetValueAsInt("WritingPeriod", 1);
                WritingUnit = options.GetValueAsEnum("WritingUnit", TimeUnit.Minute);
                WritingMode = options.GetValueAsEnum("WritingMode", WritingMode.Auto);
                StoragePeriod = options.GetValueAsInt("StoragePeriod", 365);
            }

            /// <summary>
            /// Gets or sets a value indicating whether the archive stores a copy of the data.
            /// </summary>
            public bool IsCopy { get; set; }
            /// <summary>
            /// Gets the period of writing data to a file.
            /// </summary>
            public int WritingPeriod { get; set; }
            /// <summary>
            /// Gets the unit of measure for the writing period.
            /// </summary>
            public TimeUnit WritingUnit { get; set; }
            /// <summary>
            /// Gets the writing mode.
            /// </summary>
            public WritingMode WritingMode { get; set; }
            /// <summary>
            /// Gets the data storage period.
            /// </summary>
            public int StoragePeriod { get; set; }
        }


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public BasicHAL(ArchiveConfig archiveConfig, int[] cnlNums, PathOptions pathOptions)
            : base(archiveConfig, cnlNums)
        {
        }


        /// <summary>
        /// Gets the input channel data.
        /// </summary>
        public override CnlData GetCnlData(int cnlNum, DateTime timestamp)
        {
            return CnlData.Empty;
        }

        /// <summary>
        /// Gets the trends of the specified input channels.
        /// </summary>
        public override TrendBundle GetTrends(int[] cnlNums, DateTime startTime, DateTime endTime)
        {
            return null;
        }

        /// <summary>
        /// Gets the trend of the specified input channel.
        /// </summary>
        public override Trend GetTrend(int cnlNum, DateTime startTime, DateTime endTime)
        {
            return null;
        }

        /// <summary>
        /// Gets the available timestamps.
        /// </summary>
        public override List<DateTime> GetTimestamps(DateTime startTime, DateTime endTime)
        {
            return null;
        }

        /// <summary>
        /// Gets the slice of the specified input channels at the timestamp.
        /// </summary>
        public override Slice GetSlice(int[] cnlNums, DateTime timestamp)
        {
            return null;
        }

        /// <summary>
        /// Processes new data.
        /// </summary>
        public override void ProcessData(ICurrentData curData)
        {

        }

        /// <summary>
        /// Accepts or rejects data with the specified timestamp.
        /// </summary>
        public override bool AcceptData(DateTime timestamp)
        {
            return false;
        }

        /// <summary>
        /// Maintains performance when data is written one at a time.
        /// </summary>
        public override void BeginUpdate()
        {
        }

        /// <summary>
        /// Completes the update operation.
        /// </summary>
        public override void EndUpdate()
        {
        }

        /// <summary>
        /// Writes the input channel data.
        /// </summary>
        public override void WriteCnlData(int cnlNum, DateTime timestamp, CnlData cnlData)
        {

        }
    }
}
