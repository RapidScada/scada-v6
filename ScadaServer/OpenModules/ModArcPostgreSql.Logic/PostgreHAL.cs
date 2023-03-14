// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Npgsql;
using NpgsqlTypes;
using Scada.Data.Models;
using Scada.Dbms;
using Scada.Log;
using Scada.Server.Archives;
using Scada.Server.Config;
using Scada.Server.Lang;
using Scada.Server.Modules.ModArcPostgreSql.Config;
using System.Data;
using System.Diagnostics;

namespace Scada.Server.Modules.ModArcPostgreSql.Logic
{
    /// <summary>
    /// Implements the historical data archive logic.
    /// <para>Реализует логику архива исторических данных.</para>
    /// </summary>
    internal class PostgreHAL : HistoricalArchiveLogic
    {
        private readonly ModuleConfig moduleConfig; // the module configuration
        private readonly PostgreHAO options;        // the archive options
        private readonly ILog appLog;               // the application log
        private readonly ILog arcLog;               // the archive log
        private readonly Stopwatch stopwatch;       // measures the time of operations
        private readonly QueryBuilder queryBuilder; // builds SQL requests
        private readonly PointQueue pointQueue;     // contains data points for writing
        private readonly int writingPeriod;         // the writing period in seconds
        private readonly CnlDataEqualsDelegate cnlDataEqualsFunc; // the function for comparing channel data
        private readonly object readingLock;        // synchronizes reading from the archive
        private readonly object writingLock;        // synchronizes writing to the archive

        private bool hasError;                      // the archive is in error state
        private DbConnectionOptions connOptions;    // the database connection options
        private NpgsqlConnection readingConn;       // the database connection for reading
        private Thread thread;                      // the thread for writing data
        private volatile bool terminated;           // necessary to stop the thread
        private DateTime nextWriteTime;             // the next time to write the current data
        private int[] cnlIndexes;                   // the channel mapping indexes
        private CnlData[] prevCnlData;              // the previous channel data
        private DateTime updateTime;                // the timestamp of the current update operation
        private Dictionary<int, CnlData> updatedCnlData; // holds recently updated channel data


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PostgreHAL(IArchiveContext archiveContext, ArchiveConfig archiveConfig, int[] cnlNums,
            ModuleConfig moduleConfig) : base(archiveContext, archiveConfig, cnlNums)
        {
            this.moduleConfig = moduleConfig ?? throw new ArgumentNullException(nameof(moduleConfig));
            options = new PostgreHAO(archiveConfig.CustomOptions);
            appLog = archiveContext.Log;
            arcLog = options.LogEnabled ? CreateLog(ModuleUtils.ModuleCode) : null;
            stopwatch = new Stopwatch();
            queryBuilder = new QueryBuilder(Code);
            pointQueue = options.ReadOnly
                ? null
                : new PointQueue(FixQueueSize(), options.BatchSize, queryBuilder.InsertHistoricalDataQuery)
                {
                    ReturnOnError = true,
                    ArchiveCode = Code,
                    AppLog = appLog,
                    ArcLog = arcLog
                };
            writingPeriod = GetPeriodInSec(options.WritingPeriod, options.WritingPeriodUnit);
            cnlDataEqualsFunc = SelectCnlDataEquals();
            readingLock = new object();
            writingLock = new object();

            hasError = false;
            connOptions = null;
            readingConn = null;
            thread = null;
            terminated = false;
            nextWriteTime = DateTime.MinValue;
            cnlIndexes = null;
            prevCnlData = null;
            updateTime = DateTime.MinValue;
            updatedCnlData = null;
        }


        /// <summary>
        /// Gets the current archive status as text.
        /// </summary>
        public override string StatusText
        {
            get
            {
                return DbUtils.GetStatusText(IsReady, hasError, pointQueue);
            }
        }

        /// <summary>
        /// Gets the archive options.
        /// </summary>
        protected override HistoricalArchiveOptions ArchiveOptions => options;


        /// <summary>
        /// Corrects the size of the data queue.
        /// </summary>
        private int FixQueueSize()
        {
            return Math.Max(options.MaxQueueSize, CnlNums.Length * 2);
        }

