// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Dbms;
using System.Data.Common;

namespace Scada.MultiDb
{
    /// <summary>
    /// Represents the base class of the data source.
    /// <para>Представляет базовый класс источника данных.</para>
    /// </summary>
    public abstract class DataSource
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DataSource(DbConnectionOptions connectionOptions)
        {
            ConnectionOptions = connectionOptions ?? throw new ArgumentNullException(nameof(connectionOptions));
            Connection = CreateConnection();
            Connection.ConnectionString = string.IsNullOrEmpty(connectionOptions.ConnectionString)
                ? BuildConnectionString()
                : connectionOptions.ConnectionString;
        }


        /// <summary>
        /// Gets the database connection options.
        /// </summary>
        public DbConnectionOptions ConnectionOptions { get; }

        /// <summary>
        /// Gets the database connection.
        /// </summary>
        public DbConnection Connection { get; }


        /// <summary>
        /// Creates a database connection.
        /// </summary>
        protected abstract DbConnection CreateConnection();

        /// <summary>
        /// Adds a command parameter with the specified name and value.
        /// </summary>
        protected abstract DbParameter AddParamWithValue(DbCommand cmd, string paramName, object value);

        /// <summary>
        /// Clears the connection pool.
        /// </summary>
        protected abstract void ClearPool();


        /// <summary>
        /// Connects to the database.
        /// </summary>
        public virtual void Connect()
        {
            if (Connection == null)
                throw new InvalidOperationException("Connection is not initialized.");

            try
            {
                Connection.Open();
            }
            catch
            {
                Connection.Close();
                ClearPool();
                throw;
            }
        }

        /// <summary>
        /// Disconnects from the database.
        /// </summary>
        public virtual void Disconnect()
        {
            Connection?.Close();
        }

        /// <summary>
        /// Builds a connection string based on the specified connection settings.
        /// </summary>
        public virtual string BuildConnectionString()
        {
            return ConnectionStringBuilder.Build(ConnectionOptions, false);
        }

        /// <summary>
        /// Creates a command.
        /// </summary>
        public virtual DbCommand CreateCommand()
        {
            return Connection.CreateCommand();
        }

        /// <summary>
        /// Creates a command with the specified command text.
        /// </summary>
        public virtual DbCommand CreateCommand(string commandText)
        {
            DbCommand command = CreateCommand();
            command.CommandText = commandText;
            return command;
        }

        /// <summary>
        /// Sets the command parameter.
        /// </summary>
        public virtual DbParameter SetParam(DbCommand cmd, string paramName, object value)
        {
            ArgumentNullException.ThrowIfNull(cmd, nameof(cmd));

            if (cmd.Parameters.Contains(paramName))
            {
                DbParameter param = cmd.Parameters[paramName];
                param.Value = value;
                return param;
            }
            else
            {
                return AddParamWithValue(cmd, paramName, value);
            }
        }
    }
}
