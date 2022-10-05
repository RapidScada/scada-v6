// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Models;
using Scada.Lang;
using Scada.Report;

namespace Scada.Web.Plugins.PlgMain.Report
{
    /// <summary>
    /// Represents arguments for generating an event report.
    /// <para>Представляет аргументы для генерации отчёта по событиям.</para>
    /// </summary>
    public class EventReportArgs : ReportArgs
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EventReportArgs()
            : base()
        {
            ArchiveCode = "";
            TailMode = false;
            EventCount = 0;
            EventDepth = 0;
            View = null;
            ObjNum = 0;
            ObjNums = null;
            Severities = null;
            MaxPeriod = 0;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EventReportArgs(ReportArgs reportArgs)
            : base()
        {
            ArgumentNullException.ThrowIfNull(reportArgs, nameof(reportArgs));
            StartTime = reportArgs.StartTime;
            EndTime = reportArgs.EndTime;
            Format = reportArgs.Format;
            CustomArgs = reportArgs.CustomArgs;

            ArchiveCode = CustomArgs.GetValueAsString("ArchiveCode");
            TailMode = CustomArgs.GetValueAsBool("TailMode");
            EventCount = CustomArgs.GetValueAsInt("EventCount");
            EventDepth = CustomArgs.GetValueAsInt("EventDepth");
            View = null;
            ObjNum = CustomArgs.GetValueAsInt("ObjNum");
            ObjNums = ScadaUtils.ParseRange(CustomArgs.GetValueAsString("ObjNums"), true, true);
            Severities = ScadaUtils.ParseRange(CustomArgs.GetValueAsString("Severities"), true, true);
            MaxPeriod = 0;
        }


        /// <summary>
        /// Gets the archive code.
        /// </summary>
        public string ArchiveCode { get; init; }

        /// <summary>
        /// Gets a value indicating whether to retrieve only the specified number of recent events.
        /// The start time and end time are ignored.
        /// </summary>
        public bool TailMode { get; init; }

        /// <summary>
        /// Gets the number of events.
        /// </summary>
        public int EventCount { get; init; }

        /// <summary>
        /// Gets the number of days to request events.
        /// </summary>
        public int EventDepth { get; init; }

        /// <summary>
        /// Gets the view to filter events.
        /// </summary>
        public ViewBase View { get; init; }

        /// <summary>
        /// Gets the parent object number to filter events.
        /// </summary>
        public int ObjNum { get; init; }

        /// <summary>
        /// Gets the full list of object numbers to filter events.
        /// </summary>
        public IList<int> ObjNums { get; init; }

        /// <summary>
        /// Gets the severities to filter events.
        /// </summary>
        public IList<int> Severities { get; init; }

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
        }
    }
}
