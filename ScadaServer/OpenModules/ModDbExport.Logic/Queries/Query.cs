// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.MultiDb;
using Scada.Server.Modules.ModDbExport.Config;
using System.Data.Common;

namespace Scada.Server.Modules.ModDbExport.Logic.Queries
{
    /// <summary>
    /// Represents a query.
    /// <para>Представляет запрос.</para>
    /// </summary>
    internal abstract class Query
    {
        /// <summary>
        /// Represents a query filter for runtime.
        /// </summary>
        public class QueryFilterRuntime
        {
            public QueryFilterRuntime(QueryFilter queryFilter) 
            {
                ArgumentNullException.ThrowIfNull(queryFilter, nameof(queryFilter));
                Enabled = false;
                CnlNums = new HashSet<int>(queryFilter.CnlNums);
                DeviceNums = new HashSet<int>(queryFilter.DeviceNums);
                ObjNums = new HashSet<int>(queryFilter.ObjNums);
            }

            public bool Enabled { get; init; }
            public HashSet<int> CnlNums { get; }
            public HashSet<int> DeviceNums { get; }
            public HashSet<int> ObjNums { get; }
        }


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Query(QueryOptions queryOptions, DataSource dataSource)
        {
            Options = queryOptions ?? throw new ArgumentNullException(nameof(queryOptions));
            DataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
            Filter = new QueryFilterRuntime(queryOptions.Filter) { Enabled = queryOptions.FilterApplicable };
            Command = dataSource.CreateCommand();
            Command.CommandText = queryOptions.Sql;
        }


        /// <summary>
        /// Gets the query options.
        /// </summary>
        public QueryOptions Options { get; }

        /// <summary>
        /// Gets the data source.
        /// </summary>
        public DataSource DataSource { get; }

        /// <summary>
        /// Gets the query filter.
        /// </summary>
        public QueryFilterRuntime Filter { get; }

        /// <summary>
        /// Gets the database command corresponding to the query.
        /// </summary>
        public DbCommand Command { get; }
    }
}
