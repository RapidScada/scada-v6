// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Client;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Lang;
using System;
using System.Globalization;
using System.Text;
using System.Web;

namespace Scada.Web.Plugins.PlgChart
{
    /// <summary>
    /// Builds a JavaScript representation of chart properties and data.
    /// <para>Строит JavaScript-представление свойств и данных графика.</para>
    /// </summary>
    public class ChartDataBuilder
    {
        /// <summary>
        /// Represents data builder options.
        /// </summary>
        public class Options
        {
            /// <summary>
            /// Gets or sets the input channel numbers.
            /// </summary>
            public int[] CnlNums { get; set; }

            /// <summary>
            /// Gets or sets the chart time range.
            /// </summary>
            public TimeRange TimeRange { get; set; }

            /// <summary>
            /// Gets or sets the bit number of the chart data archive.
            /// </summary>
            public int ArchiveBit { get; set; }
            
            /// <summary>
            /// Gets or sets the user's time zone.
            /// </summary>
            public TimeZoneInfo TimeZone { get; set; }

            /// <summary>
            /// Validates the options.
            /// </summary>
            public void Validate()
            {
                if (CnlNums == null)
                    throw new ScadaException("CnlNums must not be null.");
                if (TimeZone == null)
                    throw new ScadaException("TimeZone must not be null.");
            }
        }


        private readonly BaseDataSet baseDataSet;    // the configuration database
        private readonly ScadaClient scadaClient;    // interacts with the server
        private readonly Options options;            // the builder options
        private readonly CnlDataFormatter formatter; // formats input channel data

        protected InCnl[] inCnls;          // the input channels of the chart
        protected Trend singleTrend;       // the chart data if only one channel is used in the chart
        protected TrendBundle trendBundle; // the chart data of many channels


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ChartDataBuilder(BaseDataSet baseDataSet, ScadaClient scadaClient, Options options)
        {
            this.baseDataSet = baseDataSet ?? throw new ArgumentNullException(nameof(baseDataSet));
            this.scadaClient = scadaClient ?? throw new ArgumentNullException(nameof(scadaClient));
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            options.Validate();
            formatter = new CnlDataFormatter(baseDataSet);

            inCnls = Array.Empty<InCnl>();
            singleTrend = null;
            trendBundle = null;
        }


        /// <summary>
        /// Gets the quantity name and unit of the input channel.
        /// </summary>
        private string GetQuantityName(InCnl inCnl)
        {
            string quantityName = inCnl.QuantityID == null ? 
                null : baseDataSet.QuantityTable.GetItem(inCnl.QuantityID.Value)?.Name;
            string unitName = inCnl.UnitID == null ?
                null : baseDataSet.UnitTable.GetItem(inCnl.UnitID.Value)?.Name;

            return string.IsNullOrEmpty(quantityName) || string.IsNullOrEmpty(unitName)
                ? quantityName + unitName
                : quantityName + ", " + unitName;
        }

        /// <summary>
        /// Gets the unit name (with leading space) of the input channel.
        /// </summary>
        private string GetUnitName(InCnl inCnl)
        {
            Unit unit = inCnl.UnitID == null ? null : baseDataSet.UnitTable.GetItem(inCnl.UnitID.Value);
            return unit == null || string.IsNullOrEmpty(unit.Name) ? "" : " " + unit.Name;
        }

        /// <summary>
        /// Gets the single trend points as JavaScript.
        /// </summary>
        private string GetTrendPointsJs(Trend trend, InCnl inCnl)
        {
            StringBuilder sbTrendPoints = new();
            sbTrendPoints.Append('[');

            if (trend != null)
            {
                string unitName = GetUnitName(inCnl);

                foreach (TrendPoint point in trend.Points)
                {
                    sbTrendPoints
                        .Append(TrendPointToJs(new CnlData(point.Val, point.Stat), inCnl, unitName))
                        .Append(", ");
                }
            }

            sbTrendPoints.Append(']');
            return sbTrendPoints.ToString();
        }

        /// <summary>
        /// Gets the trend bundle points as JavaScript.
        /// </summary>
        private string GetTrendPointsJs(TrendBundle trendBundle, int trendInd)
        {
            StringBuilder sbTrendPoints = new();
            sbTrendPoints.Append('[');

            if (trendBundle != null)
            {
                InCnl inCnl = inCnls[trendInd];
                string unitName = GetUnitName(inCnl);

                foreach (CnlData cnlData in trendBundle.Trends[trendInd])
                {
                    sbTrendPoints
                        .Append(TrendPointToJs(cnlData, inCnl, unitName))
                        .Append(", ");
                }
            }

            sbTrendPoints.Append(']');
            return sbTrendPoints.ToString();
        }

        /// <summary>
        /// Converts the trend point to JavaScript.
        /// </summary>
        private string TrendPointToJs(CnlData cnlData, InCnl inCnl, string unitName)
        {
            // HttpUtility.JavaScriptStringEncode() is skipped for performance
            double chartVal = cnlData.Stat > 0 ? cnlData.Val : double.NaN;
            CnlDataFormatted cnlDataFormatted = formatter.FormatCnlData(cnlData, inCnl);

            return (new StringBuilder("[")
                .Append(double.IsNaN(chartVal) ? "NaN" : chartVal.ToString(CultureInfo.InvariantCulture))
                .Append(", \"")
                .Append(cnlDataFormatted.DispVal)
                .Append("\", \"")
                .Append(cnlDataFormatted.DispVal + unitName)
                .Append("\", \"")
                .Append(cnlDataFormatted.Colors.Length > 0 ? cnlDataFormatted.Colors[0] : "")
                .Append("\"]")).ToString();
        }

