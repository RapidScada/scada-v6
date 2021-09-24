// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Npgsql;
using Scada.Config;
using Scada.Data.Tables;
using Scada.Lang;
using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;

namespace Scada.Storages.PostgreSqlStorage
{
    /// <summary>
    /// Represents a storage logic.
    /// <para>Представляет логику хранилища.</para>
    /// </summary>
    public class PostgreSqlStorageLogic : StorageLogic
    {
        /// <summary>
        /// The database schema.
        /// </summary>
        private const string Schema = "project";
        /// <summary>
        /// The period of attempts to connect to the database.
        /// </summary>
        private static readonly TimeSpan ConnectAttemptPeriod = TimeSpan.FromSeconds(10);

        private TimeSpan waitTimeout;            // how long to wait for connection
        private DbConnectionOptions connOptions; // the database connection options
        private NpgsqlConnection conn;           // the database connection


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PostgreSqlStorageLogic(StorageContext storageContext)
            : base(storageContext)
        {
            waitTimeout = TimeSpan.Zero;
            connOptions = null;
            conn = null;
        }


        /// <summary>
        /// Attempts to connect to the database.
        /// </summary>
        private bool CheckConnection(out string errMsg)
        {
            try
            {
                conn.Open();
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = string.Format(Locale.IsRussian ?
                    "Ошибка при соединении с БД хранилища: {0}" :
                    "Error connecting to storage database: {0}", ex.Message);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// Creates a database connection.
        /// </summary>
        public static NpgsqlConnection CreateDbConnection(DbConnectionOptions options)
        {
            return new NpgsqlConnection(
                string.IsNullOrEmpty(options.ConnectionString) ?
                new NpgsqlConnectionStringBuilder
                {
                    Host = options.Server,
                    Database = options.Database,
                    Username = options.Username,
                    Password = options.Password
                }.ToString() :
                options.ConnectionString);
        }

        /// <summary>
        /// Gets the table name according to the specified category.
        /// </summary>
        private static string GetTableName(DataCategory category)
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
        /// Loads the configuration from the XML node.
        /// </summary>
        public override void LoadConfig(XmlElement xmlElement)
        {
            base.LoadConfig(xmlElement);
            waitTimeout = TimeSpan.FromSeconds(xmlElement.GetChildAsInt("WaitTimeout"));

            if (xmlElement.SelectSingleNode("Connection") is XmlNode connectionNode)
            {
                connOptions = new DbConnectionOptions();
                connOptions.LoadFromXml(connectionNode);
            }
        }

        /// <summary>
        /// Makes the storage ready for operating.
        /// </summary>
        public override void MakeReady()
        {
            if (connOptions == null)
                throw new ScadaException(CommonPhrases.ConnOptionsNotFound);

            conn = CreateDbConnection(connOptions);

            // wait for connection
            DateTime utcNow = DateTime.UtcNow;
            DateTime startDT = utcNow;
            DateTime attempDT = DateTime.MinValue;

            while (utcNow - startDT >= waitTimeout)
            {
                if (utcNow - attempDT >= ConnectAttemptPeriod)
                {
                    attempDT = utcNow;

                    if (CheckConnection(out string errMsg))
                        break;
                    else
                        StorageContext.Log.WriteError(errMsg);
                }

                Thread.Sleep(ScadaUtils.ThreadDelay);
                utcNow = DateTime.UtcNow;
            }
        }

        /// <summary>
        /// Closes the storage.
        /// </summary>
        public override void Close()
        {
            if (conn != null)
            {
                conn.Dispose();
                conn = null;
            }
        }

        /// <summary>
        /// Reads text from the file.
        /// </summary>
        public override string ReadText(DataCategory category, string path)
        {
            string sql = 
                $"SELECT contents FROM {GetTableName(category)} " +
                "WHERE " + (category == DataCategory.View ? "" : "app_id = @appID AND ") +
                "path = @path LIMIT 1";

            NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("path", path);

            if (category != DataCategory.View)
                cmd.Parameters.AddWithValue("appID", (int)App);

            try
            {
                Monitor.Enter(conn);
                conn.Open();

                using (NpgsqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (reader.Read())
                        return reader.IsDBNull(0) ? "" : reader.GetString(0);
                }
            }
            finally
            {
                conn.Close();
                Monitor.Exit(conn);
            }

            throw new FileNotFoundException(string.Format(CommonPhrases.NamedFileNotFound, path));
        }

        /// <summary>
        /// Reads a byte array from the file.
        /// </summary>
        public override byte[] ReadBytes(DataCategory category, string path)
        {
            string contents = ReadText(category, path);
            return Convert.FromBase64String(contents);
        }

        /// <summary>
        /// Reads the table of the configuration database.
        /// </summary>
        public override void ReadBaseTable(IBaseTable baseTable)
        {
            try
            {
                Monitor.Enter(conn);
                conn.Open();

                string sql = $"SELECT * from {Schema}.\"{baseTable.Name.ToLowerInvariant()}\"";
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
                            try { colIdxs[i] = reader.GetOrdinal(props[i].Name.ToLowerInvariant()); }
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
            finally
            {
                conn.Close();
                Monitor.Exit(conn);
            }
        }

        /// <summary>
        /// Writes the text to the file.
        /// </summary>
        public override void WriteText(DataCategory category, string path, string contents)
        {
        }

        /// <summary>
        /// Writes the byte array to the file.
        /// </summary>
        public override void WriteBytes(DataCategory category, string path, byte[] bytes)
        {
            string contents = Convert.ToBase64String(bytes);
            WriteText(category, path, contents);
        }

        /// <summary>
        /// Opens a text file for reading.
        /// </summary>
        public override TextReader OpenText(DataCategory category, string path)
        {
            string contents = ReadText(category, path);
            return new StringReader(contents);
        }

        /// <summary>
        /// Opens an existing file for reading.
        /// </summary>
        public override Stream OpenRead(DataCategory category, string path)
        {
            byte[] bytes = ReadBytes(category, path);
            return new MemoryStream(bytes, false);
        }

        /// <summary>
        /// Gets information associated with the file.
        /// </summary>
        public override ShortFileInfo GetFileInfo(DataCategory category, string path)
        {
            string sql =
                $"SELECT length(contents), write_time FROM {GetTableName(category)} " +
                "WHERE " + (category == DataCategory.View ? "" : "app_id = @appID AND ") +
                "path = @path LIMIT 1";

            NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("path", path);

            if (category != DataCategory.View)
                cmd.Parameters.AddWithValue("appID", (int)App);

            try
            {
                Monitor.Enter(conn);
                conn.Open();

                using (NpgsqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (reader.Read())
                    {
                        return new ShortFileInfo
                        {
                            Exists = true,
                            LastWriteTime = reader.IsDBNull(1)
                                ? DateTime.MinValue
                                : reader.GetDateTime(1).ToUniversalTime(),
                            Length = reader.GetInt32(0)
                        };
                    }
                }
            }
            finally
            {
                conn.Close();
                Monitor.Exit(conn);
            }

            return ShortFileInfo.FileNotExists;
        }
    }
}
