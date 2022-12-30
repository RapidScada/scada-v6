// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Models;
using Scada.MultiDb;
using Scada.Server.Modules.ModDbExport.Config;
using System.Data.Common;

namespace Scada.Server.Modules.ModDbExport.Logic.Queries
{
    /// <summary>
    /// Represents a query for exporting events.
    /// <para>Представляет запрос для экспорта событий.</para>
    /// </summary>
    internal class EventQuery : Query
    {
        /// <summary>
        /// Represents query parameters.
        /// </summary>
        public class QueryParameters
        {
            public DbParameter EventID { get; init; }
            public DbParameter Timestamp { get; init; }
            public DbParameter Hidden { get; init; }
            public DbParameter CnlNum { get; init; }
            public DbParameter ObjNum { get; init; }
            public DbParameter DeviceNum { get; init; }
            public DbParameter PrevCnlVal { get; init; }
            public DbParameter PrevCnlStat { get; init; }
            public DbParameter CnlVal { get; init; }
            public DbParameter CnlStat { get; init; }
            public DbParameter Severity { get; init; }
            public DbParameter AckRequired { get; init; }
            public DbParameter Ack { get; init; }
            public DbParameter AckTimestamp { get; init; }
            public DbParameter AckUserID { get; init; }
            public DbParameter TextFormat { get; init; }
            public DbParameter EventText { get; init; }
            public DbParameter EventData { get; init; }
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EventQuery(int queryID, QueryOptions queryOptions, DataSource dataSource)
            : base(queryID, queryOptions, dataSource)
        {
            Parameters = new QueryParameters
            {
                EventID = DataSource.SetParam(Command, "eventID", 0L),
                Timestamp = DataSource.SetParam(Command, "timestamp", DateTime.MinValue),
                Hidden = DataSource.SetParam(Command, "hidden", 0),
                CnlNum = DataSource.SetParam(Command, "cnlNum", 0),
                ObjNum = DataSource.SetParam(Command, "objNum", 0),
                DeviceNum = DataSource.SetParam(Command, "deviceNum", 0),
                PrevCnlVal = DataSource.SetParam(Command, "prevCnlVal", 0.0),
                PrevCnlStat = DataSource.SetParam(Command, "prevCnlStat", 0),
                CnlVal = DataSource.SetParam(Command, "cnlVal", 0.0),
                CnlStat = DataSource.SetParam(Command, "cnlStat", 0),
                Severity = DataSource.SetParam(Command, "severity", 0),
                AckRequired = DataSource.SetParam(Command, "ackRequired", 0),
                Ack = DataSource.SetParam(Command, "ack", 0),
                AckTimestamp = DataSource.SetParam(Command, "ackTimestamp", DateTime.MinValue),
                AckUserID = DataSource.SetParam(Command, "ackUserID", 0),
                TextFormat = DataSource.SetParam(Command, "textFormat", 0),
                EventText = DataSource.SetParam(Command, "eventText", ""),
                EventData = DataSource.SetParam(Command, "eventData", Array.Empty<byte>())
            };
        }

        /// <summary>
        /// Gets the query parameters.
        /// </summary>
        public QueryParameters Parameters { get; }

        /// <summary>
        /// Sets the query parameters according the specified event.
        /// </summary>
        public void SetParameters(Event ev)
        {
            ArgumentNullException.ThrowIfNull(ev, nameof(ev));
            Parameters.EventID.Value = ev.EventID;
            Parameters.Timestamp.Value = ev.Timestamp;
            Parameters.Hidden.Value = ev.Hidden ? 1 : 0;
            Parameters.CnlNum.Value = ev.CnlNum;
            Parameters.ObjNum.Value = ev.ObjNum;
            Parameters.DeviceNum.Value = ev.DeviceNum;
            Parameters.PrevCnlVal.Value = ev.PrevCnlVal;
            Parameters.PrevCnlStat.Value = ev.PrevCnlStat;
            Parameters.CnlVal.Value = ev.CnlVal;
            Parameters.CnlStat.Value = ev.CnlStat;
            Parameters.Severity.Value = ev.Severity;
            Parameters.AckRequired.Value = ev.AckRequired ? 1 : 0;
            Parameters.Ack.Value = ev.Ack ? 1 : 0;
            Parameters.AckTimestamp.Value = ev.AckTimestamp;
            Parameters.AckUserID.Value = ev.AckUserID;
            Parameters.TextFormat.Value = (int)ev.TextFormat;
            Parameters.EventText.Value = string.IsNullOrEmpty(ev.Text) ? DBNull.Value : ev.Text;
            Parameters.EventData.Value = ev.Data == null || ev.Data.Length == 0 ? DBNull.Value : ev.Data;
        }
    }
}
