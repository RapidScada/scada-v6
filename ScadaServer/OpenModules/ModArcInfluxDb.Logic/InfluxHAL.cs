// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Core.Flux.Domain;
using InfluxDB.Client.Writes;
using NodaTime;
using Scada.Data.Models;
using Scada.Lang;
using Scada.Log;
using Scada.Server.Archives;
using Scada.Server.Config;
using Scada.Server.Lang;
using Scada.Server.Modules.ModArcInfluxDb.Config;
using System.Diagnostics;
using System.Text;

namespace Scada.Server.Modules.ModArcInfluxDb.Logic
{
    /// <summary>
    /// Implements the historical data archive logic.
    /// <para>Реализует логику архива исторических данных.</para>
    /// </summary>
    internal class InfluxHAL : HistoricalArchiveLogic
    {
        /// <summary>
        /// The channel number tag name.
        /// </summary>
        private const string CnlNumTag = "cnl_num";
        /// <summary>
        /// The channel value field name.
        /// </summary>
        private const string ValField = "val";
        /// <summary>
        /// The channel status field name.
        /// </summary>
        private const string StatField = "stat";
        /// <summary>
        /// The name of the data availability flag field.
        /// </summary>
        private const string AvailField = "avail";
        /// <summary>
        /// The date and time format string, for example 2020-01-01T00:00:00.000Z
        /// </summary>
        private const string TimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffK";

        private readonly ModuleConfig moduleConfig; // the module configuration
        private readonly InfluxHAO options;         // the archive options
        private readonly ILog appLog;               // the application log
        private readonly ILog arcLog;               // the archive log
        private readonly Stopwatch stopwatch;       // measures the time of operations
        private readonly int writingPeriod;         // the writing period in seconds
        private readonly CnlDataEqualsDelegate cnlDataEqualsFunc; // the function for comparing channel data

        private ConnectionOptions connOptions;      // the database connection options
        private InfluxDBClient client;              // the InfluxDB client
        private WriteApi writeApi;                  // writes to the database
        private QueryApi queryApi;                  // reads from the database
        private DateTime nextWriteTime;             // the next time to write data to the archive
        private int[] cnlIndexes;                   // the channel mapping indexes
        private CnlData[] prevCnlData;              // the previous channel data
        private DateTime updateTime;                // the timestamp of the current update operation
        private Dictionary<int, CnlData> updatedCnlData; // holds recently updated channel data


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public InfluxHAL(IArchiveContext archiveContext, ArchiveConfig archiveConfig, int[] cnlNums, 
            ModuleConfig moduleConfig) : base(archiveContext, archiveConfig, cnlNums)
        {
            this.moduleConfig = moduleConfig ?? throw new ArgumentNullException(nameof(moduleConfig));
            options = new InfluxHAO(archiveConfig.CustomOptions);
            appLog = archiveContext.Log;
            arcLog = options.LogEnabled ? CreateLog(ModuleUtils.ModuleCode) : null;
            stopwatch = new Stopwatch();
            writingPeriod = GetPeriodInSec(options.WritingPeriod, options.PeriodUnit);
            cnlDataEqualsFunc = SelectCnlDataEquals();

            connOptions = null;
            client = null;
            writeApi = null;
            queryApi = null;
            nextWriteTime = DateTime.MinValue;
            cnlIndexes = null;
            prevCnlData = null;
            updateTime = DateTime.MinValue;
            updatedCnlData = null;
        }


        /// <summary>
        /// Gets the archive options.
        /// </summary>
        protected override HistoricalArchiveOptions2 ArchiveOptions => options;


        /// <summary>
        /// Writes a data point to the database.
        /// </summary>
        private void WritePoint(DateTime timestamp, int cnlNum, CnlData cnlData)
        {
            // for better performance, use the coarsest precision possible for timestamps
            PointData point = PointData
                .Measurement(Code)
                .Timestamp(timestamp, WritePrecision.Ms)
                .Tag(CnlNumTag, cnlNum.ToString())
                .Field(ValField, cnlData.Val)
                .Field(StatField, cnlData.Stat);

            writeApi.WritePoint(point, connOptions.Bucket, connOptions.Org);
        }

