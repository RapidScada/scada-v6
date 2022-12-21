// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
            public DbParameter Text { get; init; }
            public DbParameter Data { get; init; }
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EventQuery(QueryOptions queryOptions, DataSource dataSource)
            : base(queryOptions, dataSource)
        {
            Parameters = new()
            {
                EventID = DataSource.SetParam(Command, "eventID", 0),
                Timestamp = DataSource.SetParam(Command, "timestamp", DateTime.MinValue),
                Hidden = DataSource.SetParam(Command, "hidden", false),
                CnlNum = DataSource.SetParam(Command, "cnlNum", 0),
                ObjNum = DataSource.SetParam(Command, "objNum", 0),
                DeviceNum = DataSource.SetParam(Command, "deviceNum", 0),
                PrevCnlVal = DataSource.SetParam(Command, "prevCnlVal", 0.0),
                PrevCnlStat = DataSource.SetParam(Command, "prevCnlStat", 0),
                CnlVal = DataSource.SetParam(Command, "cnlVal", 0.0),
                CnlStat = DataSource.SetParam(Command, "cnlStat", 0),
                Severity = DataSource.SetParam(Command, "severity", 0),
                AckRequired = DataSource.SetParam(Command, "ackRequired", false),
                Ack = DataSource.SetParam(Command, "ack", false),
                AckTimestamp = DataSource.SetParam(Command, "ackTimestamp", DateTime.MinValue),
                AckUserID = DataSource.SetParam(Command, "ackUserID", 0),
                TextFormat = DataSource.SetParam(Command, "textFormat", 0),
                Text = DataSource.SetParam(Command, "text", ""),
                Data = DataSource.SetParam(Command, "data", Array.Empty<byte>())
            };
        }

        /// <summary>
        /// Gets the query parameters.
        /// </summary>
        public QueryParameters Parameters { get; }
    }
}
