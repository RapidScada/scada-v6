// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MySql.Data.MySqlClient;
using Scada.Dbms;
using System.Data.Common;

namespace Scada.MultiDb
{
    /// <summary>
    /// Represents a MySQL data source.
    /// <para>Представляет источник данных MySQL.</para>
    /// </summary>
    public class MySqlDataSource : DataSource
    {
        /// <summary>
        /// The default port of the database server.
        /// </summary>
        private const int DefaultPort = 3306;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public MySqlDataSource(DbConnectionOptions connectionOptions)
            : base(connectionOptions)
        {
        }


        /// <summary>
        /// Creates a database connection.
        /// </summary>
        protected override DbConnection CreateConnection()
        {
            return new MySqlConnection();
        }

        /// <summary>
        /// Adds the command parameter containing the value.
        /// </summary>
        protected override DbParameter AddParamWithValue(DbCommand cmd, string paramName, object value)
        {
            return cmd is MySqlCommand mySqlCommand
                ? mySqlCommand.Parameters.AddWithValue(paramName, value)
                : throw new ArgumentException("MySqlCommand is required.", nameof(cmd));
        }

        /// <summary>
        /// Clears the connection pool.
        /// </summary>
        protected override void ClearPool()
        {
            if (Connection != null)
                MySqlConnection.ClearPool((MySqlConnection)Connection);
        }
    }
}
