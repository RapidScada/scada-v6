// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Dbms;
using System.Data.Common;
using System.Data.OleDb;

namespace Scada.MultiDb
{
    /// <summary>
    /// Implements a data source for OLE DB.
    /// <para>Реализует источник данных для OLE DB.</para>
    /// </summary>
    public class OleDbDataSource : DataSource
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public OleDbDataSource(DbConnectionOptions connectionOptions)
            : base(connectionOptions)
        {
        }


        /// <summary>
        /// Creates a database connection.
        /// </summary>
        protected override DbConnection CreateConnection()
        {
            return new OleDbConnection();
        }

        /// <summary>
        /// Adds a command parameter with the specified name and value.
        /// </summary>
        protected override DbParameter AddParamWithValue(DbCommand cmd, string paramName, object value)
        {
            if (cmd == null)
                throw new ArgumentNullException(nameof(cmd));

            return cmd is OleDbCommand oleDbCmd ?
                oleDbCmd.Parameters.AddWithValue(paramName, value) :
                throw new ArgumentException("OleDbCommand is required.", nameof(cmd));
        }

        /// <summary>
        /// Clears the connection pool.
        /// </summary>
        protected override void ClearPool()
        {
            // do nothing
        }
    }
}
