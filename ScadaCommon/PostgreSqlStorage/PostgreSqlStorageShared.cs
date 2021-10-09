// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Npgsql;
using Scada.Config;
using Scada.Data.Tables;
using Scada.Lang;
using System.ComponentModel;

namespace Scada.Storages.PostgreSqlStorage
{
    /// <summary>
    /// The class provides helper methods for PostgreSQL storage shared with other assemblies.
    /// <para>Класс предоставляет вспомогательные методы для хранилища PostgreSQL, используемые другими сборками.</para>
    /// </summary>
    internal static class PostgreSqlStorageShared
    {
        /// <summary>
        /// The database schema.
        /// </summary>
        public const string Schema = "project";


        /// <summary>
        /// Creates a database connection.
        /// </summary>
        public static NpgsqlConnection CreateDbConnection(DbConnectionOptions options)
        {
            string connectionString = options.ConnectionString;

            if (string.IsNullOrEmpty(connectionString))
            {
                ScadaUtils.RetrieveHostAndPort(options.Server, NpgsqlConnection.DefaultPort,
                    out string host, out int port);

                connectionString = new NpgsqlConnectionStringBuilder
                {
                    Host = host,
                    Port = port,
                    Database = options.Database,
                    Username = options.Username,
                    Password = options.Password
                }
                .ToString();
            }

            return new NpgsqlConnection(connectionString);
        }

        /// <summary>
        /// Gets the table name according to the specified category.
        /// </summary>
        public static string GetTableName(DataCategory category)
        {
            switch (category)
            {
                case DataCategory.Config:
                    return Schema + ".app_config";

                case DataCategory.Storage:
                    return Schema + ".app_storage";

                case DataCategory.View:
                    return Schema + ".view_file";

                default:
                    throw new ScadaException("Data category not supported.");
            }
        }

        /// <summary>
        /// Gets the name of the configuration database table.
        /// </summary>
        public static string GetBaseTableName(IBaseTable baseTable)
        {
            return $"{Schema}.\"{baseTable.Name.ToLowerInvariant()}\"";
        }

        /// <summary>
        /// Gets the column name of the configuration database table.
        /// </summary>
        public static string GetBaseColumnName(string s)
        {
            return '"' + s.ToLowerInvariant() + '"';
        }

        /// <summary>
        /// Gets the column name of the configuration database table.
        /// </summary>
        public static string GetBaseColumnName(PropertyDescriptor prop)
        {
            return GetBaseColumnName(prop.Name);
        }

        /// <summary>
        /// Reads the configuration database table.
        /// </summary>
        public static void ReadBaseTable(IBaseTable baseTable, NpgsqlConnection conn)
        {
            string sql = "SELECT * from " + GetBaseTableName(baseTable);
            NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);

            using (NpgsqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    // check primary key column
                    try
                    {
                        reader.GetOrdinal(baseTable.PrimaryKey.ToLowerInvariant());
                    }
                    catch
                    {
                        throw new ScadaException(Locale.IsRussian ?
                            "Первичный ключ \"{0}\" не найден" :
                            "Primary key \"{0}\" not found", baseTable.PrimaryKey);
                    }

                    // find column indexes
                    PropertyDescriptorCollection props = TypeDescriptor.GetProperties(baseTable.ItemType);
                    int propCnt = props.Count;
                    int[] colIdxs = new int[propCnt];

                    for (int i = 0; i < propCnt; i++)
                    {
                        try { colIdxs[i] = reader.GetOrdinal(GetBaseColumnName(props[i])); }
                        catch { colIdxs[i] = -1; }
                    }

                    // read rows
                    while (reader.Read())
                    {
                        object item = baseTable.NewItem();

                        for (int i = 0; i < propCnt; i++)
                        {
                            int colIdx = colIdxs[i];

                            if (colIdx >= 0 && !reader.IsDBNull(colIdx))
                                props[i].SetValue(item, reader[colIdx]);
                        }

                        baseTable.AddObject(item);
                    }
                }
            }
        }
    }
}
