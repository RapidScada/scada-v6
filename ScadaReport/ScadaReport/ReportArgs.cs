// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
            StartDT = DateTime.MinValue;
            EndDT = DateTime.MinValue;
            Format = OutputFormat.Default;
            CustomArgs = null;
        }


        /// <summary>
        /// Gets or sets the start date and time of the report period, UTC.
        /// </summary>
        public DateTime StartDT { get; set; }

        /// <summary>
        /// Gets or sets the end date and time of the report period, UTC.
        /// </summary>
        public DateTime EndDT { get; set; }

        /// <summary>
        /// Gets or sets the output format.
        /// </summary>
        public OutputFormat Format { get; set; }

        /// <summary>
        /// Gets or sets the custom report arguments.
        /// </summary>
        public IDictionary<string, string> CustomArgs { get; set; }
    }
}
