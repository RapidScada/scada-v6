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
        /// Initializes a new instance of the class.
        /// </summary>
        public Query(QueryOptions queryOptions, DataSource dataSource)
        {
            Options = queryOptions ?? throw new ArgumentNullException(nameof(queryOptions));
            DataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));

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
        /// Gets the database command corresponding to the query.
        /// </summary>
        public DbCommand Command { get; }
    }
}