        /// <summary>
        /// Gets the timestamps of the single trend as JavaScript.
        /// </summary>
        private string GetTimestampsJs(Trend trend)
        {
            StringBuilder sbTimestamps = new();
            sbTimestamps.Append('[');

            if (trend != null)
            {
                foreach (TrendPoint point in trend.Points)
                {
                    sbTimestamps.Append(EncodeTimestamp(point.Timestamp)).Append(", ");
                }
            }

            sbTimestamps.Append(']');
            return sbTimestamps.ToString();
        }

        /// <summary>
        /// Gets the timestamps of the trend bundle as JavaScript.
        /// </summary>
        private string GetTimestampsJs(TrendBundle trendBundle)
        {
            StringBuilder sbTimestamps = new();
            sbTimestamps.Append('[');

            if (trendBundle != null)
            {
                foreach (DateTime timestamp in trendBundle.Timestamps)
                {
                    sbTimestamps.Append(EncodeTimestamp(timestamp)).Append(", ");
                }
            }

            sbTimestamps.Append(']');
            return sbTimestamps.ToString();
        }

        /// <summary>
        /// Encodes the timestamp to a string representation of a double number to suit the chart format.
        /// </summary>
        private string EncodeTimestamp(DateTime timestamp)
        {
            if (timestamp.Kind == DateTimeKind.Utc)
                timestamp = TimeZoneInfo.ConvertTimeFromUtc(timestamp, options.TimeZone);

            return timestamp.ToOADate().ToString(CultureInfo.InvariantCulture);
        }


        /// <summary>
        /// Fills the input channel array.
        /// </summary>
        public void FillInCnls()
        {
            int cnlCnt = options.CnlNums.Length;
            inCnls = new InCnl[cnlCnt];

            for (int i = 0; i < cnlCnt; i++)
            {
                int cnlNum = options.CnlNums[i];
                inCnls[i] = baseDataSet.InCnlTable.GetItem(cnlNum) ?? new InCnl() { CnlNum = cnlNum };
            }
        }

        /// <summary>
        /// Fills chart data for the normalized start date.
        /// </summary>
        public void FillData()
        {
            singleTrend = null;
            trendBundle = null;
            int cnlCnt = options.CnlNums.Length;

            try
            {
                if (cnlCnt == 1)
                    singleTrend = scadaClient.GetTrend(options.CnlNums[0], options.TimeRange, options.ArchiveBit);
                else if (cnlCnt > 1)
                    trendBundle = scadaClient.GetTrends(options.CnlNums, options.TimeRange, options.ArchiveBit);
            }
            catch (Exception ex)
            {
                throw new ScadaException(Locale.IsRussian ?
                    "Ошибка при получении одного или нескольких трендов" :
                    "Error getting one or several trends", ex);
            }
        }

        /// <summary>
        /// Converts the chart data to JavaScript.
        /// </summary>
        public void ToJs(StringBuilder stringBuilder)
        {
            // time range
            stringBuilder
                .AppendLine("var timeRange = new scada.chart.TimeRange();")
                .AppendFormat("timeRange.startTime = {0};", EncodeTimestamp(options.TimeRange.StartTime)).AppendLine()
                .AppendFormat("timeRange.endTime = {0};", EncodeTimestamp(options.TimeRange.EndTime)).AppendLine()
                .AppendLine();

            // chart data
            StringBuilder sbTrends = new("[");
            bool isSingle = singleTrend != null;

            for (int i = 0, cnlCnt = inCnls.Length; i < cnlCnt; i++)
            {
                string trendName = "trend" + i;
                InCnl inCnl = inCnls[i];
                sbTrends.Append(trendName).Append(", ");

                stringBuilder
                    .Append("var ").Append(trendName).AppendLine(" = new scada.chart.Trend();")
                    .Append(trendName).AppendFormat(".cnlNum = {0};", inCnl.CnlNum).AppendLine()
                    .Append(trendName).AppendFormat(".cnlName = '{0}';",
                        HttpUtility.JavaScriptStringEncode(inCnl.Name)).AppendLine()
                    .Append(trendName).AppendFormat(".quantityID = {0};", inCnl.QuantityID ?? 0).AppendLine()
                    .Append(trendName).AppendFormat(".quantityName = '{0}';",
                        HttpUtility.JavaScriptStringEncode(GetQuantityName(inCnl))).AppendLine()
                    .Append(trendName).AppendFormat(".trendPoints = {0};",
                        isSingle ? GetTrendPointsJs(singleTrend, inCnl) : GetTrendPointsJs(trendBundle, i))
                    .AppendLine()
                    .AppendLine();
            }

            sbTrends.Append("];");

            stringBuilder
                .AppendLine("var chartData = new scada.chart.ChartData();")
                .Append("chartData.timePoints = ")
                .Append(isSingle ? GetTimestampsJs(singleTrend) : GetTimestampsJs(trendBundle)).AppendLine(";")
                .Append("chartData.trends = ").Append(sbTrends).AppendLine()
                .AppendLine();
        }
    }
}
