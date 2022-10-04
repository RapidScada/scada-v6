// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;
using Scada.Report;

namespace Scada.Web.Plugins.PlgMain.Report
{
    /// <summary>
    /// Represents arguments for generating a historical data report.
    /// <para>Представляет аргументы для генерации отчёта по историческим данным.</para>
    /// </summary>
    public class HistDataReportArgs : ReportArgs
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public HistDataReportArgs()
            : base()
        {
            ArchiveCode = "";
            CnlNums = null;
            MaxPeriod = 0;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public HistDataReportArgs(ReportArgs reportArgs)
            : base()
        {
            ArgumentNullException.ThrowIfNull(reportArgs, nameof(reportArgs));
            StartTime = reportArgs.StartTime;
            EndTime = reportArgs.EndTime;
            TimeZone = reportArgs.TimeZone;
            Format = reportArgs.Format;
            CustomArgs = reportArgs.CustomArgs;

            ArchiveCode = CustomArgs.GetValueAsString("ArchiveCode");
            CnlNums = ScadaUtils.ParseRange(CustomArgs.GetValueAsString("CnlNums"), true, true);
            MaxPeriod = 0;
        }


        /// <summary>
        /// Gets the archive code.
        /// </summary>
        public string ArchiveCode { get; init; }

        /// <summary>
        /// Gets the channel numbers.
        /// </summary>
        public IList<int> CnlNums { get; init; }

        /// <summary>
        /// Gets the time maximum report period, in days.
        /// </summary>
        public int MaxPeriod { get; init; }


        /// <summary>
        /// Validates the arguments, raises an exception on failure.
        /// </summary>
        public override void Validate()
        {
            base.Validate();

            if (MaxPeriod > 0 && (EndTime - StartTime).TotalDays > MaxPeriod)
            {
                throw new ScadaException(Locale.IsRussian ?
                    "Превышен период отчёта." :
                    "Report period exceeded.") { MessageIsPublic = true };
            }

            if (CnlNums == null || CnlNums.Count <= 0)
            {
                throw new ScadaException(Locale.IsRussian ?
                    "Номера каналов отсутствуют." :
                    "Channel numbers are missing.") { MessageIsPublic = true };
            }
        }
    }
}
