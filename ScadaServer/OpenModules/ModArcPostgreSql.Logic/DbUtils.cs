// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Npgsql;
using Scada.Config;
using Scada.Dbms;
using Scada.Lang;
using Scada.Server.Modules.ModArcPostgreSql.Config;
using System.Diagnostics;
using System.Globalization;
using System.Xml;

namespace Scada.Server.Modules.ModArcPostgreSql.Logic
{
    /// <summary>
    /// The class provides helper methods for accessing a database.
    /// <para>Класс, предоставляющий вспомогательные методы для доступа к базе данных.</para>
    /// </summary>
    internal static class DbUtils
    {
        /// <summary>
        /// The code of the storage that contains connection options.
        /// </summary>
        private const string StorageCode = "PostgreSqlStorage";
        /// <summary>
        /// The date format used for naming partitions.
        /// </summary>
        private const string PartitionDateFormat = "yyyyMMdd";
        /// <summary>
        /// The database schema.
        /// </summary>
        public const string Schema = "mod_arc_postgre_sql";
        /// <summary>
        /// The minimum queue size.
        /// </summary>
        public const int MinQueueSize = 100;
        /// <summary>
        /// The number of queue items transferred in a single loop iteration.
        /// </summary>
        public const int BundleSize = 100;
        /// <summary>
        /// The delay in case of a database error, in milliseconds.
        /// </summary>
        public const int ErrorDelay = 1000;


