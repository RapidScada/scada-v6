// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Npgsql;
using NpgsqlTypes;
using Scada.Data.Models;
using Scada.Dbms;
using Scada.Server.Lang;

namespace Scada.Server.Modules.ModArcPostgreSql.Logic
{
    /// <summary>
    /// Represents a queue for writing data points to a database.
    /// <para>Представляет очередь для записи точек данных в базу данных.</para>
    /// </summary>
    internal class PointQueue : QueueBase<CnlDataPoint>
    {
        private readonly NpgsqlCommand command;          // writes a data point
        private readonly NpgsqlParameter cnlNumParam;    // the channel number parameter
        private readonly NpgsqlParameter timestampParam; // the timestamp parameter
        private readonly NpgsqlParameter valParam;       // the channel value parameter
        private readonly NpgsqlParameter statParam;      // the channel status parameter


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PointQueue(int maxQueueSize, int batchSize, string insertSql)
            : base(maxQueueSize, batchSize)
        {
            command = new NpgsqlCommand(insertSql);
            cnlNumParam = command.Parameters.Add("cnlNum", NpgsqlDbType.Integer);
            timestampParam = command.Parameters.Add("timestamp", NpgsqlDbType.TimestampTz);
            valParam = command.Parameters.Add("val", NpgsqlDbType.Double);
            statParam = command.Parameters.Add("stat", NpgsqlDbType.Integer);

            ReturnOnError = false;
        }


        /// <summary>
        /// Gets a value indicating whether to return points to the queue in case of error.
        /// </summary>
        public bool ReturnOnError { get; init; }


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
        /// Retrieves items from the queue and inserts or updates them in the database.
        /// </summary>
        public override bool ProcessItems()
        {
            if (Connection == null)
                throw new InvalidOperationException("Connection must not be null.");

            if (Count == 0)
                return false;

            NpgsqlTransaction trans = null;

            try
            {
                Connection.Open();
                trans = Connection.BeginTransaction();
                command.Connection = Connection;
                command.Transaction = trans;

                for (int i = 0; i < BatchSize; i++)
                {
                    // retrieve a data point from the queue
                    if (!TryDequeue(out CnlDataPoint point))
                        break;

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
                            Enqueue(point);

                        throw;
                    }
                }

                if (Count == 0)
                    ArcLog?.WriteInfo(ServerPhrases.QueueBecameEmpty);

                trans.Commit();
                LastCommitTime = DateTime.UtcNow;
                Stats.HasError = false;
                return true;
            }
            catch (Exception ex)
            {
                SilentCommitOrRollback(trans);
                Stats.HasError = true;
                AppLog?.WriteError(ex, ServerPhrases.ArchiveMessage, ArchiveCode, ServerPhrases.WriteDbError);
                ArcLog?.WriteError(ex, ServerPhrases.WriteDbError);
                Thread.Sleep(ScadaUtils.ErrorDelay);
                return false;
            }
            finally
            {
                Connection.Close();
            }
        }
    }
}
