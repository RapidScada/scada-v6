// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Report
{
    /// <summary>
    /// Represents the base class for building Rapid SCADA reports.
    /// <para>Представляет базовый класс для постоения отчётов Rapid SCADA.</para>
    /// </summary>
    public abstract class ScadaReportBuilder : ReportBuilder
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ScadaReportBuilder(IReportContext reportContext)
        {
            ReportContext = reportContext ?? throw new ArgumentNullException(nameof(reportContext));
        }

        /// <summary>
        /// Gets the report context.
        /// </summary>
        protected IReportContext ReportContext { get; }
    }
}
