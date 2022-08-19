// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Npgsql;
using NpgsqlTypes;
using Scada.Config;
using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Log;
using Scada.Server.Archives;
using Scada.Server.Config;
using Scada.Server.Lang;
using Scada.Server.Modules.ModArcPostgreSql.Config;
using System.Data;
using System.Data.Common;
using System.Diagnostics;

namespace Scada.Server.Modules.ModArcPostgreSql.Logic
{
    /// <summary>
    /// Implements the event archive logic.
    /// <para>Реализует логику архива событий.</para>
    /// </summary>
    internal class PostgreEAL : EventArchiveLogic
    {
        private readonly ModuleConfig moduleConfig; // the module configuration
        private readonly PostgreEAO options;        // the archive options
        private readonly ILog appLog;               // the application log
        private readonly ILog arcLog;               // the archive log
        private readonly Stopwatch stopwatch;       // measures the time of operations
        private readonly QueryBuilder queryBuilder; // builds SQL requests
        private readonly EventQueue eventQueue;     // contains events for writing

        private bool hasError;            // the archive is in error state
        private NpgsqlConnection conn;    // the database connection
        private Thread thread;            // the thread for writing events
        private volatile bool terminated; // necessary to stop the thread


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PostgreEAL(IArchiveContext archiveContext, ArchiveConfig archiveConfig, int[] cnlNums,
            ModuleConfig moduleConfig) : base(archiveContext, archiveConfig, cnlNums)
        {
            this.moduleConfig = moduleConfig ?? throw new ArgumentNullException(nameof(moduleConfig));
            options = new PostgreEAO(archiveConfig.CustomOptions);
            appLog = archiveContext.Log;
            arcLog = options.LogEnabled ? CreateLog(ModuleUtils.ModuleCode) : null;
            stopwatch = new Stopwatch();
            queryBuilder = new QueryBuilder(Code);
            eventQueue = options.ReadOnly
                ? null
                : new EventQueue(FixQueueSize(), queryBuilder.InsertEventQuery)
                {
                    ArchiveCode = Code,
                    AppLog = appLog,
                    ArcLog = arcLog
                };

            hasError = false;
            conn = null;
            thread = null;
            terminated = false;
        }


        /// <summary>
        /// Gets the current archive status as text.
        /// </summary>
        public override string StatusText
        {
            get
            {
                return DbUtils.GetStatusText(IsReady, hasError, eventQueue);
            }
        }


        /// <summary>
        /// Checks and corrects the size of the event queue.
        /// </summary>
        private int FixQueueSize()
        {
            return Math.Max(options.MaxQueueSize, DbUtils.MinQueueSize);
        }

