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
        /// Gets the actual report start date.
        /// </summary>
        public static DateTime GetStartDate(RelativeDate startDate, DateTime currentDate)
        {
            return startDate switch
            {
                RelativeDate.Yesterday => currentDate.AddDays(-1.0).Date,
                _ => currentDate.Date,
            };
        }

        /// <summary>
        /// Normalizes the time range.
        /// </summary>
        /// <remarks>
        /// Makes the startDate a left point of the time range, and makes the period positive.
        /// </remarks>
        public static void NormalizeTimeRange(ref DateTime startDate, ref int period, 
            PeriodUnit unit = PeriodUnit.Day)
        {
            startDate = startDate > DateTime.MinValue ? startDate.Date : DateTime.Today;

            if (unit == PeriodUnit.Month)
            {
                if (period < 0)
                {
                    startDate = startDate.AddMonths(period).Date;
                    period = -period;
                }
            }
            else
            {
                // Examples:
                // If the period is -1, 0 or 1, it means the single day, the startDate.
                // If the period is 2, it means 2 days starting from the startDate.
                // If the period is -2, it means 2 days ending with the startDate and including it.
                if (period <= -2)
                {
                    startDate = startDate.AddDays(period + 1).Date;
                    period = -period;
                }
                else if (period < 1)
                {
                    period = 1;
                }
            }
        }

        /// <summary>
        /// Normalizes the time range.
        /// </summary>
        public static void NormalizeTimeRange(ref DateTime startDate, ref DateTime endDate, ref int period,
            PeriodUnit unit = PeriodUnit.Day)
        {
            bool periodInMonths = unit == PeriodUnit.Month;

            if (startDate > DateTime.MinValue && endDate > DateTime.MinValue)
            {
                if (endDate < startDate)
                    endDate = startDate;
                period = periodInMonths ?
                    ((endDate.Year - startDate.Year) * 12) + endDate.Month - startDate.Month :
                    (int)(endDate - startDate).TotalDays + 1;
            }
            else if (startDate > DateTime.MinValue)
            {
                NormalizeTimeRange(ref startDate, ref period, unit);
                endDate = periodInMonths ?
                    startDate.AddMonths(period) :
                    startDate.AddDays(period - 1);
            }
            else if (endDate > DateTime.MinValue)
            {
                period = Math.Abs(period);
                NormalizeTimeRange(ref endDate, ref period, unit);
                startDate = periodInMonths ?
                    endDate.AddMonths(-period) :
                    endDate.AddDays(-period + 1);
            }
            else
            {
                NormalizeTimeRange(ref startDate, ref period, unit);
                endDate = periodInMonths ?
                    startDate.AddMonths(period) :
                    startDate.AddDays(period - 1);
            }
        }

        /// <summary>
        /// Builds a report file name to save or download.
        /// </summary>
        public static string BuildFileName(string prefix, DateTime generateTime, OutputFormat format)
        {
            return prefix + "_" +
                generateTime.ToLocalTime().ToString("yyyy-MM-dd_HH-mm-ss") + 
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
                OutputFormat.OpenXml => ".xlsx",
                _ => ""
            };
        }
    }
}
