// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Client;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Lang;
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
        /// Gets the external database connection string.
        /// </summary>
        string ConnectionString { get; }

        /// <summary>
        /// Gets the culture.
        /// </summary>
        CultureInfo Culture { get; }

        /// <summary>
        /// Gets the time zone.
        /// </summary>
        TimeZoneInfo TimeZone { get; }

        /// <summary>
        /// Gets the username who generated the report.
        /// </summary>
        public string Username { get; }

        /// <summary>
        /// Gets the directory of the application configuration.
        /// </summary>
        string ConfigDir { get; init; }

        /// <summary>
        /// Gets the directory of templates.
        /// </summary>
        string TemplateDir { get; }

        /// <summary>
        /// Get the path to CSS files for HTML reports.
        /// </summary>
        string CssPath { get; }


        /// <summary>
        /// Converts a universal time (UTC) to the time in the report's time zone.
        /// </summary>
        DateTime ConvertTimeFromUtc(DateTime dateTime)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(dateTime, TimeZone);
        }

        /// <summary>
        /// Converts a date and time (UTC) to a string representation in the report's time zone and culture.
        /// </summary>
        string FormatDateTime(DateTime dateTime)
        {
            return ConvertTimeFromUtc(dateTime).ToLocalizedString(Culture);
        }

        /// <summary>
        /// Converts a date and time (UTC) to a string representation that includes the UTC offset.
        /// </summary>
        string FormatDateTimeWithOffset(DateTime dateTime)
        {
            TimeSpan offset = TimeZone.GetUtcOffset(dateTime);
            string offsetStr = (offset >= TimeSpan.Zero ? "+" : "") + offset.ToString(@"hh\:mm");
            return ConvertTimeFromUtc(dateTime).ToLocalizedString(Culture) + $" (UTC{offsetStr})";
        }

        /// <summary>
        /// Finds an archive entity by the first non-empty archive code.
        /// Raises an exception if not found.
        /// </summary>
        Archive FindArchive(params string[] archiveCodes)
        {
            string archiveCode = ScadaUtils.FirstNonEmpty(archiveCodes);
            return ConfigDatabase.ArchiveTable.SelectFirst(new TableFilter("Code", archiveCode)) ??
                throw new ScadaException(Locale.IsRussian ?
                    "Архив не найдён в базе конфигурации." :
                    "Archive not found in the configuration database.")
                { MessageIsPublic = true };
        }

        /// <summary>
        /// Finds channel entities by the specified channel numbers.
        /// If some channel does not exists, adds an empty channel.
        /// </summary>
        List<Cnl> FindChannels(IList<int> cnlNums)
        {
            if (cnlNums == null)
            {
                return new List<Cnl>();
            }
            else
            {
                List<Cnl> cnls = new(cnlNums.Count);

                foreach (int cnlNum in cnlNums)
                {
                    cnls.Add(
                        ConfigDatabase.CnlTable.GetItem(cnlNum) ??
                        new Cnl { CnlNum = cnlNum, Name = "" });
                }

                return cnls;
            }
        }
    }
}
