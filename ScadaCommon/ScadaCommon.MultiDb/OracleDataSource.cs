// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Oracle.ManagedDataAccess.Client;
using Scada.Dbms;
using System.Data.Common;

namespace Scada.MultiDb
{
    /// <summary>
    /// Represents an Oracle data source.
    /// <para>Представляет источник данных Oracle.</para>
    /// </summary>
    public class OracleDataSource : DataSource
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public OracleDataSource(DbConnectionOptions connectionOptions)
            : base(connectionOptions)
        {
        }


        /// <summary>
        /// Creates a database connection.
        /// </summary>
        protected override DbConnection CreateConnection()
        {
            return new OracleConnection();
        }

        /// <summary>
        /// Adds the command parameter containing the value.
        /// </summary>
        protected override DbParameter AddParamWithValue(DbCommand cmd, string paramName, object value)
        {
            return cmd is OracleCommand oracleCommand 
                ? oracleCommand.Parameters.Add(paramName, value) 
                : throw new ArgumentException("OracleCommand is required.", nameof(cmd));
        }

        /// <summary>
        /// Clears the connection pool.
        /// </summary>
        protected override void ClearPool()
        {
            if (Connection != null)
                OracleConnection.ClearPool((OracleConnection)Connection);
        }
    }
}
