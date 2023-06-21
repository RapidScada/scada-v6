// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Report
{
    /// <summary>
    /// The class contains utility methods for reports.
    /// <para>Класс, содержащий вспомогательные методы для отчётов.</para>
    /// </summary>
    public static class ReportUtils
    {
        /// <summary>
        /// Gets the report start time as UTC.
        /// </summary>
        public static DateTime GetUtcStartTime(DateTime utcNow, TimeZoneInfo timeZone, PeriodUnit unit)
        {
            ArgumentNullException.ThrowIfNull(timeZone, nameof(timeZone));
            DateTime localStartTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, timeZone);

            localStartTime = unit == PeriodUnit.Month
                ? new DateTime(localStartTime.Year, localStartTime.Month, 1, 0, 0, 0, DateTimeKind.Local)
                : localStartTime.Date;

            return TimeZoneInfo.ConvertTimeToUtc(localStartTime, timeZone);
        }

        /// <summary>
        /// Gets the report end time.
        /// </summary>
        public static DateTime GetEndTime(DateTime startTime, int period, PeriodUnit unit)
        {
            NormalizeTimeRange(ref startTime, ref period, unit);
            return AddPeriod(startTime, period, unit);
        }

        /// <summary>
        /// Normalizes the report time range.
        /// </summary>
        /// <remarks>
        /// Makes startTime the left point of the time range, and makes the period non-negative.
        /// </remarks>
        public static void NormalizeTimeRange(ref DateTime startTime, ref int period, PeriodUnit unit)
        {
            if (startTime == DateTime.MinValue)
                throw new ArgumentException("Start time is not specified.", nameof(startTime));

            if (period < 0)
            {
                startTime = AddPeriod(startTime, period, unit);
                period = -period;
            }
        }

        /// <summary>
        /// Normalizes the report time range.
        /// </summary>
        /// <remarks>
        /// Makes startTime less than or equal to endTime.
        /// </remarks>
        public static void NormalizeTimeRange(ref DateTime startTime, ref DateTime endTime, int period, 
            PeriodUnit unit)
        {
            if (startTime > DateTime.MinValue && endTime > DateTime.MinValue)
            {
                if (startTime > endTime)
                    (startTime, endTime) = (endTime, startTime); // swap values
            }
            else if (startTime > DateTime.MinValue)
            {
                NormalizeTimeRange(ref startTime, ref period, unit);
                endTime = AddPeriod(startTime, period, unit);
            }
            else if (endTime > DateTime.MinValue)
            {
                startTime = AddPeriod(endTime, -Math.Abs(period), unit);
            }
            else
            {
                throw new ArgumentException("Neither start time nor end time is not specified.");
            }
        }

        /// <summary>
        /// Adds the specified period to the date and time value.
        /// </summary>
        public static DateTime AddPeriod(DateTime dateTime, int period, PeriodUnit unit)
        {
            return unit == PeriodUnit.Month
                ? dateTime.AddMonths(period)
                : dateTime.AddDays(period);
        }

        /// <summary>
        /// Builds a report file name to save or download.
        /// </summary>
        public static string BuildFileName(string prefix, DateTime generateTime, OutputFormat format)
        {
            return prefix + "_" +
                generateTime.ToString("yyyy-MM-dd_HH-mm-ss") + 
                format.GetExtension();
        }

        /// <summary>
        /// Gets the file extension, including the period, according to the output format.
        /// </summary>
        public static string GetExtension(this OutputFormat format)
        {
            return format switch
            {
                OutputFormat.Pdf => ".pdf",
                OutputFormat.Xml2003 => ".xml",
                OutputFormat.Xlsx => ".xlsx",
                OutputFormat.Html => ".html",
                _ => ""
            };
        }
    }
}
