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
    /// Represents a queue for writing events to a database.
    /// <para>Представляет очередь для записи событий в базу данных.</para>
    /// </summary>
    internal class EventQueue : QueueBase<Event>
    {
        private readonly NpgsqlCommand command; // writes an event
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
        public EventQueue(int maxQueueSize, int batchSize, string insertSql)
            : base(maxQueueSize, batchSize)
        {
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
            eventTextParam.Value = string.IsNullOrEmpty(ev.Text) ? DBNull.Value : ev.Text;
            eventDataParam.Value = ev.Data == null || ev.Data.Length == 0 ? DBNull.Value : ev.Data;
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
                    // retrieve an event from the queue
                    if (!TryDequeue(out Event ev))
                        break;

                    try
                    {
                        // write the event
                        if (ev != null)
                        {
                            SetCommandParams(ev);
                            command.ExecuteNonQuery();
                        }
                    }
                    catch
                    {
                        // return the unwritten event to the queue
                        Enqueue(ev);
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
