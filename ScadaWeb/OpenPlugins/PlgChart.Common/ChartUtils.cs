// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Const;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Data.Tables;
using System;

namespace Scada.Web.Plugins.PlgChart
{
    /// <summary>
    /// The class provides helper methods for charts.
    /// <para>Класс, предоставляющий вспомогательные методы для графиков.</para>
    /// </summary>
    public static class ChartUtils
    {
        /// <summary>
        /// Formats local date and time to use in JavaScript.
        /// </summary>
        public const string LocalTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
        /// <summary>
        /// Formats local date to use in JavaScript.
        /// </summary>
        public const string LocalDateFormat = "yyyy'-'MM'-'dd";


        /// <summary>
        /// Gets the chart start date as UTC.
        /// </summary>
        public static DateTime GetUtcStartDate(DateTime startDate, TimeZoneInfo timeZone)
        {
            if (startDate == DateTime.MinValue)
                startDate = DateTime.UtcNow;

            if (startDate.Kind == DateTimeKind.Utc)
                startDate = TimeZoneInfo.ConvertTimeFromUtc(startDate, timeZone);

            startDate = startDate.Date;
            return TimeZoneInfo.ConvertTimeToUtc(startDate, timeZone);
        }

        /// <summary>
        /// Gets the chart time range.
        /// </summary>
        public static TimeRange GetTimeRange(DateTime startDate, int periodInDays, bool endInclusive)
        {
            return periodInDays switch
            {
                > 0     => new TimeRange(startDate, startDate.AddDays(periodInDays), endInclusive),
                0 or -1 => new TimeRange(startDate, startDate.AddDays(1), endInclusive),
                _       => new TimeRange(startDate.AddDays(periodInDays + 1), startDate.AddDays(1), endInclusive)
            };
        }

        /// <summary>
        /// Converts the specified value to a local time string.
        /// </summary>
        public static string ToLocalTimeString(this DateTime dateTime, TimeZoneInfo timeZone)
        {
            if (dateTime.Kind == DateTimeKind.Utc)
                dateTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime, timeZone);

            return dateTime.ToString(LocalTimeFormat);
        }

        /// <summary>
        /// Converts the specified value to a local date string.
        /// </summary>
        public static string ToLocalDateString(this DateTime dateTime, TimeZoneInfo timeZone)
        {
            if (dateTime.Kind == DateTimeKind.Utc)
                dateTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime, timeZone);

            return dateTime.ToString(LocalDateFormat);
        }

        /// <summary>
        /// Gets the chart archive bit from the configuration database.
        /// </summary>
        public static int FindArchiveBit(BaseDataSet configBase, string archiveCode)
        {
            if (string.IsNullOrEmpty(archiveCode))
                return ArchiveBit.Minute;
            else if (configBase.ArchiveTable.SelectFirst(new TableFilter("Code", archiveCode)) is Archive archive)
                return archive.Bit;
            else
                return ArchiveBit.Unknown;
        }
    }
}