        /// <summary>
        /// Writes a data availability flag to the database.
        /// </summary>
        private void WriteFlag(DateTime timestamp)
        {
            PointData point = PointData
                .Measurement(Code)
                .Field(AvailField, true)
                .Timestamp(timestamp, WritePrecision.Ms);

            writeApi.WritePoint(point, connOptions.Bucket, connOptions.Org);
        }

        /// <summary>
        /// Logs information about a writing event.
        /// </summary>
        private void WriteApi_EventHandler(object sender, EventArgs e)
        {
            if (e is WriteSuccessEvent)
            {
                arcLog?.WriteAction(Locale.IsRussian ?
                    "Успешный ответ от сервера базы данных" :
                    "Successful response from the database server");
            }
            else
            {
                Exception ex = null;

                if (e is WriteErrorEvent e1)
                    ex = e1.Exception;
                else if (e is WriteErrorEvent e2)
                    ex = e2.Exception;

                if (ex != null)
                {
                    string msg = Locale.IsRussian ?
                        "Ошибка при отправке данных в архив" :
                        "Error sending data to the archive";

                    appLog.WriteError(ex, ServerPhrases.ArchiveMessage, Code, msg);
                    arcLog?.WriteError(ex, msg);
                }
            }
        }

        /// <summary>
        /// Gets the InfluxDB query to fetch the data of the specified channel.
        /// </summary>
        private string GetCnlDataFlux(DateTime startTime, DateTime endTime, int cnlNum)
        {
            string startTimeStr = startTime.ToString(TimeFormat);
            string endTimeStr = endTime.AddSeconds(1.0).ToString(TimeFormat);

            return
                $"from(bucket: \"{connOptions.Bucket}\")" +
                $"|> range(start: {startTimeStr}, stop: {endTimeStr})" +
                $"|> filter(fn: (r) => " +
                $"  r[\"_measurement\"] == \"{Code}\" and " +
                $"  r[\"{CnlNumTag}\"] == \"{cnlNum}\" and" +
                $"  (r[\"_field\"] == \"{ValField}\" or r[\"_field\"] == \"{StatField}\"))" +
                "|> pivot(" +
                "  rowKey:[\"_time\"]," +
                "  columnKey: [\"_field\"]," +
                "  valueColumn: \"_value\"" +
                ")";
        }

        /// <summary>
        /// Gets the InfluxDB query to fetch the data of the specified channels.
        /// </summary>
        private string GetCnlDataFlux(DateTime startTime, DateTime endTime, int[] cnlNums)
        {
            string startTimeStr = startTime.ToString(TimeFormat);
            string endTimeStr = endTime.AddSeconds(1.0).ToString(TimeFormat);
            StringBuilder sbCnlNums = new();

            for (int i = 0, cnt = cnlNums.Length; i < cnt; i++)
            {
                if (i > 0)
                    sbCnlNums.Append(" or ");
                sbCnlNums.Append($"r[\"{CnlNumTag}\"] == \"").Append(cnlNums[i]).Append('"');
            }

            return
                $"from(bucket: \"{connOptions.Bucket}\")" +
                $"|> range(start: {startTimeStr}, stop: {endTimeStr})" +
                $"|> filter(fn: (r) => " +
                $"  r[\"_measurement\"] == \"{Code}\" and " +
                $"  ({sbCnlNums}) and" +
                $"  (r[\"_field\"] == \"{ValField}\" or r[\"_field\"] == \"{StatField}\"))" +
                "|> pivot(" +
                "  rowKey:[\"_time\"]," +
                "  columnKey: [\"_field\"]," +
                "  valueColumn: \"_value\"" +
                ")";
        }

        /// <summary>
        /// Gets the InfluxDB query to fetch the available timestamps.
        /// </summary>
        private string GetFlagFlux(DateTime startTime, DateTime endTime)
        {
            string startTimeStr = startTime.ToString(TimeFormat);
            string endTimeStr = endTime.AddSeconds(1.0).ToString(TimeFormat);

            return
                $"from(bucket: \"{connOptions.Bucket}\")" +
                $"|> range(start: {startTimeStr}, stop: {endTimeStr})" +
                $"|> filter(fn: (r) => " +
                $"  r[\"_measurement\"] == \"{Code}\" and " +
                $"  r[\"_field\"] == \"{AvailField}\")";
        }

