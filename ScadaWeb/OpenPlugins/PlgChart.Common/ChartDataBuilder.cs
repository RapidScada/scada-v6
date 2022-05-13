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
        private readonly BaseDataSet baseDataSet;         // the configuration database
        private readonly ScadaClient scadaClient;         // interacts with the server
        private readonly ChartDataBuilderOptions options; // the builder options
        private readonly CnlDataFormatter formatter;      // formats channel data

        private Cnl[] cnls;              // the channels of the chart
        private Trend singleTrend;       // the chart data if only one channel is used in the chart
        private TrendBundle trendBundle; // the chart data of many channels


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ChartDataBuilder(BaseDataSet baseDataSet, ScadaClient scadaClient, ChartDataBuilderOptions options)
        {
            this.baseDataSet = baseDataSet ?? throw new ArgumentNullException(nameof(baseDataSet));
            this.scadaClient = scadaClient ?? throw new ArgumentNullException(nameof(scadaClient));
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            options.Validate();
            formatter = new CnlDataFormatter(baseDataSet);

            cnls = Array.Empty<Cnl>();
            singleTrend = null;
            trendBundle = null;
        }


        /// <summary>
        /// Gets the quantity name and unit of the specified channel.
        /// </summary>
        private string GetQuantityName(Cnl cnl)
        {
            string quantityName = cnl.QuantityID == null ? 
                null : baseDataSet.QuantityTable.GetItem(cnl.QuantityID.Value)?.Name;
            string unitName = cnl.UnitID == null ?
                null : baseDataSet.UnitTable.GetItem(cnl.UnitID.Value)?.Name;

            return string.IsNullOrEmpty(quantityName) || string.IsNullOrEmpty(unitName)
                ? quantityName + unitName
                : quantityName + ", " + unitName;
        }

        /// <summary>
        /// Gets the unit name (with leading space) of the specified channel.
        /// </summary>
        private string GetUnitName(Cnl cnl)
        {
            Unit unit = cnl.UnitID == null ? null : baseDataSet.UnitTable.GetItem(cnl.UnitID.Value);
            return unit == null || string.IsNullOrEmpty(unit.Name) ? "" : " " + unit.Name;
        }

        /// <summary>
        /// Gets the single trend points as JavaScript.
        /// </summary>
        private string GetTrendPointsJs(Trend trend, Cnl cnl)
        {
            StringBuilder sbTrendPoints = new();
            sbTrendPoints.Append('[');

            if (trend != null)
            {
                string unitName = GetUnitName(cnl);

                foreach (Data.Models.TrendPoint point in trend.Points)
                {
                    sbTrendPoints
                        .Append(TrendPointToJs(new CnlData(point.Val, point.Stat), cnl, unitName))
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
                Cnl cnl = cnls[trendInd];
                string unitName = GetUnitName(cnl);

                foreach (CnlData cnlData in trendBundle.Trends[trendInd])
                {
                    sbTrendPoints
                        .Append(TrendPointToJs(cnlData, cnl, unitName))
                        .Append(", ");
                }
            }

            sbTrendPoints.Append(']');
            return sbTrendPoints.ToString();
        }

        /// <summary>
        /// Converts the trend point to JavaScript.
        /// </summary>
        private string TrendPointToJs(CnlData cnlData, Cnl cnl, string unitName)
        {
            // HttpUtility.JavaScriptStringEncode() is skipped for performance
            double chartVal = cnlData.Stat > 0 ? cnlData.Val : double.NaN;
            CnlDataFormatted cnlDataFormatted = formatter.FormatCnlData(cnlData, cnl, false);

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
                foreach (Data.Models.TrendPoint point in trend.Points)
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
        /// Fills the channel array.
        /// </summary>
        public void FillCnls()
        {
            int cnlCnt = options.CnlNums.Length;
            cnls = new Cnl[cnlCnt];

            for (int i = 0; i < cnlCnt; i++)
            {
                int cnlNum = options.CnlNums[i];
                cnls[i] = baseDataSet.CnlTable.GetItem(cnlNum) ?? new Cnl { CnlNum = cnlNum };
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
                    singleTrend = scadaClient.GetTrend(options.ArchiveBit, options.TimeRange, options.CnlNums[0]);
                else if (cnlCnt > 1)
                    trendBundle = scadaClient.GetTrends(options.ArchiveBit, options.TimeRange, options.CnlNums);
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

            for (int i = 0, cnlCnt = cnls.Length; i < cnlCnt; i++)
            {
                string trendName = "trend" + i;
                Cnl cnl = cnls[i];
                sbTrends.Append(trendName).Append(", ");

                stringBuilder
                    .Append("var ").Append(trendName).AppendLine(" = new scada.chart.Trend();")
                    .Append(trendName).AppendFormat(".cnlNum = {0};", cnl.CnlNum).AppendLine()
                    .Append(trendName).AppendFormat(".cnlName = '{0}';",
                        HttpUtility.JavaScriptStringEncode(cnl.Name)).AppendLine()
                    .Append(trendName).AppendFormat(".quantityID = {0};", cnl.QuantityID ?? 0).AppendLine()
                    .Append(trendName).AppendFormat(".quantityName = '{0}';",
                        HttpUtility.JavaScriptStringEncode(GetQuantityName(cnl))).AppendLine()
                    .Append(trendName).AppendFormat(".trendPoints = {0};",
                        isSingle ? GetTrendPointsJs(singleTrend, cnl) : GetTrendPointsJs(trendBundle, i))
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
