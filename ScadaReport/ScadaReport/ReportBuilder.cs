// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Report
{
    /// <summary>
    /// Represents the base class for building Rapid SCADA reports.
    /// <para>Представляет базовый класс для постоения отчётов Rapid SCADA.</para>
    /// </summary>
    public abstract class ReportBuilder
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ReportBuilder(IReportContext reportContext)
        {
            ReportContext = reportContext ?? throw new ArgumentNullException(nameof(reportContext));
            GenerateTime = DateTime.MinValue;
        }


        /// <summary>
        /// Gets the report context.
        /// </summary>
        protected IReportContext ReportContext { get; }

        /// <summary>
        /// Gets the time when a report was last built, UTC.
        /// </summary>
        public DateTime GenerateTime { get; protected set; }


        /// <summary>
        /// Generates a report to the output stream.
        /// </summary>
        public abstract void Generate(ReportArgs args, Stream outStream);
    }
}