        /// <summary>
        /// Creates database entities if they do not exist.
        /// </summary>
        private void CreateDbEntities()
        {
            NpgsqlConnection conn = null;

            try
            {
                conn = DbUtils.CreateDbConnection(connOptions);
                conn.Open();

                NpgsqlCommand cmd1 = new(queryBuilder.CreateSchemaQuery, conn);
                cmd1.ExecuteNonQuery();

                NpgsqlCommand cmd2 = new(queryBuilder.CreateHistoricalTableQuery, conn);
                cmd2.ExecuteNonQuery();
            }
            finally
            {
                conn?.Close();
            }
        }

        /// <summary>
        /// Creates a necessary partition if it does not exist.
        /// </summary>
        private void CreatePartition(DateTime today, bool throwOnFail)
        {
            NpgsqlConnection conn = null;

            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                conn = DbUtils.CreateDbConnection(connOptions);
                conn.Open();

                DbUtils.CreatePartition(conn, queryBuilder.HistoricalTable, 
                    today, options.PartitionSize, out string partitionName);
                stopwatch.Stop();

                hasError = false;
                arcLog?.WriteAction(ModulePhrases.CreationPartitionCompleted,
                    partitionName, stopwatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                hasError = true;

                if (throwOnFail)
                {
                    throw;
                }
                else
                {
                    appLog.WriteError(ex, ServerPhrases.ArchiveMessage, Code, ModulePhrases.CreatePartitionError);
                    arcLog?.WriteError(ex, ModulePhrases.CreatePartitionError);
                    Thread.Sleep(ScadaUtils.ErrorDelay);
                }
            }
            finally
            {
                conn?.Close();
            }
        }

        /// <summary>
        /// Gets a trend bundle containing the trend of the first channel.
        /// </summary>
        private TrendBundle GetFirstTrend(TimeRange timeRange, int[] cnlNums)
        {
            try
            {
                Monitor.Enter(readingLock);
                Stopwatch stopwatch = Stopwatch.StartNew();
                readingConn.Open();

                TrendBundle trendBundle = new(cnlNums, 0);
                TrendBundle.CnlDataList trend = trendBundle.Trends[0];
                NpgsqlCommand cmd = CreateTrendCommand(timeRange, cnlNums[0]);
                NpgsqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    trendBundle.Timestamps.Add(reader.GetDateTimeUtc(0));
                    trend.Add(new CnlData(
                        reader.GetDouble(1),
                        reader.GetInt32(2)));
                }

                stopwatch.Stop();
                arcLog?.WriteAction(ServerPhrases.ReadingTrendCompleted,
                    trendBundle.Timestamps.Count, stopwatch.ElapsedMilliseconds);
                return trendBundle;
            }
            finally
            {
                readingConn.SilentClose();
                Monitor.Exit(readingLock);
            }
        }

        /// <summary>
        /// Creates a command to get a trend of the specified channel.
        /// </summary>
        private NpgsqlCommand CreateTrendCommand(TimeRange timeRange, int cnlNum)
        {
            string endOper = timeRange.EndInclusive ? "<=" : "<";
            string sql =
                $"SELECT DISTINCT time_stamp, val, stat FROM {queryBuilder.HistoricalTable} " +
                $"WHERE cnl_num = @cnlNum AND @startTime <= time_stamp AND time_stamp {endOper} @endTime " +
                $"ORDER BY time_stamp";
            NpgsqlCommand cmd = new(sql, readingConn);
            cmd.Parameters.AddWithValue("cnlNum", cnlNum);
            cmd.Parameters.Add("startTime", NpgsqlDbType.TimestampTz).Value = timeRange.StartTime;
            cmd.Parameters.Add("endTime", NpgsqlDbType.TimestampTz).Value = timeRange.EndTime;
            return cmd;
        }

        /// <summary>
        /// Gets the recently updated channel data.
        /// </summary>
        private bool GetRecentCnlData(DateTime timestamp, int cnlNum, out CnlData cnlData)
        {
            if (Monitor.TryEnter(writingLock))
            {
                try
                {
                    if (updatedCnlData != null && timestamp == updateTime && 
                        updatedCnlData.TryGetValue(cnlNum, out cnlData))
                    {
                        return true;
                    }
                }
                finally
                {
                    Monitor.Exit(writingLock); 
                }
            }

            cnlData = CnlData.Empty;
            return false;
        }

