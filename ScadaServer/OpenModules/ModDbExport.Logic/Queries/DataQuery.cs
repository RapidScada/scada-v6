// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.MultiDb;
using Scada.Server.Modules.ModDbExport.Config;
using System.Data.Common;

namespace Scada.Server.Modules.ModDbExport.Logic.Queries
{
    /// <summary>
    /// Represents a query for exporting current and historical data.
    /// <para>Представляет запрос для экспорта текущих и исторических данных.</para>
    /// </summary>
    internal class DataQuery : Query
    {
        /// <summary>
        /// Represents query parameters.
        /// </summary>
        public class QueryParameters
        {
            public DbParameter Timestamp { get; init; }
            public DbParameter CnlNum { get; init; }
            public DbParameter DeviceNum { get; init; }
            public DbParameter Val { get; init; }
            public DbParameter Stat { get; init; }
        }


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DataQuery(QueryOptions queryOptions, DataSource dataSource)
            : base(queryOptions, dataSource)
        {
            Parameters = new()
            {
                Timestamp = DataSource.SetParam(Command, "timestamp", DateTime.MinValue),
                CnlNum = DataSource.SetParam(Command, "cnlNum", 0),
                DeviceNum = DataSource.SetParam(Command, "deviceNum", 0),
                Val = DataSource.SetParam(Command, "val", 0.0),
                Stat = DataSource.SetParam(Command, "stat", 0)
            };
        }


        /// <summary>
        /// Gets the query parameters.
        /// </summary>
        public QueryParameters Parameters { get; }


        /// <summary>
        /// Sets the command parameter that represents a value of the specified channel.
        /// </summary>
        public void SetValParam(int cnlNum, double val)
        {
            DataSource.SetParam(Command, "val" + cnlNum, val);
        }

        /// <summary>
        /// Sets the command parameter that represents a status of the specified channel.
        /// </summary>
        public void SetStatParam(int cnlNum, int stat)
        {
            DataSource.SetParam(Command, "stat" + cnlNum, stat);
        }
    }
}
