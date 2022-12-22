// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.MultiDb;
using Scada.Server.Modules.ModDbExport.Config;
using System.Data.Common;

namespace Scada.Server.Modules.ModDbExport.Logic.Queries
{
    /// <summary>
    /// Represents a query for exporting event acknowledgements.
    /// <para>Представляет запрос для экспорта квитирования событий.</para>
    /// </summary>
    internal class EventAckQuery : Query
    {
        /// <summary>
        /// Represents query parameters.
        /// </summary>
        public class QueryParameters
        {
            public DbParameter EventID { get; init; }
            public DbParameter Timestamp { get; init; }
            public DbParameter UserID { get; init; }
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EventAckQuery(int queryID, QueryOptions queryOptions, DataSource dataSource)
            : base(queryID, queryOptions, dataSource)
        {
            Parameters = new()
            {
                EventID = DataSource.SetParam(Command, "eventID", 0),
                Timestamp = DataSource.SetParam(Command, "timestamp", DateTime.MinValue),
                UserID = DataSource.SetParam(Command, "userID", 0)
            };
        }

        /// <summary>
        /// Gets the query parameters.
        /// </summary>
        public QueryParameters Parameters { get; }
    }
}