        /// <summary>
        /// Converts the specified substring of a partition name to a date.
        /// </summary>
        private static bool ParsePartitionDate(string s, out DateTime result)
        {
            return DateTime.TryParseExact(s, PartitionDateFormat,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
        }

        /// <summary>
        /// Gets the connection options from the module configuration.
        /// </summary>
        public static DbConnectionOptions GetConnectionOptions(ModuleConfig moduleConfig, string connName)
        {
            ArgumentNullException.ThrowIfNull(moduleConfig, nameof(moduleConfig));
            ArgumentNullException.ThrowIfNull(connName, nameof(connName));

            return moduleConfig.Connections.TryGetValue(connName, out DbConnectionOptions connOptions)
                ? connOptions
                : throw new ScadaException(CommonPhrases.ConnectionNotFound, connName);
        }

        /// <summary>
        /// Gets the connection options from the instance configuration.
        /// </summary>
        public static DbConnectionOptions GetConnectionOptions(InstanceConfig instanceConfig)
        {
            ArgumentNullException.ThrowIfNull(instanceConfig, nameof(instanceConfig));

            if (instanceConfig.Storages.TryGetValue(StorageCode, out XmlElement storageElem) &&
                storageElem.SelectSingleNode("Connection") is XmlNode connectionNode)
            {
                DbConnectionOptions connOptions = new();
                connOptions.LoadFromXml(connectionNode);
                return connOptions;
            }

            throw new ScadaException(CommonPhrases.ConnOptionsNotFound);
        }

        /// <summary>
        /// Creates a database connection.
        /// </summary>
        public static NpgsqlConnection CreateDbConnection(DbConnectionOptions options)
        {
            ArgumentNullException.ThrowIfNull(options, nameof(options));
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
        /// Creates a necessary partition if it does not exist.
        /// </summary>
        public static void CreatePartition(NpgsqlConnection conn, string tableName, 
            DateTime today, PartitionSize partitionSize, out string partitionName)
        {
            ArgumentNullException.ThrowIfNull(conn, nameof(conn));
            ArgumentNullException.ThrowIfNull(tableName, nameof(tableName));

            DateTime startDate;
            DateTime endDate;

            if (partitionSize == PartitionSize.OneMonth)
            {
                startDate = new DateTime(today.Year, today.Month, 1, 0, 0, 0, DateTimeKind.Utc);
                endDate = startDate.AddMonths(1);
            }
            else // PartitionSize.OneYear
            {
                startDate = new DateTime(today.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                endDate = startDate.AddYears(1);
            }

            partitionName = tableName +
                "_" + startDate.ToString(PartitionDateFormat) +
                "_" + endDate.ToString(PartitionDateFormat);

            new NpgsqlCommand(
                $"CREATE TABLE IF NOT EXISTS {partitionName} PARTITION OF {tableName} " +
                $"FOR VALUES FROM('{startDate:yyyy-MM-dd} 00:00:00Z') TO ('{endDate:yyyy-MM-dd} 00:00:00Z')",
                conn).ExecuteNonQuery();
        }

        /// <summary>
        /// Gets the names of the outdated partitions.
        /// </summary>
        public static List<string> GetOutdatedPartitions(NpgsqlConnection conn, string tableName, DateTime minDT)
        {
            ArgumentNullException.ThrowIfNull(conn, nameof(conn));
            ArgumentNullException.ThrowIfNull(tableName, nameof(tableName));

            string sql = "SELECT inhrelid::regclass::varchar AS child FROM pg_catalog.pg_inherits " +
                $"WHERE inhparent = '{tableName}'::regclass";
            NpgsqlCommand cmd = new(sql, conn);
            List<string> partitionsToDelete = new();

            using (NpgsqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string partitionName = reader.GetString(0);
                    int endDateIndex = partitionName.LastIndexOf('_');

                    if (partitionName.StartsWith(tableName) && endDateIndex > 0 &&
                        ParsePartitionDate(partitionName[(endDateIndex + 1)..], out DateTime endDate) &&
                        endDate < minDT)
                    {
                        partitionsToDelete.Add(partitionName);
                    }
                }
            }

            return partitionsToDelete;
        }

        /// <summary>
        /// Gets the time (UTC) when the archive was last written to.
        /// </summary>
        public static DateTime GetLastWriteTime(NpgsqlConnection conn, string tableName)
        {
            ArgumentNullException.ThrowIfNull(conn, nameof(conn));
            ArgumentNullException.ThrowIfNull(tableName, nameof(tableName));

            string sql = "SELECT MAX(time_stamp) FROM " + tableName;
            NpgsqlCommand cmd = new(sql, conn);
            object timestampObj = cmd.ExecuteScalar();
            return timestampObj is DateTime dateTime
                ? dateTime.ToUniversalTime()
                : DateTime.MinValue;
        }

        /// <summary>
        /// Rollbacks the transaction silently.
        /// </summary>
        public static void SilentClose(this NpgsqlConnection conn)
        {
            if (conn != null)
            {
                try
                {
                    conn.Close();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }

        /// <summary>
        /// Rollbacks the transaction silently.
        /// </summary>
        public static void SilentRollback(this NpgsqlTransaction trans)
        {
            if (trans != null)
            {
                try
                {
                    trans.Rollback();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }

        /// <summary>
        /// Gets the value of the specified column as a universal time.
        /// </summary>
        public static DateTime GetDateTimeUtc(this NpgsqlDataReader reader, int columnIndex)
        {
            return reader.GetDateTime(columnIndex).ToUniversalTime();
        }

        /// <summary>
        /// Gets the archive status as text.
        /// </summary>
        public static string GetStatusText(bool isReady, bool hasError, QueueBase queue)
        {
            if (isReady)
            {
                hasError |= queue != null && queue.HasError;
                string readyText = Locale.IsRussian
                    ? (hasError ? "ошибка" : "готовность")
                    : (hasError ? "Error" : "Ready");

                if (queue == null)
                {
                    return Locale.IsRussian
                        ? readyText + ", только чтение"
                        : readyText + ". Read only";
                }
                else
                {
                    return Locale.IsRussian 
                        ? readyText + $", заполнение очереди {queue.Count}/{queue.MaxQueueSize}" 
                        : readyText + $". Queue fullness is {queue.Count}/{queue.MaxQueueSize}";
                }
            }
            else
            {
                return Locale.IsRussian 
                    ? "не готов" 
                    : "Not Ready";
            }
        }
    }
}