        /// <summary>
        /// Gets a trend bundle containing the trend of the first channel.
        /// </summary>
        private TrendBundle GetFirstTrend(TimeRange timeRange, int[] cnlNums)
        {
            stopwatch.Restart();
            TrendBundle trendBundle;
            string flux = GetCnlDataFlux(timeRange.StartTime, timeRange.EndTime, cnlNums[0]);
            List<FluxTable> tables = queryApi.QueryAsync(flux, connOptions.Org).Result;

            if (tables.Count > 0)
            {
                List<FluxRecord> records = tables[0].Records;
                trendBundle = new TrendBundle(cnlNums, records.Count);
                TrendBundle.CnlDataList trend = trendBundle.Trends[0];

                ProcessRecords(records, timeRange, (timestamp, record) =>
                {
                    trendBundle.Timestamps.Add(timestamp);
                    trend.Add(new CnlData(
                        Convert.ToDouble(record.GetValueByKey(ValField)),
                        Convert.ToInt32(record.GetValueByKey(StatField))));
                });
            }
            else
            {
                trendBundle = new TrendBundle(cnlNums, 0);
            }

            stopwatch.Stop();
            arcLog?.WriteAction(ServerPhrases.ReadingTrendCompleted,
                trendBundle.Timestamps.Count, stopwatch.ElapsedMilliseconds);
            return trendBundle;
        }

        /// <summary>
        /// Gets channel data from the first record of the table if it has the specified timestamp.
        /// </summary>
        private static bool GetCnlData(FluxTable table, long timeMs, out int cnlNum, out CnlData cnlData)
        {
            if (table.Records.Count > 0 &&
                table.Records[0] is FluxRecord record &&
                record.GetTime() is Instant instant &&
                instant.ToUnixTimeMilliseconds() == timeMs)
            {
                cnlNum = Convert.ToInt32(record.GetValueByKey(CnlNumTag));
                cnlData = new CnlData(
                    Convert.ToDouble(record.GetValueByKey(ValField)),
                    Convert.ToInt32(record.GetValueByKey(StatField)));
                return true;
            }
            else
            {
                cnlNum = 0;
                cnlData = CnlData.Empty;
                return false;
            }
        }

        /// <summary>
        /// Processes the records.
        /// </summary>
        private static void ProcessRecords(List<FluxRecord> records, TimeRange timeRange, 
            Action<DateTime, FluxRecord> processFunc)
        {
            // by default records are sorted by time (except using groups)
            DateTime startTime = timeRange.StartTime;
            DateTime endTime = timeRange.EndTime;

            foreach (FluxRecord record in records)
            {
                if (record.GetTime() is Instant instant)
                {
                    DateTime timestamp = instant.ToDateTimeUtc();

                    // skip points having timestamp less than start
                    if (timestamp < startTime)
                        continue;

                    if (timestamp < endTime || timestamp == endTime && timeRange.EndInclusive)
                    {
                        // process the record
                        processFunc(timestamp, record);
                    }
                    else
                    {
                        // stop iterating if end is reached
                        break;
                    }
                }
            }
        }


        /// <summary>
        /// Makes the archive ready for operating.
        /// </summary>
        public override void MakeReady()
        {
            if (!moduleConfig.Connections.TryGetValue(options.Connection, out connOptions))
                throw new ScadaException(CommonPhrases.ConnectionNotFound, options.Connection);

            client = string.IsNullOrEmpty(connOptions.Token) ?
                InfluxDBClientFactory.Create(connOptions.Url, connOptions.Username, connOptions.Password.ToCharArray()) :
                InfluxDBClientFactory.Create(connOptions.Url, connOptions.Token.ToCharArray());
            client.EnableGzip();
            writeApi = client.GetWriteApi();
            writeApi.EventHandler += WriteApi_EventHandler;
            queryApi = client.GetQueryApi();

            if (!options.ReadOnly)
            {
                if (options.WriteWithPeriod)
                    nextWriteTime = GetNextWriteTime(DateTime.UtcNow, writingPeriod);

                if (options.WriteOnChange)
                    appLog.WriteWarning(ServerPhrases.ArchiveMessage, Code, ServerPhrases.WritingOnChangeIsSlow);
            }
        }

