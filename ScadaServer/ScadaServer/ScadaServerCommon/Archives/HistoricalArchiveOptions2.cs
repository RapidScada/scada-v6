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
 * Module   : ScadaServerCommon
 * Summary  : Represents options of a historical data archive
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2022
 */

using Scada.Config;
using System.Globalization;

namespace Scada.Server.Archives
{
    /// <summary>
    /// Represents options of a historical data archive.
    /// <para>Представляет параметры архива исторических данных.</para>
    /// </summary>
    public abstract class HistoricalArchiveOptions2 : ArchiveOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public HistoricalArchiveOptions2(OptionList options)
            : base(options)
        {
            CanWriteOnChange = true;
            IsPeriodic = options.GetValueAsBool("IsPeriodic", true);
            WriteWithPeriod = options.GetValueAsBool("WriteWithPeriod", true);
            WritingPeriod = options.GetValueAsInt("WritingPeriod", 1);
            PeriodUnit = options.GetValueAsEnum("PeriodUnit", TimeUnit.Minute);
            PullToPeriod = options.GetValueAsInt("PullToPeriod", 0);
            WriteOnChange = options.GetValueAsBool("WriteOnChange", false);
            Deadband = options.GetValueAsDouble("Deadband", 0);
            DeadbandUnit = options.GetValueAsEnum("DeadbandUnit", DeadbandUnit.Absolute);
            Retention = options.GetValueAsInt("Retention", 365);
        }


        /// <summary>
        /// Gets or sets a value indicating whether writing data on change is supported by the archive.
        /// </summary>
        protected bool CanWriteOnChange { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the archive contains only periodic data.
        /// </summary>
        public bool IsPeriodic { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to append current data to the archive with a period.
        /// </summary>
        public bool WriteWithPeriod { get; set; }

        /// <summary>
        /// Gets or sets the period of writing data to a file.
        /// </summary>
        public int WritingPeriod { get; set; }

        /// <summary>
        /// Gets or sets the unit of measure for the writing period.
        /// </summary>
        public TimeUnit PeriodUnit { get; set; }

        /// <summary>
        /// Gets or sets the deviation of timestamps from the writing period to update timestamps, in seconds.
        /// </summary>
        public int PullToPeriod { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to append current data to the archive when data changes.
        /// </summary>
        public bool WriteOnChange { get; set; }

        /// <summary>
        /// Gets or sets the deadband of a value to be written on change.
        /// </summary>
        public double Deadband { get; set; }

        /// <summary>
        /// Gets or sets the deadband unit.
        /// </summary>
        public DeadbandUnit DeadbandUnit { get; set; }

        /// <summary>
        /// Gets or sets the duration of time that the archive retains data, days.
        /// </summary>
        public int Retention { get; set; }


        /// <summary>
        /// Adds the options to the list.
        /// </summary>
        public override void AddToOptionList(OptionList options)
        {
            base.AddToOptionList(options);

            if (!ReadOnly)
            {
                options["IsPeriodic"] = IsPeriodic.ToLowerString();
                options["WriteWithPeriod"] = WriteWithPeriod.ToLowerString();
                options["WritingPeriod"] = WritingPeriod.ToString();
                options["PeriodUnit"] = PeriodUnit.ToString();
                options["PullToPeriod"] = PullToPeriod.ToString();
                options["Retention"] = Retention.ToString();

                if (CanWriteOnChange)
                {
                    options["WriteOnChange"] = WriteOnChange.ToLowerString();
                    options["Deadband"] = Deadband.ToString(NumberFormatInfo.InvariantInfo);
                    options["DeadbandUnit"] = DeadbandUnit.ToString();
                }
            }
        }
    }
}
