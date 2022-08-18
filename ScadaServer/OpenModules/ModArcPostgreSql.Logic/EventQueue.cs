// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Npgsql;
using NpgsqlTypes;
using Scada.Data.Models;
using Scada.Log;
using Scada.Server.Lang;

namespace Scada.Server.Modules.ModArcPostgreSql.Logic
{
    /// <summary>
    /// Represents a queue for writing events to a database.
    /// <para>Представляет очередь для записи событий в базу данных.</para>
    /// </summary>
    internal class EventQueue : IDataQueue
    {
        private readonly Queue<Event> eventQueue; // contains events for writing
        private readonly NpgsqlCommand command;   // writes an event
        private readonly NpgsqlParameter eventIdParam;
        private readonly NpgsqlParameter timestampParam;
        private readonly NpgsqlParameter hiddenParam;
        private readonly NpgsqlParameter cnlNumParam;
        private readonly NpgsqlParameter objNumParam;
        private readonly NpgsqlParameter deviceNumParam;
        private readonly NpgsqlParameter prevCnlValParam;
        private readonly NpgsqlParameter prevCnlStatParam;
        private readonly NpgsqlParameter cnlValParam;
        private readonly NpgsqlParameter cnlStatParam;
        private readonly NpgsqlParameter severityParam;
        private readonly NpgsqlParameter ackRequiredParam;
        private readonly NpgsqlParameter ackParam;
        private readonly NpgsqlParameter ackTimestampParam;
        private readonly NpgsqlParameter ackUserIDParam;
        private readonly NpgsqlParameter textFormatParam;
        private readonly NpgsqlParameter eventTextParam;
        private readonly NpgsqlParameter eventDataParam;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EventQueue(int maxQueueSize, string insertSql)
        {
            eventQueue = new Queue<Event>(maxQueueSize);

            command = new NpgsqlCommand(insertSql);
            eventIdParam = command.Parameters.Add("eventID", NpgsqlDbType.Bigint);
            timestampParam = command.Parameters.Add("timestamp", NpgsqlDbType.TimestampTz);
            hiddenParam = command.Parameters.Add("hidden", NpgsqlDbType.Boolean);
            cnlNumParam = command.Parameters.Add("cnlNum", NpgsqlDbType.Integer);
            objNumParam = command.Parameters.Add("objNum", NpgsqlDbType.Integer);
            deviceNumParam = command.Parameters.Add("deviceNum", NpgsqlDbType.Integer);
            prevCnlValParam = command.Parameters.Add("prevCnlVal", NpgsqlDbType.Double);
            prevCnlStatParam = command.Parameters.Add("prevCnlStat", NpgsqlDbType.Integer);
            cnlValParam = command.Parameters.Add("cnlVal", NpgsqlDbType.Double);
            cnlStatParam = command.Parameters.Add("cnlStat", NpgsqlDbType.Integer);
            severityParam = command.Parameters.Add("severity", NpgsqlDbType.Integer);
            ackRequiredParam = command.Parameters.Add("ackRequired", NpgsqlDbType.Boolean);
            ackParam = command.Parameters.Add("ack", NpgsqlDbType.Boolean);
            ackTimestampParam = command.Parameters.Add("ackTimestamp", NpgsqlDbType.TimestampTz);
            ackUserIDParam = command.Parameters.Add("ackUserID", NpgsqlDbType.Integer);
            textFormatParam = command.Parameters.Add("textFormat", NpgsqlDbType.Integer);
            eventTextParam = command.Parameters.Add("eventText", NpgsqlDbType.Varchar);
            eventDataParam = command.Parameters.Add("eventData", NpgsqlDbType.Bytea);

            MaxQueueSize = maxQueueSize;
            HasError = false;
        }


        /// <summary>
        /// Gets the maximum queue size.
        /// </summary>
        public int MaxQueueSize { get; }

