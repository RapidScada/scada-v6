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
using System.Diagnostics;

namespace Scada.Server.Modules.ModArcPostgreSql.Logic
{
    /// <summary>
    /// Implements the current data archive logic.
    /// <para>Реализует логику архива текущих данных.</para>
    /// </summary>
    internal class PostgreCAL : CurrentArchiveLogic
    {
        private readonly ModuleConfig moduleConfig; // the module configuration
        private readonly PostgreCAO options;        // the archive options
        private readonly ILog appLog;               // the application log
        private readonly ILog arcLog;               // the archive log
        private readonly QueryBuilder queryBuilder; // builds SQL requests
        private readonly PointQueue pointQueue;     // contains data points for writing
        private readonly object readingLock;        // synchronizes reading from the archive
        private readonly object writingLock;        // synchronizes writing to the archive

        private DbConnectionOptions connOptions;    // the database connection options
        private NpgsqlConnection readingConn;       // the database connection for reading
        private Thread thread;                      // the thread for writing data
        private volatile bool terminated;           // necessary to stop the thread
        private DateTime nextWriteTime;             // the next time to write the current data
        private int[] cnlIndexes;                   // the channel mapping indexes


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PostgreCAL(IArchiveContext archiveContext, ArchiveConfig archiveConfig, int[] cnlNums,
            ModuleConfig moduleConfig) : base(archiveContext, archiveConfig, cnlNums)
        {
            this.moduleConfig = moduleConfig ?? throw new ArgumentNullException(nameof(moduleConfig));
            options = new PostgreCAO(archiveConfig.CustomOptions);
            appLog = archiveContext.Log;
            arcLog = options.LogEnabled ? CreateLog(ModuleUtils.ModuleCode) : null;
            queryBuilder = new QueryBuilder(Code);
            pointQueue = options.ReadOnly
                ? null
                : new PointQueue(FixQueueSize(), options.BatchSize, queryBuilder.InsertCurrentDataQuery)
                {
                    RemoveExceeded = true,
                    ArchiveCode = Code,
                    AppLog = appLog,
                    ArcLog = arcLog
                };
            readingLock = new object();
            writingLock = new object();

            connOptions = null;
            readingConn = null;
            thread = null;
            terminated = false;
            nextWriteTime = DateTime.MinValue;
            cnlIndexes = null;
        }


        /// <summary>
        /// Gets the current archive status as text.
        /// </summary>
        public override string StatusText
        {
            get
            {
                return GetStatusText(pointQueue?.Stats, pointQueue?.Count);
            }
        }


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

                NpgsqlCommand cmd2 = new(queryBuilder.CreateCurrentTableQuery, conn);
                cmd2.ExecuteNonQuery();
            }
            finally
            {
                conn?.Close();
            }
        }

        /// <summary>
        /// Writing loop running in a separate thread.
        /// </summary>
        private void Execute()
        {
            while (!terminated)
            {
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
            connOptions = options.UseDefaultConn
                ? ArchiveContext.InstanceConfig.Connection
                : moduleConfig.GetConnectionOptions(options.Connection);
            readingConn = DbUtils.CreateDbConnection(connOptions);

            if (!options.ReadOnly)
            {
                pointQueue.Connection = DbUtils.CreateDbConnection(connOptions); // a new connection for the queue
                CreateDbEntities();

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
                DateTime timestamp = DbUtils.GetLastWriteTime(readingConn, queryBuilder.CurrentTable);
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
        /// Reads the current data.
        /// </summary>
        public override void ReadData(ICurrentData curData, out bool completed)
        {
            NpgsqlTransaction trans = null;

            try
            {
                Monitor.Enter(readingLock);
                Stopwatch stopwatch = Stopwatch.StartNew();
                readingConn.Open();
                trans = readingConn.BeginTransaction();

                string sql = "SELECT cnl_num, time_stamp, val, stat FROM " + queryBuilder.CurrentTable;
                NpgsqlCommand cmd = new(sql, readingConn, trans);
                List<int> cnlsToDelete = [];
                int pointCnt = 0;

                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int cnlNum = reader.GetInt32(0);
                        int cnlIndex = curData.GetCnlIndex(cnlNum);

                        if (cnlIndex >= 0)
                        {
                            curData.Timestamps[cnlIndex] = reader.GetDateTimeUtc(1);
                            curData.CnlData[cnlIndex] = new CnlData
                            {
                                Val = reader.GetDouble(2),
                                Stat = reader.GetInt32(3)
                            };
                            pointCnt++;
                        }
                        else
                        {
                            cnlsToDelete.Add(cnlNum);
                        }
                    }
                }

                // delete data of unused channels
                if (cnlsToDelete.Count > 0)
                {
                    sql = $"DELETE FROM {queryBuilder.CurrentTable} WHERE cnl_num = @cnlNum";
                    cmd = new NpgsqlCommand(sql, readingConn, trans);
                    NpgsqlParameter cnlNumParam = cmd.Parameters.Add("cnlNum", NpgsqlDbType.Integer);

                    foreach (int cnlNum in cnlsToDelete)
                    {
                        cnlNumParam.Value = cnlNum;
                        cmd.ExecuteNonQuery();
                    }
                }

                trans.Commit();
                completed = true;
                stopwatch.Stop();
                arcLog?.WriteAction(ServerPhrases.ReadingPointsCompleted, pointCnt, stopwatch.ElapsedMilliseconds);
            }
            catch
            {
                trans?.Rollback();
                completed = false;
            }
            finally
            {
                readingConn.SilentClose();
                Monitor.Exit(readingLock);
            }
        }

        /// <summary>
        /// Writes the current data.
        /// </summary>
        public override void WriteData(ICurrentData curData)
        {
            if (options.ReadOnly)
                return;

            lock (writingLock)
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                InitCnlIndexes(curData, ref cnlIndexes);
                int addedCnt = 0;
                int lostCnt = 0;

                lock (pointQueue.SyncRoot)
                {
                    for (int i = 0, cnlCnt = CnlNums.Length; i < cnlCnt; i++)
                    {
                        int cnlIndex = cnlIndexes[i];

                        if (pointQueue.EnqueueNoLock(new CnlDataPoint(
                            CnlNums[i], curData.Timestamps[cnlIndex], curData.CnlData[cnlIndex])))
                        {
                            addedCnt++;
                        }
                        else
                        {
                            lostCnt = cnlCnt - i;
                            break;
                        }
                    }
                }

                stopwatch.Stop();

                if (addedCnt > 0)
                    arcLog?.WriteAction(ServerPhrases.QueueingPointsCompleted, addedCnt, stopwatch.ElapsedMilliseconds);

                if (lostCnt > 0)
                    arcLog?.WriteWarning(ServerPhrases.PointsLost, lostCnt);
            }
        }

        /// <summary>
        /// Processes new data.
        /// </summary>
        public override void ProcessData(ICurrentData curData)
        {
            if (!options.ReadOnly && nextWriteTime <= curData.Timestamp)
            {
                nextWriteTime = GetNextWriteTime(curData.Timestamp, options.FlushPeriod, 0);
                WriteData(curData);
            }
        }
    }
}
