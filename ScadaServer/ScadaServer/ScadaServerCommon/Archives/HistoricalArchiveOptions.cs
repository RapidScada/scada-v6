/*
 * Copyright 2021 Rapid Software LLC
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
 * Summary  : Represents options of a historical data archive
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2021
 */

using Scada.Config;
using System;

namespace Scada.Server.Archives
{
    /// <summary>
    /// Represents options of a historical data archive.
    /// <para>Представляет параметры архива исторических данных.</para>
    /// </summary>
    public abstract class HistoricalArchiveOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public HistoricalArchiveOptions(OptionList options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            WritingMode = options.GetValueAsEnum("WritingMode", WritingMode.AutoWithPeriod);
            WritingPeriod = options.GetValueAsInt("WritingPeriod", 10);
            WritingUnit = options.GetValueAsEnum("WritingUnit", TimeUnit.Minute);
            PullToPeriod = options.GetValueAsInt("PullToPeriod", 0);
            Retention = options.GetValueAsInt("Retention", 365);
            LogEnabled = options.GetValueAsBool("LogEnabled");
        }


        /// <summary>
        /// Gets or sets the writing mode.
        /// </summary>
        public WritingMode WritingMode { get; set; }

        /// <summary>
        /// Gets or sets the period of writing data to a file.
        /// </summary>
        public int WritingPeriod { get; set; }

        /// <summary>
        /// Gets or sets the unit of measure for the writing period.
        /// </summary>
        public TimeUnit WritingUnit { get; set; }

        /// <summary>
        /// Gets or sets the possible deviation of timestamps from the writing period, in seconds.
        /// </summary>
        public int PullToPeriod { get; set; }

        /// <summary>
        /// Gets or sets the duration of time that the archive retains data, days.
        /// </summary>
        public int Retention { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to write the archive log.
        /// </summary>
        public bool LogEnabled { get; set; }


        /// <summary>
        /// Adds the options to the list.
        /// </summary>
        public virtual void AddToOptionList(OptionList options)
        {
            options.Clear();
            options["WritingMode"] = WritingMode.ToString();
            options["WritingPeriod"] = WritingPeriod.ToString();
            options["WritingUnit"] = WritingUnit.ToString();
            options["PullToPeriod"] = PullToPeriod.ToString();
            options["Retention"] = Retention.ToString();
            options["LogEnabled"] = LogEnabled.ToString().ToLowerInvariant();
        }
    }
}
