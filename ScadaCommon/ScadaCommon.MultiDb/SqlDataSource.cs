// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Data.SqlClient;
using Scada.Dbms;
using System.Data;
using System.Data.Common;

namespace Scada.MultiDb
{
    /// <summary>
    /// Represents a Microsoft SQL Server data source.
    /// <para>Представляет источник данных Microsoft SQL Server.</para>
    /// </summary>
    public class SqlDataSource : DataSource
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public SqlDataSource(DbConnectionOptions connectionOptions)
            : base(connectionOptions)
        {
        }


        /// <summary>
        /// Creates a database connection.
        /// </summary>
        protected override DbConnection CreateConnection()
        {
            return new SqlConnection();
        }

        /// <summary>
        /// Adds a command parameter with the specified name and value.
        /// </summary>
        protected override DbParameter AddParamWithValue(DbCommand cmd, string paramName, object value)
        {
            if (cmd is not SqlCommand sqlCommand)
                throw new ArgumentException("SqlCommand is required.", nameof(cmd));

            if (value is DateTime)
                return sqlCommand.Parameters.Add(new SqlParameter(paramName, SqlDbType.DateTime2) { Value = value });
            else if (value is byte[])
                return sqlCommand.Parameters.Add(new SqlParameter(paramName, SqlDbType.VarBinary) { Value = value });
            else
                return sqlCommand.Parameters.AddWithValue(paramName, value);
        }

        /// <summary>
        /// Clears the connection pool.
        /// </summary>
        protected override void ClearPool()
        {
            if (Connection != null)
                SqlConnection.ClearPool((SqlConnection)Connection);
        }
    }
}
