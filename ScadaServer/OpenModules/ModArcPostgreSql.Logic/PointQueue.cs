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
    /// Represents a queue for writing data points to a database.
    /// <para>Представляет очередь для записи точек данных в базу данных.</para>
    /// </summary>
    internal class PointQueue : IDataQueue
    {
        private readonly Queue<CnlDataPoint> dataQueue;  // contains data points for writing
        private readonly NpgsqlCommand command;          // writes a data point
        private readonly NpgsqlParameter cnlNumParam;    // the channel number parameter
        private readonly NpgsqlParameter timestampParam; // the timestamp parameter
        private readonly NpgsqlParameter valParam;       // the channel value parameter
        private readonly NpgsqlParameter statParam;      // the channel status parameter


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PointQueue(int maxQueueSize, string insertSql)
        {
            dataQueue = new Queue<CnlDataPoint>(maxQueueSize);

            command = new NpgsqlCommand(insertSql);
            cnlNumParam = command.Parameters.Add("cnlNum", NpgsqlDbType.Integer);
            timestampParam = command.Parameters.Add("timestamp", NpgsqlDbType.TimestampTz);
            valParam = command.Parameters.Add("val", NpgsqlDbType.Double);
            statParam = command.Parameters.Add("stat", NpgsqlDbType.Integer);

            MaxQueueSize = maxQueueSize;
            HasError = false;
            ReturnOnError = false;
        }


        /// <summary>
        /// Gets the maximum queue size.
        /// </summary>
        public int MaxQueueSize { get; }

        /// <summary>
        /// Gets a value indicating whether to return points to the queue in case of error.
        /// </summary>
        public bool ReturnOnError { get; set; }

        /// <summary>
        /// Gets the current queue size.
        /// </summary>
        public int Count
        {
            get
            {
                return dataQueue.Count;
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
        /// Sets the command parameters according to the data point.
        /// </summary>
        private void SetCommandParams(CnlDataPoint point)
        {
            cnlNumParam.Value = point.CnlNum;
            timestampParam.Value = point.Timestamp;
            valParam.Value = point.Val;
            statParam.Value = point.Stat;
        }

        /// <summary>
        /// Enqueues a data point to the queue.
        /// </summary>
        public void EnqueuePoint(int cnlNum, DateTime timestamp, CnlData cnlData)
        {
            lock (SyncRoot)
            {
                dataQueue.Enqueue(new CnlDataPoint(cnlNum, timestamp, cnlData));
            }
        }

        /// <summary>
        /// Enqueues a data point without locking the queue.
        /// </summary>
        public void EnqueueWithoutLock(int cnlNum, DateTime timestamp, CnlData cnlData)
        {
            dataQueue.Enqueue(new CnlDataPoint(cnlNum, timestamp, cnlData));
        }

        /// <summary>
        /// Inserts or updates data points from the queue.
        /// </summary>
        public void InsertPoints()
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
                    // retrieve a data point from the queue
                    CnlDataPoint point;

                    lock (SyncRoot)
                    {
                        if (dataQueue.Count > 0)
                            point = dataQueue.Dequeue();
                        else
                            break;
                    }

                    try
                    {
                        // write the data point
                        SetCommandParams(point);
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        // return the unwritten data point to the queue
                        if (ReturnOnError)
                        {
                            lock (SyncRoot)
                            {
                                dataQueue.Enqueue(point);
                            }
                        }

                        throw;
                    }
                }

                if (dataQueue.Count == 0)
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
        /// Writes all data points from the queue.
        /// </summary>
        public void FlushPoints()
        {
            if (Connection == null)
                throw new InvalidOperationException("Connection must not be null.");

            if (dataQueue.Count == 0)
                return;

            NpgsqlTransaction trans = null;

            try
            {
                Connection.Open();
                trans = Connection.BeginTransaction();
                command.Connection = Connection;
                command.Transaction = trans;

                lock (SyncRoot)
                {
                    while (dataQueue.Count > 0)
                    {
                        SetCommandParams(dataQueue.Dequeue());
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
        /// Removes excess points from the head of the queue.
        /// </summary>
        public void RemoveExcessPoints()
        {
            int lostCnt = 0;

            lock (SyncRoot)
            {
                while (dataQueue.Count > MaxQueueSize)
                {
                    dataQueue.Dequeue();
                    lostCnt++;
                }
            }

            if (lostCnt > 0)
            {
                string msg = string.Format(ServerPhrases.PointsWereLost, lostCnt);
                AppLog?.WriteError(ServerPhrases.ArchiveMessage, ArchiveCode, msg);
                ArcLog?.WriteError(msg);
            }
        }
    }
}
