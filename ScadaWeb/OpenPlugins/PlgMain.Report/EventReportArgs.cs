// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Models;
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
            TailMode = false;
            EventCount = 0;
            EventDepth = 0;
            View = null;
            ObjNums = null;
            Severities = null;
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
            MaxPeriod = reportArgs.MaxPeriod;
            ArchiveCode = reportArgs.ArchiveCode;
            Format = reportArgs.Format;
            CustomArgs = reportArgs.CustomArgs;

            TailMode = CustomArgs.GetValueAsBool("TailMode");
            EventCount = CustomArgs.GetValueAsInt("EventCount");
            EventDepth = CustomArgs.GetValueAsInt("EventDepth");
            View = null;
            string objNumsStr = CustomArgs.GetValueAsString("ObjNums");
            string severitiesStr = CustomArgs.GetValueAsString("Severities");
            ObjNums = string.IsNullOrEmpty(objNumsStr) ? null : ScadaUtils.ParseRange(objNumsStr, true, true);
            Severities = string.IsNullOrEmpty(severitiesStr) ? null : ScadaUtils.ParseRange(severitiesStr, true, true);
        }


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
        /// Gets a value indicating whether to filter events by view.
        /// </summary>
        public bool FilterByView => View != null;

        /// <summary>
        /// Gets the full list of object numbers to filter events.
        /// </summary>
        public IList<int> ObjNums { get; init; }

        /// <summary>
        /// Gets a value indicating whether to filter events by object.
        /// </summary>
        public bool FilterByObj => ObjNums != null;

        /// <summary>
        /// Gets the severities to filter events.
        /// </summary>
        public IList<int> Severities { get; init; }

        /// <summary>
        /// Gets a value indicating whether to filter events by severity.
        /// </summary>
        public bool FilterBySeverity => Severities != null;
    }
}