        /// <summary>
        /// Creates database entities if they do not exist.
        /// </summary>
        private void CreateDbEntities()
        {
            try
            {
                conn.Open();

                NpgsqlCommand cmd1 = new(queryBuilder.CreateSchemaQuery, conn);
                cmd1.ExecuteNonQuery();

                NpgsqlCommand cmd2 = new(queryBuilder.CreateEventTableQuery, conn);
                cmd2.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// Creates a necessary partition if it does not exist.
        /// </summary>
        private void CreatePartition(DateTime today, bool throwOnFail)
        {
            try
            {
                stopwatch.Restart();
                conn.Open();
                DbUtils.CreatePartition(conn, queryBuilder.EventTable,
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
                    Thread.Sleep(DbUtils.ErrorDelay);
                }
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// Reads an event.
        /// </summary>
        private static Event ReadEvent(NpgsqlDataReader reader)
        {
            return new Event
            {
                EventID = reader.GetInt64(0),
                Timestamp = reader.GetDateTimeUtc(1),
                Hidden = reader.GetBoolean(2),
                CnlNum = reader.GetInt32(3),
                ObjNum = reader.GetInt32(4),
                DeviceNum = reader.GetInt32(5),
                PrevCnlVal = reader.GetDouble(6),
                PrevCnlStat = reader.GetInt32(7),
                CnlVal = reader.GetDouble(8),
                CnlStat = reader.GetInt32(9),
                Severity = reader.GetInt32(10),
                AckRequired = reader.GetBoolean(11),
                Ack = reader.GetBoolean(12),
                AckTimestamp = reader.GetDateTimeUtc(13),
                AckUserID = reader.GetInt32(14),
                TextFormat = (EventTextFormat)reader.GetInt32(15),
                Text = reader.IsDBNull(16) ? "" : reader.GetString(16),
                Data = reader.IsDBNull(17) ? null : (byte[])reader[17]
            };
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

                if (eventQueue.Count > 0)
                    eventQueue.InsertEvents();

                Thread.Sleep(ScadaUtils.ThreadDelay);
            }
        }


        /// <summary>
        /// Makes the archive ready for operating.
        /// </summary>
        public override void MakeReady()
        {
            // prepare database
            DbConnectionOptions connOptions = options.UseStorageConn
                ? DbUtils.GetConnectionOptions(ArchiveContext.InstanceConfig)
                : DbUtils.GetConnectionOptions(moduleConfig, options.Connection);
            conn = DbUtils.CreateDbConnection(connOptions);

            if (!options.ReadOnly)
            {
                eventQueue.Connection = DbUtils.CreateDbConnection(connOptions); // a new connection for the queue
                CreateDbEntities();
                CreatePartition(DateTime.UtcNow, true);

                // start thread for writing events
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

            if (eventQueue?.Connection != null)
            {
                eventQueue.FlushEvents();
                eventQueue.Connection.Dispose();
                eventQueue.Connection = null;
            }

            if (conn != null)
            {
                conn.Dispose();
                conn = null;
            }
        }

        /// <summary>
        /// Deletes the outdated data from the archive.
        /// </summary>
        public override void DeleteOutdatedData()
        {
            if (options.ReadOnly)
                return;

            DateTime minDT = DateTime.UtcNow.AddDays(-options.Retention);
            appLog.WriteAction(ServerPhrases.DeleteOutdatedData, Code, minDT.ToLocalizedDateString());

            try
            {
                conn.Open();

                foreach (string partitionName in
                    DbUtils.GetOutdatedPartitions(conn, queryBuilder.EventTable, minDT))
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
                conn.Close();
            }
        }

        /// <summary>
        /// Gets the time (UTC) when the archive was last written to.
        /// </summary>
        public override DateTime GetLastWriteTime()
        {
            if (!options.ReadOnly)
                return ScadaUtils.Max(eventQueue.LastCommitTime, LastWriteTime);

            try
            {
                stopwatch.Restart();
                conn.Open();
                DateTime timestamp = DbUtils.GetLastWriteTime(conn, queryBuilder.EventTable);
                stopwatch.Stop();
                arcLog?.WriteAction(ServerPhrases.ReadingWriteTimeCompleted, stopwatch.ElapsedMilliseconds);
                return timestamp;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// Gets the event by ID.
        /// </summary>
        public override Event GetEventByID(long eventID)
        {
            try
            {
                stopwatch.Restart();
                conn.Open();

                string sql = queryBuilder.SelectEventQuery +
                    "WHERE event_id = @eventID AND @startTime <= time_stamp AND time_stamp < @endTime";

                DateTime eventTime = ScadaUtils.RetrieveTimeFromID(eventID); // accurate to seconds
                NpgsqlCommand cmd = new(sql, conn);
                cmd.Parameters.AddWithValue("eventID", eventID);
                cmd.Parameters.AddWithValue("startTime", eventTime);
                cmd.Parameters.AddWithValue("endTime", eventTime.AddSeconds(1.0));
                Event ev = null;

                using (NpgsqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (reader.Read())
                        ev = ReadEvent(reader);
                }

                stopwatch.Stop();
                arcLog?.WriteAction(ServerPhrases.ReadingEventCompleted, stopwatch.ElapsedMilliseconds);
                return ev;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// Gets the events ordered by timestamp.
        /// </summary>
        public override List<Event> GetEvents(TimeRange timeRange, DataFilter filter)
        {
            try
            {
                stopwatch.Restart();
                conn.Open();

                string sqlFilter = filter.GetSqlFilter(QueryBuilder.EventColumnMap, "AND (", ")",
                    () => { return new NpgsqlParameter(); }, out List<DbParameter> dbParams);
                string endOper = timeRange.EndInclusive ? "<=" : "<";
                string sortOrder = filter.OriginBegin ? "ASC" : "DESC";
                string sql = queryBuilder.SelectEventQuery +
                    $"WHERE @startTime <= time_stamp AND time_stamp {endOper} @endTime {sqlFilter} " +
                    $"ORDER BY time_stamp {sortOrder} " +
                    $"LIMIT {(filter.Limit > 0 ? filter.Limit.ToString() : "ALL")} " +
                    $"OFFSET {filter.Offset}";

                NpgsqlCommand cmd = new(sql, conn);
                cmd.Parameters.Add("startTime", NpgsqlDbType.TimestampTz).Value = timeRange.StartTime;
                cmd.Parameters.Add("endTime", NpgsqlDbType.TimestampTz).Value = timeRange.EndTime;
                dbParams.ForEach(p => cmd.Parameters.Add(p));
                List<Event> events = new();

                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        events.Add(ReadEvent(reader));
                    }
                }

                if (!filter.OriginBegin)
                    events.Reverse();

                stopwatch.Stop();
                arcLog?.WriteAction(ServerPhrases.ReadingEventsCompleted, events.Count, stopwatch.ElapsedMilliseconds);
                return events;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// Writes the event.
        /// </summary>
        public override void WriteEvent(Event ev)
        {
            if (!options.ReadOnly)
            {
                stopwatch.Restart();
                eventQueue.EnqueueEvent(ev);
                stopwatch.Stop();
                arcLog?.WriteAction(ServerPhrases.QueueingEventCompleted, stopwatch.ElapsedMilliseconds);
            }
        }

        /// <summary>
        /// Acknowledges the event.
        /// </summary>
        public override void AckEvent(EventAck eventAck)
        {
            try
            {
                stopwatch.Restart();
                conn.Open();

                string sql = $"UPDATE {queryBuilder.EventTable} " +
                    "SET ack = true, ack_timestamp = @ackTimestamp, ack_user_id = @ackUserID " +
                    "WHERE event_id = @eventID AND @startTime <= time_stamp AND time_stamp < @endTime";

                DateTime eventTime = ScadaUtils.RetrieveTimeFromID(eventAck.EventID);
                NpgsqlCommand cmd = new(sql, conn);
                cmd.Parameters.AddWithValue("ackTimestamp", eventAck.Timestamp);
                cmd.Parameters.AddWithValue("ackUserID", eventAck.UserID);
                cmd.Parameters.AddWithValue("eventID", eventAck.EventID);
                cmd.Parameters.AddWithValue("startTime", eventTime);
                cmd.Parameters.AddWithValue("endTime", eventTime.AddSeconds(1.0));
                int rowsAffected = cmd.ExecuteNonQuery();
                LastWriteTime = DateTime.UtcNow;
                stopwatch.Stop();

                if (rowsAffected > 0)
                {
                    arcLog?.WriteAction(ServerPhrases.AckEventCompleted, eventAck.EventID, 
                        stopwatch.ElapsedMilliseconds);
                }
                else
                {
                    arcLog?.WriteAction(ServerPhrases.AckEventNotFound, eventAck.EventID);
                }
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