        /// <summary>
        /// Writes the current data after the writing period has elapsed.
        /// </summary>
        private void WriteWithPeriod(ICurrentData curData)
        {
            lock (writingLock)
            {
                DateTime writeTime = GetClosestWriteTime(curData.Timestamp, writingPeriod);
                nextWriteTime = writeTime.AddSeconds(writingPeriod);

                Stopwatch stopwatch = Stopwatch.StartNew();
                InitCnlIndexes(curData, ref cnlIndexes);
                InitPrevCnlData(curData, cnlIndexes, ref prevCnlData);
                int cnlCnt = CnlNums.Length;

                lock (pointQueue.SyncRoot)
                {
                    for (int i = 0; i < cnlCnt; i++)
                    {
                        CnlData cnlData = curData.CnlData[cnlIndexes[i]];

                        if (prevCnlData != null)
                            prevCnlData[i] = cnlData;

                        pointQueue.EnqueueWithoutLock(CnlNums[i], writeTime, cnlData);
                    }
                }

                pointQueue.RemoveExcessItems();
                stopwatch.Stop();
                arcLog?.WriteAction(ServerPhrases.QueueingPointsCompleted, cnlCnt, stopwatch.ElapsedMilliseconds);
            }
        }

        /// <summary>
        /// Writes the current data on change.
        /// </summary>
        private void WriteOnChange(ICurrentData curData)
        {
            lock (writingLock)
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                int changesCnt = 0;
                bool justInited = prevCnlData == null;
                InitCnlIndexes(curData, ref cnlIndexes);
                InitPrevCnlData(curData, cnlIndexes, ref prevCnlData);

                if (!justInited)
                {
                    for (int i = 0, cnlCnt = CnlNums.Length; i < cnlCnt; i++)
                    {
                        CnlData cnlData = curData.CnlData[cnlIndexes[i]];

                        if (!cnlDataEqualsFunc(prevCnlData[i], cnlData))
                        {
                            prevCnlData[i] = cnlData;
                            pointQueue.EnqueuePoint(CnlNums[i], curData.Timestamp, cnlData);
                            changesCnt++;
                        }
                    }
                }

                if (changesCnt > 0)
                {
                    pointQueue.RemoveExcessItems();
                    stopwatch.Stop();
                    arcLog?.WriteAction(ServerPhrases.QueueingPointsCompleted,
                        changesCnt, stopwatch.ElapsedMilliseconds);
                }
                else
                {
                    stopwatch.Stop();
                }
            }
        }

        /// <summary>
        /// Writing loop running in a separate thread.
        /// </summary>
        private void Execute()
        {
            DateTime prevDate = DateTime.UtcNow.Date;

            while (!terminated)
            {
                DateTime today = DateTime.UtcNow.Date;

                if (prevDate != today)
                {
                    prevDate = today;
                    CreatePartition(today, false);
                }

                pointQueue.ProcessItems();
                Thread.Sleep(ScadaUtils.ThreadDelay);
            }
        }


        /// <summary>
        /// Makes the archive ready for operating.
        /// </summary>
        public override void MakeReady()
        {
            // prepare database
            connOptions = options.UseStorageConn
                ? DbUtils.GetConnectionOptions(ArchiveContext.InstanceConfig)
                : DbUtils.GetConnectionOptions(moduleConfig, options.Connection);
            readingConn = DbUtils.CreateDbConnection(connOptions);

            if (!options.ReadOnly)
            {
                pointQueue.Connection = DbUtils.CreateDbConnection(connOptions); // a new connection for the queue
                CreateDbEntities();
                CreatePartition(DateTime.UtcNow, true);

                if (options.WriteWithPeriod)
                    nextWriteTime = GetNextWriteTime(DateTime.UtcNow, writingPeriod);

                if (options.WriteOnChange)
                    appLog.WriteWarning(ServerPhrases.ArchiveMessage, Code, ServerPhrases.WritingOnChangeIsSlow);

                // start thread for writing data
                terminated = false;
                thread = new Thread(Execute);
                thread.Start();
            }
        }

        /// <summary>
        /// Closes the archive.
        /// </summary>
        public override void Close()
        {
            if (thread != null)
            {
                terminated = true;
                thread.Join();
                thread = null;
            }

            if (pointQueue?.Connection != null)
            {
                pointQueue.FlushItems(ArchiveContext.AppConfig.GeneralOptions.StopWait);
                pointQueue.Connection.Dispose();
                pointQueue.Connection = null;
            }

            if (readingConn != null)
            {
                readingConn.Dispose();
                readingConn = null;
            }
        }

        /// <summary>
        /// Deletes the outdated data from the archive.
        /// </summary>
        public override void DeleteOutdatedData()
        {
            if (options.ReadOnly)
                return;

            NpgsqlConnection conn = null;

            try
            {
                DateTime minDT = DateTime.UtcNow.AddDays(-options.Retention);
                appLog.WriteAction(ServerPhrases.DeleteOutdatedData, Code, minDT.ToLocalizedDateString());

                conn = DbUtils.CreateDbConnection(connOptions);
                conn.Open();
                Stopwatch stopwatch = new();

                foreach (string partitionName in
                    DbUtils.GetOutdatedPartitions(conn, queryBuilder.HistoricalTable, minDT))
                {
                    stopwatch.Restart();
                    new NpgsqlCommand($"DROP TABLE {partitionName}", conn).ExecuteNonQuery();
                    stopwatch.Stop();
                    arcLog?.WriteAction(ModulePhrases.PartitionDeleted, partitionName, stopwatch.ElapsedMilliseconds);
                }

                hasError = false;
            }
            catch
            {
                hasError = true;
                throw;
            }
            finally
            {
                conn?.Close();
            }
        }

        /// <summary>
        /// Gets the time (UTC) when the archive was last written to.
        /// </summary>
        public override DateTime GetLastWriteTime()
        {
            if (!options.ReadOnly)
                return pointQueue.LastCommitTime;

            try
            {
                Monitor.Enter(readingLock);
                Stopwatch stopwatch = Stopwatch.StartNew();
                readingConn.Open();
                DateTime timestamp = DbUtils.GetLastWriteTime(readingConn, queryBuilder.HistoricalTable);
                stopwatch.Stop();
                arcLog?.WriteAction(ServerPhrases.ReadingWriteTimeCompleted, stopwatch.ElapsedMilliseconds);
                return timestamp;
            }
            finally
            {
                readingConn.SilentClose();
                Monitor.Exit(readingLock);
            }
        }

        /// <summary>
        /// Gets the trends of the specified channels.
        /// </summary>
        public override TrendBundle GetTrends(TimeRange timeRange, int[] cnlNums)
        {
            return cnlNums.Length == 1 
                ? GetFirstTrend(timeRange, cnlNums) 
                : MergeTrends(timeRange, cnlNums);
        }

        /// <summary>
        /// Gets the trend of the specified channel.
        /// </summary>
        public override Trend GetTrend(TimeRange timeRange, int cnlNum)
        {
            try
            {
                Monitor.Enter(readingLock);
                Stopwatch stopwatch = Stopwatch.StartNew();
                readingConn.Open();

                Trend trend = new(cnlNum, 0);
                NpgsqlCommand cmd = CreateTrendCommand(timeRange, cnlNum);
                NpgsqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    trend.Points.Add(new TrendPoint(
                        reader.GetDateTimeUtc(0),
                        reader.GetDouble(1),
                        reader.GetInt32(2)));
                }

                stopwatch.Stop();
                arcLog?.WriteAction(ServerPhrases.ReadingTrendCompleted,
                    trend.Points.Count, stopwatch.ElapsedMilliseconds);
                return trend;
            }
            finally
            {
                readingConn.SilentClose();
                Monitor.Exit(readingLock);
            }
        }

        /// <summary>
        /// Gets the available timestamps.
        /// </summary>
        public override List<DateTime> GetTimestamps(TimeRange timeRange)
        {
            try
            {
                Monitor.Enter(readingLock);
                Stopwatch stopwatch = Stopwatch.StartNew();
                readingConn.Open();
                List<DateTime> timestamps = new();

                string endOper = timeRange.EndInclusive ? "<=" : "<";
                string sql = 
                    $"SELECT DISTINCT time_stamp FROM {queryBuilder.HistoricalTable} " +
                    $"WHERE @startTime <= time_stamp AND time_stamp {endOper} @endTime " +
                    $"ORDER BY time_stamp";
                NpgsqlCommand cmd = new(sql, readingConn);
                cmd.Parameters.Add("startTime", NpgsqlDbType.TimestampTz).Value = timeRange.StartTime;
                cmd.Parameters.Add("endTime", NpgsqlDbType.TimestampTz).Value = timeRange.EndTime;
                NpgsqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    timestamps.Add(reader.GetDateTimeUtc(0));
                }

                stopwatch.Stop();
                arcLog?.WriteAction(ServerPhrases.ReadingTimestampsCompleted,
                    timestamps.Count, stopwatch.ElapsedMilliseconds);
                return timestamps;
            }
            finally
            {
                readingConn.SilentClose();
                Monitor.Exit(readingLock);
            }
        }

        /// <summary>
        /// Gets the slice of the specified channels at the timestamp.
        /// </summary>
        public override Slice GetSlice(DateTime timestamp, int[] cnlNums)
        {
            try
            {
                Monitor.Enter(readingLock);
                Stopwatch stopwatch = Stopwatch.StartNew();
                readingConn.Open();

                Slice slice = new(timestamp, cnlNums);
                Dictionary<int, int> cnlIndexes = GetCnlIndexes(cnlNums);

                string sql = 
                    $"SELECT cnl_num, val, stat FROM {queryBuilder.HistoricalTable} " +
                    $"WHERE cnl_num IN ({string.Join(",", cnlNums)}) AND time_stamp = @timestamp ";
                NpgsqlCommand cmd = new(sql, readingConn);
                cmd.Parameters.AddWithValue("timestamp", timestamp);
                NpgsqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int cnlNum = reader.GetInt32(0);

                    if (cnlIndexes.TryGetValue(cnlNum, out int cnlIndex))
                    {
                        slice.CnlData[cnlIndex] = new CnlData
                        {
                            Val = reader.GetDouble(1),
                            Stat = reader.GetInt32(2)
                        };
                    }
                }

                stopwatch.Stop();
                arcLog?.WriteAction(ServerPhrases.ReadingSliceCompleted, 
                    cnlNums.Length, stopwatch.ElapsedMilliseconds);
                return slice;
            }
            finally
            {
                readingConn.SilentClose();
                Monitor.Exit(readingLock);
            }
        }

        /// <summary>
        /// Gets the channel data.
        /// </summary>
        public override CnlData GetCnlData(DateTime timestamp, int cnlNum)
        {
            if (GetRecentCnlData(timestamp, cnlNum, out CnlData cnlData))
                return cnlData;

            try
            {
                Monitor.Enter(readingLock);
                readingConn.Open();

                string sql = $"SELECT val, stat FROM {queryBuilder.HistoricalTable} " + 
                    "WHERE cnl_num = @cnlNum AND time_stamp = @timestamp";
                NpgsqlCommand cmd = new(sql, readingConn);
                cmd.Parameters.AddWithValue("cnlNum", cnlNum);
                cmd.Parameters.AddWithValue("timestamp", timestamp);
                NpgsqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SingleRow);

                return reader.Read() ?
                    new CnlData(reader.GetDouble(0), reader.GetInt32(1)) :
                    CnlData.Empty;
            }
            finally
            {
                readingConn.SilentClose();
                Monitor.Exit(readingLock);
            }
        }

        /// <summary>
        /// Processes new data.
        /// </summary>
        public override void ProcessData(ICurrentData curData)
        {
            if (!options.ReadOnly)
            {
                if (options.WriteWithPeriod && nextWriteTime <= curData.Timestamp)
                    WriteWithPeriod(curData);
                else if (options.WriteOnChange)
                    WriteOnChange(curData);
            }
        }

        /// <summary>
        /// Accepts or rejects data with the specified timestamp.
        /// </summary>
        public override bool AcceptData(ref DateTime timestamp)
        {
            if (options.ReadOnly || !TimeInsideRetention(timestamp, DateTime.UtcNow))
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
            Monitor.Enter(writingLock);
            stopwatch.Restart();
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
            Monitor.Exit(writingLock);
        }

        /// <summary>
        /// Writes the channel data.
        /// </summary>
        public override void WriteCnlData(DateTime timestamp, int cnlNum, CnlData cnlData)
        {
            if (!options.ReadOnly)
            {
                lock (writingLock)
                {
                    pointQueue.EnqueuePoint(cnlNum, timestamp, cnlData);

                    if (updatedCnlData != null)
                        updatedCnlData[cnlNum] = cnlData;
                }
            }
        }
    }
}
