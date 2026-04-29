// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Client;
using Scada.Data.Models;
using System.Globalization;

namespace Scada.Report
{
    /// <summary>
    /// Represents a report environment.
    /// <para>Представляет окружение отчёта.</para>
    /// </summary>
    public class ReportContext : IReportContext
    {
        /// <summary>
        /// Gets the cached configuration database.
        /// </summary>
        public ConfigDataset ConfigDatabase { get; init; }

        /// <summary>
        /// Gets the client that interacts with the Server service.
        /// </summary>
        public ScadaClient ScadaClient { get; init; }

        /// <summary>
        /// Gets the external database connection string.
        /// </summary>
        public string ConnectionString { get; init; }

        /// <summary>
        /// Gets the culture.
        /// </summary>
        public CultureInfo Culture { get; init; }

        /// <summary>
        /// Gets the time zone.
        /// </summary>
        public TimeZoneInfo TimeZone { get; init; }

        /// <summary>
        /// Gets the username who generated the report.
        /// </summary>
        public string Username { get; init; }

        /// <summary>
        /// Gets the directory of the application configuration.
        /// </summary>
        public string ConfigDir { get; init; }

        /// <summary>
        /// Gets the directory of templates.
        /// </summary>
        public string TemplateDir { get; init; }

        /// <summary>
        /// Get the path to CSS files for HTML reports.
        /// </summary>
        public string CssPath { get; init; }
    }
}
