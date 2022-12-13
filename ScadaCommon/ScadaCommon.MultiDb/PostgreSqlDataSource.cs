// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Npgsql;
using Scada.Dbms;
using System.Data.Common;

namespace Scada.MultiDb
{
    /// <summary>
    /// Represents a PostgreSQL data source.
    /// <para>Представляет источник данных PostgreSQL.</para>
    /// </summary>
    public class PostgreSqlDataSource : DataSource
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PostgreSqlDataSource(DbConnectionOptions connectionOptions)
            : base(connectionOptions)
        {
        }


        /// <summary>
        /// Creates a database connection.
        /// </summary>
        protected override DbConnection CreateConnection()
        {
            return new NpgsqlConnection();
        }

        /// <summary>
        /// Adds a command parameter with the specified name and value.
        /// </summary>
        protected override DbParameter AddParamWithValue(DbCommand cmd, string paramName, object value)
        {
            ArgumentNullException.ThrowIfNull(cmd, nameof(cmd));
            ArgumentNullException.ThrowIfNull(paramName, nameof(paramName));

            return cmd is NpgsqlCommand npgsqlCommand 
                ? npgsqlCommand.Parameters.AddWithValue(paramName, value) 
                : throw new ArgumentException("NpgsqlCommand is required.", nameof(cmd));
        }

        /// <summary>
        /// Clears the connection pool.
        /// </summary>
        protected override void ClearPool()
        {
            NpgsqlConnection.ClearAllPools();
        }
    }
}
