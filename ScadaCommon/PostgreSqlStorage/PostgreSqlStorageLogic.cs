// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Npgsql;
using Scada.Config;
using Scada.Data.Tables;
using Scada.Lang;
using System;
using System.Data;
using System.IO;
using System.Text;
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

        private DbConnectionOptions connOptions; // the database connection options
        private NpgsqlConnection conn;           // the database connection


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PostgreSqlStorageLogic(StorageContext storageContext)
            : base(storageContext)
        {
            conn = null;
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

            // TODO: check DB and wait
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
        /// Opens a binary file for reading.
        /// </summary>
        public override BinaryReader OpenBinary(DataCategory category, string path)
        {
            byte[] bytes = ReadBytes(category, path);
            return new BinaryReader(new MemoryStream(bytes, false), Encoding.UTF8, false);
        }

        /// <summary>
        /// Gets information associated with the file.
        /// </summary>
        public override StorageFileInfo GetFileInfo(DataCategory category, string path)
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
                conn.Open();

                using (NpgsqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (reader.Read())
                    {
                        return new StorageFileInfo
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
            }

            return StorageFileInfo.FileNotExists;
        }
    }
}
