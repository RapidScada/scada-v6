﻿// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Client;
using Scada.Data.Models;
using System.Globalization;

namespace Scada.Report
{
    /// <summary>
    /// Defines functionality to access the report environment.
    /// <para>Определяет функциональность для доступа к окружению отчёта.</para>
    /// </summary>
    public interface IReportContext
    {
        /// <summary>
        /// Gets the cached configuration database.
        /// </summary>
        ConfigDataset ConfigDatabase { get; }

        /// <summary>
        /// Gets the client that interacts with the Server service.
        /// </summary>
        ScadaClient ScadaClient { get; }

        /// <summary>
        /// Gets the time zone.
        /// </summary>
        TimeZoneInfo TimeZone { get; }

        /// <summary>
        /// Gets the culture.
        /// </summary>
        CultureInfo Culture { get; }

        /// <summary>
        /// Gets the directory of templates.
        /// </summary>
        string TemplateDir { get; }


        /// <summary>
        /// Converts a universal time (UTC) to the time in the report's time zone.
        /// </summary>
        DateTime ConvertTimeFromUtc(DateTime dateTime)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(dateTime, TimeZone);
        }
    }
}
