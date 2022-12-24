// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Npgsql;
using NpgsqlTypes;
using Scada.Dbms;
using System.Data.Common;

namespace Scada.MultiDb
{
    /// <summary>
    /// Represents a PostgreSQL data source.
    /// <para>Представляет источник данных PostgreSQL.</para>
    /// </summary>
    public class NpgsqlDataSource : DataSource
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public NpgsqlDataSource(DbConnectionOptions connectionOptions)
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
            if (cmd is NpgsqlCommand npgsqlCommand)
            {
                return value is DateTime
                    ? npgsqlCommand.Parameters.AddWithValue(paramName, NpgsqlDbType.TimestampTz, value)
                    : npgsqlCommand.Parameters.AddWithValue(paramName, value);
            }
            else
            {
                throw new ArgumentException("NpgsqlCommand is required.", nameof(cmd));
            }
        }

        /// <summary>
        /// Clears the connection pool.
        /// </summary>
        protected override void ClearPool()
        {
            if (Connection != null)
                NpgsqlConnection.ClearPool((NpgsqlConnection)Connection);
        }
    }
}
