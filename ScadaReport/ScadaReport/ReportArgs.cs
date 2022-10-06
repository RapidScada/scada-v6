// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.Report
{
    /// <summary>
    /// Represents report generating arguments.
    /// <para>Представляет аргументы для генерации отчета.</para>
    /// </summary>
    public class ReportArgs
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ReportArgs()
        {
            StartTime = DateTime.MinValue;
            EndTime = DateTime.MinValue;
            Format = OutputFormat.Default;
            CustomArgs = new Dictionary<string, string>();
        }


        /// <summary>
        /// Gets or sets the start date and time of the report period, UTC.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end date and time of the report period, UTC.
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Gets or sets the output format.
        /// </summary>
        public OutputFormat Format { get; set; }

        /// <summary>
        /// Gets the custom report arguments.
        /// </summary>
        public IDictionary<string, string> CustomArgs { get; protected set; }


        /// <summary>
        /// Validates the arguments, raises an exception on failure.
        /// </summary>
        public virtual void Validate()
        {
            if (StartTime > EndTime)
            {
                throw new ScadaException(Locale.IsRussian ?
                    "Некорректный диапазон времени." :
                    "Invalid time range.") { MessageIsPublic = true };
            }
        }
    }
}