        /// <summary>
        /// Closes the archive.
        /// </summary>
        public override void Close()
        {
            writeApi?.Dispose();
            client?.Dispose();
        }

        /// <summary>
        /// Gets the trends of the specified channels.
        /// </summary>
        public override TrendBundle GetTrends(TimeRange timeRange, int[] cnlNums)
        {
            return cnlNums.Length == 1 ? 
                GetFirstTrend(timeRange, cnlNums) : 
                MergeTrends(timeRange, cnlNums);
        }

        /// <summary>
        /// Gets the trend of the specified channel.
        /// </summary>
        public override Trend GetTrend(TimeRange timeRange, int cnlNum)
        {
            stopwatch.Restart();
            Trend trend;
            string flux = GetCnlDataFlux(timeRange.StartTime, timeRange.EndTime, cnlNum);
            List<FluxTable> tables = queryApi.QueryAsync(flux, connOptions.Org).Result;

            if (tables.Count > 0)
            {
                List<FluxRecord> records = tables[0].Records;
                trend = new Trend(cnlNum, records.Count);

                ProcessRecords(records, timeRange, (timestamp, record) =>
                {
                    trend.Points.Add(new TrendPoint(timestamp)
                    {
                        Val = Convert.ToDouble(record.GetValueByKey(ValField)),
                        Stat = Convert.ToInt32(record.GetValueByKey(StatField))
                    });
                });
            }
            else
            {
                trend = new Trend(cnlNum, 0);
            }

            stopwatch.Stop();
            arcLog?.WriteAction(ServerPhrases.ReadingTrendCompleted,
                trend.Points.Count, stopwatch.ElapsedMilliseconds);
            return trend;
        }

        /// <summary>
        /// Gets the available timestamps.
        /// </summary>
        public override List<DateTime> GetTimestamps(TimeRange timeRange)
        {
            stopwatch.Restart();
            List<DateTime> timestamps;
            string flux = GetFlagFlux(timeRange.StartTime, timeRange.EndTime);
            List<FluxTable> tables = queryApi.QueryAsync(flux, connOptions.Org).Result;

            if (tables.Count > 0)
            {
                List<FluxRecord> records = tables[0].Records;
                timestamps = new List<DateTime>(records.Count);
                ProcessRecords(records, timeRange, (timestamp, record) => timestamps.Add(timestamp));
            }
            else
            {
                timestamps = new List<DateTime>();
            }

            stopwatch.Stop();
            arcLog?.WriteAction(ServerPhrases.ReadingTimestampsCompleted,
                timestamps.Count, stopwatch.ElapsedMilliseconds);
            return timestamps;
        }

        /// <summary>
        /// Gets the slice of the specified channels at the timestamp.
        /// </summary>
        public override Slice GetSlice(DateTime timestamp, int[] cnlNums)
        {
            stopwatch.Restart();
            string flux = GetCnlDataFlux(timestamp, timestamp, cnlNums);
            List<FluxTable> tables = queryApi.QueryAsync(flux, connOptions.Org).Result;

            Slice slice = new(timestamp, cnlNums);
            long timeMs = new DateTimeOffset(timestamp).ToUnixTimeMilliseconds();
            Dictionary<int, int> cnlIndexes = GetCnlIndexes(cnlNums);

            foreach (FluxTable table in tables)
            {
                if (GetCnlData(table, timeMs, out int cnlNum, out CnlData cnlData))
                {
                    slice.CnlData[cnlIndexes[cnlNum]] = cnlData;
                }
            }

            stopwatch.Stop();
            arcLog?.WriteAction(ServerPhrases.ReadingSliceCompleted, cnlNums.Length, stopwatch.ElapsedMilliseconds);
            return slice;
        }

        /// <summary>
        /// Gets the channel data.
        /// </summary>
        public override CnlData GetCnlData(DateTime timestamp, int cnlNum)
        {
            if (updatedCnlData != null && timestamp == updateTime &&
                updatedCnlData.TryGetValue(cnlNum, out CnlData cnlData))
            {
                return cnlData;
            }

            string flux = GetCnlDataFlux(timestamp, timestamp, cnlNum);
            List<FluxTable> tables = queryApi.QueryAsync(flux, connOptions.Org).Result;
            long timeMs = new DateTimeOffset(timestamp).ToUnixTimeMilliseconds();
            return tables.Count > 0 && GetCnlData(tables[0], timeMs, out int n, out cnlData) && cnlNum == n ?
                cnlData : CnlData.Empty;
        }