        /// <summary>
        /// Gets the current queue size.
        /// </summary>
        public int Count
        {
            get
            {
                return eventQueue.Count;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the queue is in error state.
        /// </summary>
        public bool HasError { get; private set; }

        /// <summary>
        /// Gets or sets the archive code.
        /// </summary>
        public string ArchiveCode { get; set; }

        /// <summary>
        /// Gets or sets the application log.
        /// </summary>
        public ILog AppLog { get; set; }

        /// <summary>
        /// Gets or sets the archive log.
        /// </summary>
        public ILog ArcLog { get; set; }

        /// <summary>
        /// Gets or sets the database connection.
        /// </summary>
        public NpgsqlConnection Connection { get; set; }

        /// <summary>
        /// Gets an object that can be used to synchronize access to the queue.
        /// </summary>
        public object SyncRoot
        {
            get
            {
                return this;
            }
        }


        /// <summary>
        /// Sets the command parameters according to the event.
        /// </summary>
        private void SetCommandParams(Event ev)
        {
            eventIdParam.Value = ev.EventID;
            timestampParam.Value = ev.Timestamp;
            hiddenParam.Value = ev.Hidden;
            cnlNumParam.Value = ev.CnlNum;
            objNumParam.Value = ev.ObjNum;
            deviceNumParam.Value = ev.DeviceNum;
            prevCnlValParam.Value = ev.PrevCnlVal;
            prevCnlStatParam.Value = ev.PrevCnlStat;
            cnlValParam.Value = ev.CnlVal;
            cnlStatParam.Value = ev.CnlStat;
            severityParam.Value = ev.Severity;
            ackRequiredParam.Value = ev.AckRequired;
            ackParam.Value = ev.Ack;
            ackTimestampParam.Value = ev.AckTimestamp;
            ackUserIDParam.Value = ev.AckUserID;
            textFormatParam.Value = (int)ev.TextFormat;
            eventTextParam.Value = string.IsNullOrEmpty(ev.Text) ? (object)DBNull.Value : ev.Text;
            eventDataParam.Value = (object)ev.Data ?? DBNull.Value;
        }

        /// <summary>
        /// Enqueues the event to the queue.
        /// </summary>
        public void EnqueueEvent(Event ev)
        {
            lock (eventQueue)
            {
                eventQueue.Enqueue(ev);
            }
        }

        /// <summary>
        /// Inserts or updates events from the queue.
        /// </summary>
        public void InsertEvents()
        {
            if (Connection == null)
                throw new InvalidOperationException("Connection must not be null.");

            NpgsqlTransaction trans = null;

            try
            {
                Connection.Open();
                trans = Connection.BeginTransaction();
                command.Connection = Connection;
                command.Transaction = trans;

                for (int i = 0; i < DbUtils.BundleSize; i++)
                {
                    // retrieve an event from the queue
                    Event ev;

                    lock (eventQueue)
                    {
                        if (eventQueue.Count > 0)
                            ev = eventQueue.Dequeue();
                        else
                            break;
                    }

                    try
                    {
                        // write the event
                        SetCommandParams(ev);
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        // return the unwritten event to the queue
                        lock (eventQueue)
                        {
                            eventQueue.Enqueue(ev);
                        }

                        throw;
                    }
                }

                if (eventQueue.Count == 0)
                    ArcLog?.WriteInfo(ServerPhrases.QueueBecameEmpty);

                trans.Commit();
                HasError = false;
            }
            catch (Exception ex)
            {
                DbUtils.SafeRollback(trans);
                HasError = true;
                AppLog?.WriteError(ex, ServerPhrases.ArchiveMessage, ArchiveCode, ServerPhrases.WriteDbError);
                ArcLog?.WriteError(ex, ServerPhrases.WriteDbError);
                Thread.Sleep(DbUtils.ErrorDelay);
            }
            finally
            {
                Connection.Close();
            }
        }

        /// <summary>
        /// Writes all events from the queue.
        /// </summary>
        public void FlushEvents()
        {
            if (Connection == null)
                throw new InvalidOperationException("Connection must not be null.");

            if (eventQueue.Count == 0)
                return;

            NpgsqlTransaction trans = null;

            try
            {
                Connection.Open();
                trans = Connection.BeginTransaction();
                command.Connection = Connection;
                command.Transaction = trans;

                lock (eventQueue)
                {
                    while (eventQueue.Count > 0)
                    {
                        SetCommandParams(eventQueue.Dequeue());
                        command.ExecuteNonQuery();
                    }
                }

                ArcLog?.WriteInfo(ServerPhrases.QueueBecameEmpty);
                trans.Commit();
                HasError = false;
            }
            catch (Exception ex)
            {
                DbUtils.SafeRollback(trans);
                HasError = true;
                AppLog?.WriteError(ex, ServerPhrases.ArchiveMessage, ArchiveCode, ServerPhrases.WriteDbError);
                ArcLog?.WriteError(ex, ServerPhrases.WriteDbError);
            }
            finally
            {
                Connection.Close();
            }
        }

        /// <summary>
        /// Removes excess events from the head of the queue.
        /// </summary>
        public void RemoveExcessEvents()
        {
            int lostCnt = 0;

            lock (eventQueue)
            {
                while (eventQueue.Count > MaxQueueSize)
                {
                    eventQueue.Dequeue();
                    lostCnt++;
                }
            }

            if (lostCnt > 0)
            {
                string msg = string.Format(ServerPhrases.EventsWereLost, lostCnt);
                AppLog?.WriteError(ServerPhrases.ArchiveMessage, ArchiveCode, msg);
                ArcLog?.WriteError(msg);
            }
        }
    }
}
