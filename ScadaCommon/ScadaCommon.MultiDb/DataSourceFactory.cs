// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Dbms;

namespace Scada.MultiDb
{
    /// <summary>
    /// Creates data sources.
    /// <para>Создает источники данных.</para>
    /// </summary>
    public static class DataSourceFactory
    {
        /// <summary>
        /// Gets a new data source.
        /// </summary>
        public static DataSource GetDataSource(DbConnectionOptions connectionOptions)
        {
            ArgumentNullException.ThrowIfNull(connectionOptions, nameof(connectionOptions));

            return connectionOptions.KnownDBMS switch
            {
                KnownDBMS.MSSQL => new SqlDataSource(connectionOptions),
                KnownDBMS.MySQL => new MySqlDataSource(connectionOptions),
                KnownDBMS.Oracle => new OracleDataSource(connectionOptions),
                KnownDBMS.PostgreSQL => new NpgsqlDataSource(connectionOptions),
                _ => throw new ScadaException("Unknown DBMS."),
            };
        }
    }
}