        /// <summary>
        /// Processes new data.
        /// </summary>
        public override bool ProcessData(ICurrentData curData)
        {
            // InfluxDB client supports batch writing, so no queue implementation is required
            // https://github.com/influxdata/influxdb-client-csharp/tree/master/Client#writes
            // https://docs.influxdata.com/influxdb/v2.0/write-data/best-practices/optimize-writes/

            if (options.ReadOnly)
            {
                // do nothing
            }
            else if (options.WriteWithPeriod && nextWriteTime <= curData.Timestamp)
            {
                DateTime writeTime = GetClosestWriteTime(curData.Timestamp, writingPeriod);
                nextWriteTime = writeTime.AddSeconds(writingPeriod);

                stopwatch.Restart();
                InitCnlIndexes(curData, ref cnlIndexes);
                InitPrevCnlData(curData, cnlIndexes, ref prevCnlData);
                WriteFlag(writeTime);
                int cnlCnt = CnlNums.Length;

                for (int i = 0; i < cnlCnt; i++)
                {
                    WritePoint(writeTime, CnlNums[i], curData.CnlData[cnlIndexes[i]]);
                }

                stopwatch.Stop();
                arcLog?.WriteAction(ServerPhrases.WritingPointsCompleted, cnlCnt, stopwatch.ElapsedMilliseconds);
                return true;
            }
            else if (options.WriteOnChange)
            {
                stopwatch.Restart();
                int changesCnt = 0;
                bool justInited = prevCnlData == null;
                InitCnlIndexes(curData, ref cnlIndexes);
                InitPrevCnlData(curData, cnlIndexes, ref prevCnlData);

                if (!justInited)
                {
                    for (int i = 0, cnlCnt = CnlNums.Length; i < cnlCnt; i++)
                    {
                        CnlData curCnlData = curData.CnlData[cnlIndexes[i]];

                        if (!cnlDataEqualsFunc(prevCnlData[i], curCnlData))
                        {
                            prevCnlData[i] = curCnlData;
                            WritePoint(curData.Timestamp, CnlNums[i], curCnlData);
                            changesCnt++;
                        }
                    }
                }

                if (changesCnt > 0)
                {
                    WriteFlag(curData.Timestamp);
                    stopwatch.Stop();
                    arcLog?.WriteAction(ServerPhrases.WritingPointsCompleted, changesCnt, stopwatch.ElapsedMilliseconds);
                    return true;
                }
                else
                {
                    stopwatch.Stop();
                }
            }

            return false;
        }

        /// <summary>
        /// Accepts or rejects data with the specified timestamp.
        /// </summary>
        public override bool AcceptData(ref DateTime timestamp)
        {
            if (options.ReadOnly)
            {
                return false;
            }
            else if (options.IsPeriodic)
            {
                return options.PullToPeriod > 0
                    ? PullTimeToPeriod(ref timestamp, writingPeriod, options.PullToPeriod)
                    : TimeIsMultipleOfPeriod(timestamp, writingPeriod);
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Maintains performance when data is written one at a time.
        /// </summary>
        public override void BeginUpdate(DateTime timestamp, int deviceNum)
        {
            stopwatch.Restart();
            WriteFlag(timestamp);
            updateTime = timestamp;
            updatedCnlData = new Dictionary<int, CnlData>();
        }

        /// <summary>
        /// Completes the update operation.
        /// </summary>
        public override void EndUpdate(DateTime timestamp, int deviceNum)
        {
            updateTime = DateTime.MinValue;
            updatedCnlData = null;
            stopwatch.Stop();
            arcLog?.WriteAction(ServerPhrases.UpdateCompleted, stopwatch.ElapsedMilliseconds);
        }

        /// <summary>
        /// Writes the channel data.
        /// </summary>
        public override void WriteCnlData(DateTime timestamp, int cnlNum, CnlData cnlData)
        {
            if (!options.ReadOnly)
            {
                WritePoint(timestamp, cnlNum, cnlData);

                if (updatedCnlData != null)
                    updatedCnlData[cnlNum] = cnlData;
            }
        }
    }
}
