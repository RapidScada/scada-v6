// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Npgsql;
using NpgsqlTypes;
using Scada.Admin.Deployment;
using Scada.Admin.Project;
using Scada.Data.Entities;
using Scada.Data.Tables;
using Scada.Lang;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using static Scada.Storages.PostgreSqlStorage.PostgreSqlStorageShared;

namespace Scada.Admin.Extensions.ExtDepPostgreSql
{
    /// <summary>
    /// Uploads the configuration.
    /// <para>Передаёт конфигурацию.</para>
    /// </summary>
    internal class Uploader
    {
        private readonly ScadaProject project;
        private readonly ProjectInstance instance;
        private readonly DeploymentProfile profile;
        private readonly ITransferControl transferControl;
        private readonly UploadOptions uploadOptions;
        private NpgsqlConnection conn;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Uploader(ScadaProject project, ProjectInstance instance, DeploymentProfile profile, 
            ITransferControl transferControl)
        {
            this.project = project ?? throw new ArgumentNullException(nameof(project));
            this.instance = instance ?? throw new ArgumentNullException(nameof(instance));
            this.profile = profile ?? throw new ArgumentNullException(nameof(profile));
            this.transferControl = transferControl ?? throw new ArgumentNullException(nameof(transferControl));
            uploadOptions = profile.UploadOptions;
            conn = null;
        }


        /// <summary>
        /// Creates a database schema.
        /// </summary>
        private void CreateSchema()
        {
            transferControl.WriteMessage(Locale.IsRussian ?
                "Создание схемы базы данных" :
                "Create database schema");

            string sql = "CREATE SCHEMA IF NOT EXISTS " + Schema;
            new NpgsqlCommand(sql, conn).ExecuteNonQuery();
        }

        /// <summary>
        /// Creates and fills the application dictionary.
        /// </summary>
        private void CreateAppTable()
        {
            transferControl.WriteMessage(Locale.IsRussian ?
                "Создание справочника приложений" :
                "Create application dictionary");
            NpgsqlTransaction trans = null;

            try
            {
                trans = conn.BeginTransaction();
                new NpgsqlCommand(GetAppTableDDL(), conn, trans).ExecuteNonQuery();

                string sql = $"INSERT INTO {Schema}.app (app_id, name) VALUES (@appID, @name) " + 
                    "ON CONFLICT (app_id) DO NOTHING";
                NpgsqlCommand insertCmd = new(sql, conn, trans);
                NpgsqlParameter appIdParam = insertCmd.Parameters.Add("appID", NpgsqlDbType.Integer);
                NpgsqlParameter nameParam = insertCmd.Parameters.Add("name", NpgsqlDbType.Varchar);

                foreach (ServiceApp app in Enum.GetValues(typeof(ServiceApp)))
                {
                    if (app != ServiceApp.Unknown)
                    {
                        appIdParam.Value = (int)app;
                        nameParam.Value = ScadaUtils.GetAppName(app);
                        insertCmd.ExecuteNonQuery();
                    }
                }

                trans.Commit();
            }
            catch
            {
                trans?.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Removes all tables from the configuration database.
        /// </summary>
        private void ClearBase()
        {
            transferControl.WriteLine();
            transferControl.WriteMessage(Locale.IsRussian ?
                "Очистка базы конфигурации" :
                "Clear the configuration database");
            NpgsqlTransaction trans = null;

            try
            {
                trans = conn.BeginTransaction();

                foreach (IBaseTable baseTable in project.ConfigBase.AllTables)
                {
                    transferControl.WriteMessage(string.Format(Locale.IsRussian ?
                        "Удаление таблицы {0}" :
                        "Delete the {0} table", baseTable.Name));

                    string sql = $"DROP TABLE IF EXISTS {GetBaseTableName(baseTable)} CASCADE";
                    new NpgsqlCommand(sql, conn, trans).ExecuteNonQuery();
                }

                trans.Commit();
            }
            catch
            {
                trans?.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Creates and fills the configuration database tables.
        /// </summary>
        private void CreateBase()
        {
            transferControl.WriteLine();
            transferControl.WriteMessage(Locale.IsRussian ?
                "Создание базы конфигурации" :
                "Create the configuration database");
            NpgsqlTransaction trans = null;

            try
            {
                trans = conn.BeginTransaction();
                bool filterByObj = uploadOptions.ObjectFilter.Count > 0;

                foreach (IBaseTable baseTable in project.ConfigBase.AllTables)
                {
                    transferControl.WriteMessage(string.Format(Locale.IsRussian ?
                        "Создание таблицы {0}" :
                        "Create the {0} table", baseTable.Name));

                    string sql = GetBaseTableDDL(baseTable);
                    new NpgsqlCommand(sql, conn, trans).ExecuteNonQuery();
                    InsertRows(baseTable, trans);
                }

                trans.Commit();
            }
            catch
            {
                trans?.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Inserts rows in the configuration database table.
        /// </summary>
        private void InsertRows(IBaseTable baseTable, NpgsqlTransaction trans)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(baseTable.ItemType);
            int propCnt = props.Count;

            if (propCnt == 0 || baseTable.ItemCount == 0)
                return;

            // create INSERT script like
            // INSERT INTO tablename (col1, col2) VALUES (@Col1, @Col2);
            StringBuilder sbSql1 = new();
            StringBuilder sbSql2 = new();
            sbSql1.Append("INSERT INTO ").Append(GetBaseTableName(baseTable)).Append(" (");
            sbSql2.Append("VALUES (");

            for (int i = 0; i < propCnt; i++)
            {
                if (i > 0)
                {
                    sbSql1.Append(", ");
                    sbSql2.Append(", ");
                }

                PropertyDescriptor prop = props[i];
                sbSql1.Append(GetBaseColumnName(prop));
                sbSql2.Append('@').Append(prop.Name);
            }

            sbSql1.Append(") ");
            sbSql2.Append(");");

            // create INSERT command
            string sql = sbSql1.ToString() + sbSql2.ToString();
            NpgsqlCommand cmd = new(sql, conn, trans);

            foreach (PropertyDescriptor prop in props)
            {
                cmd.Parameters.Add(prop.Name, GetDbType(prop.PropertyType));
            }

            // execute command for table items
            bool filterByObj = uploadOptions.ObjectFilter.Count > 0 &&
                (baseTable.ItemType == typeof(Cnl) || baseTable.ItemType == typeof(View));

            foreach (object item in filterByObj ?
                SelectItems(baseTable, uploadOptions.ObjectFilter) : baseTable.EnumerateItems())
            {
                for (int i = 0; i < propCnt; i++)
                {
                    cmd.Parameters[i].Value = props[i].GetValue(item) ?? DBNull.Value;
                }

                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Creates foreign keys for the configuration database tables.
        /// </summary>
        private void CreateForeignKeys()
        {
            transferControl.WriteLine();
            transferControl.WriteMessage(Locale.IsRussian ?
                "Создание внешних ключей" :
                "Create foreign keys");
            NpgsqlTransaction trans = null;

            try
            {
                trans = conn.BeginTransaction();
                trans.Commit();

                foreach (IBaseTable baseTable in project.ConfigBase.AllTables)
                {
                    transferControl.WriteMessage(string.Format(Locale.IsRussian ?
                        "Создание внешних ключей таблицы {0}" :
                        "Create foreign keys for the {0} table", baseTable.Name));

                    foreach (TableRelation relation in baseTable.DependsOn)
                    {
                        string sql = GetBaseForeignKeyDDL(relation);
                        new NpgsqlCommand(sql, conn, trans).ExecuteNonQuery();
                    }
                }
            }
            catch
            {
                trans?.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Removes the view table.
        /// </summary>
        private void ClearViews()
        {
            transferControl.WriteLine();
            transferControl.WriteMessage(Locale.IsRussian ?
                "Очистка представлений" :
                "Clear views");

            string sql = $"DROP TABLE IF EXISTS {Schema}.view_file CASCADE";
            new NpgsqlCommand(sql, conn).ExecuteNonQuery();
        }

        /// <summary>
        /// Creates and fills the view table.
        /// </summary>
        private void CreateViews()
        {
            transferControl.WriteMessage(Locale.IsRussian ?
                "Создание представлений" :
                "Create views");
            NpgsqlTransaction trans = null;

            try
            {
                trans = conn.BeginTransaction();
                new NpgsqlCommand(GetViewTableDDL(), conn, trans).ExecuteNonQuery();

                string sql = $"INSERT INTO {Schema}.view_file (path, contents, write_time) " +
                    "VALUES (@path, @contents, @writeTime)";
                NpgsqlCommand cmd = new(sql, conn, trans);
                NpgsqlParameter pathParam = cmd.Parameters.Add("path", NpgsqlDbType.Varchar);
                NpgsqlParameter contentsParam = cmd.Parameters.Add("contents", NpgsqlDbType.Bytea);
                NpgsqlParameter writeTimeParam = cmd.Parameters.Add("writeTime", NpgsqlDbType.TimestampTz);
                int viewDirLen = project.Views.ViewDir.Length;

                foreach (FileInfo fileInfo in EnumerateViewFiles())
                {
                    transferControl.WriteMessage(string.Format(Locale.IsRussian ?
                        "Создание представления \"{0}\"" :
                        "Create view \"{0}\"", fileInfo.Name));

                    pathParam.Value = fileInfo.FullName[viewDirLen..];
                    contentsParam.Value = File.ReadAllBytes(fileInfo.FullName);
                    writeTimeParam.Value = fileInfo.LastWriteTimeUtc;
                    cmd.ExecuteNonQuery();
                }

                trans.Commit();
            }
            catch
            {
                trans?.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Returns an enumerable collection of view file names.
        /// </summary>
        private IEnumerable<FileInfo> EnumerateViewFiles()
        {
            if (uploadOptions.ObjectFilter.Count > 0)
            {
                HashSet<string> pathSet = new(); // ensures uniqueness

                foreach (View view in SelectItems(project.ConfigBase.ViewTable, uploadOptions.ObjectFilter))
                {
                    if (pathSet.Add(view.Path))
                    {
                        string fileName = Path.Combine(project.Views.ViewDir, view.Path);
                        FileInfo fileInfo = new(fileName);

                        if (fileInfo.Exists)
                            yield return fileInfo;
                    }
                }
            }
            else
            {
                DirectoryInfo viewDirInfo = new(project.Views.ViewDir);
                
                if (viewDirInfo.Exists)
                {
                    foreach (FileInfo fileInfo in viewDirInfo.EnumerateFiles("*", SearchOption.AllDirectories))
                    {
                        yield return fileInfo;
                    }
                }
            }
        }

        /// <summary>
        /// Clears the configuration of all applications.
        /// </summary>
        private void ClearAllAppConfig()
        {
            transferControl.WriteLine();
            transferControl.WriteMessage(Locale.IsRussian ?
                "Очистка конфигурации всех приложений" :
                "Clear configuration of all applications");

            string sql = $"DROP TABLE IF EXISTS {Schema}.app_config CASCADE";
            new NpgsqlCommand(sql, conn).ExecuteNonQuery();
        }

        /// <summary>
        /// Clears the configuration of the specified application.
        /// </summary>
        private void ClearAppConfig(ProjectApp app)
        {
            transferControl.WriteLine();
            transferControl.WriteMessage(string.Format(Locale.IsRussian ?
                "Очистка конфигурации приложения {0}" :
                "Clear configuration of the {0} application", app.AppName));

            string sql = $"DELETE FROM {Schema}.app_config WHERE app_id = @appID" +
                (uploadOptions.IgnoreRegKeys ? " AND path NOT LIKE '%_Reg.xml'" : "");
            NpgsqlCommand cmd = new(sql, conn);
            cmd.Parameters.AddWithValue("appID", (int)app.ServiceApp);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Creates and fills the configuration of the specified application.
        /// </summary>
        private void CreateAppConfig(ProjectApp app)
        {
            transferControl.WriteMessage(string.Format(Locale.IsRussian ?
                "Создание конфигурации приложения {0}" :
                "Create configuration of the {0} application", app.AppName));
            NpgsqlTransaction trans = null;

            try
            {
                trans = conn.BeginTransaction();
                new NpgsqlCommand(GetAppConfigTableDDL(), conn, trans).ExecuteNonQuery();

                DirectoryInfo configDirInfo = new(app.ConfigDir);
                int fileCount = 0;

                if (configDirInfo.Exists)
                {
                    string sql = $"INSERT INTO {Schema}.app_config (app_id, path, contents, write_time) " +
                        "VALUES (@appID, @path, @contents, @writeTime)";
                    NpgsqlCommand cmd = new(sql, conn, trans);
                    cmd.Parameters.AddWithValue("appID", (int)app.ServiceApp);
                    NpgsqlParameter pathParam = cmd.Parameters.Add("path", NpgsqlDbType.Varchar);
                    NpgsqlParameter contentsParam = cmd.Parameters.Add("contents", NpgsqlDbType.Varchar);
                    NpgsqlParameter writeTimeParam = cmd.Parameters.Add("writeTime", NpgsqlDbType.TimestampTz);
                    int configDirLen = app.ConfigDir.Length;

                    foreach (FileInfo fileInfo in configDirInfo.EnumerateFiles("*", SearchOption.AllDirectories))
                    {
                        if (!uploadOptions.IgnoreRegKeys || 
                            !fileInfo.Name.EndsWith("_Reg.xml", StringComparison.OrdinalIgnoreCase))
                        {
                            transferControl.WriteMessage(string.Format(Locale.IsRussian ?
                                "Создание файла конфигурации \"{0}\"" :
                                "Create configuration file \"{0}\"", fileInfo.Name));

                            pathParam.Value = fileInfo.FullName[configDirLen..];
                            contentsParam.Value = File.ReadAllText(fileInfo.FullName, Encoding.UTF8);
                            writeTimeParam.Value = fileInfo.LastWriteTimeUtc;
                            cmd.ExecuteNonQuery();
                            fileCount++;
                        }
                    }
                }

                transferControl.WriteMessage(string.Format(Locale.IsRussian ?
                    "Количество файлов: {0}" :
                    "File count: {0}", fileCount));
                trans.Commit();
            }
            catch
            {
                trans?.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Gets a SQL script to create the configuration database table.
        /// </summary>
        private static string GetBaseTableDDL(IBaseTable baseTable)
        {
            StringBuilder sbSql = new();
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(baseTable.ItemType);

            if (props.Count > 0)
            {
                sbSql.AppendLine($"CREATE TABLE IF NOT EXISTS {GetBaseTableName(baseTable)} (");

                foreach (PropertyDescriptor prop in props)
                {
                    sbSql
                        .Append(GetBaseColumnName(prop)).Append(' ')
                        .Append(GetDbTypeName(prop.PropertyType))
                        .Append(prop.PropertyType.IsNullable() || prop.PropertyType.IsClass ? "" : " NOT NULL")
                        .AppendLine(",");
                }

                sbSql.AppendLine(
                    $"CONSTRAINT pk_{baseTable.Name.ToLowerInvariant()} " +
                    $"PRIMARY KEY ({GetBaseColumnName(baseTable.PrimaryKey)}))");
            }

            return sbSql.ToString();
        }

        /// <summary>
        /// Gets a SQL script to create a foreign key of the configuration database table.
        /// </summary>
        private static string GetBaseForeignKeyDDL(TableRelation relation)
        {
            // use child colmun name instead of parent table name,
            // because the RoleRef table has 2 FKs referencing the Role table
            string fkName = "fk_" +
                relation.ChildTable.Name.ToLowerInvariant() + "_" +
                relation.ChildColumn.ToLowerInvariant();

            return
                $"ALTER TABLE {GetBaseTableName(relation.ChildTable)} " +
                $"ADD CONSTRAINT {fkName} FOREIGN KEY ({GetBaseColumnName(relation.ChildColumn)}) " +
                $"REFERENCES {GetBaseTableName(relation.ParentTable)} ({GetBaseColumnName(relation.ParentTable.PrimaryKey)})";
        }

        /// <summary>
        /// Gets a SQL script to create the view table.
        /// </summary>
        private static string GetViewTableDDL()
        {
            return
                $"CREATE TABLE IF NOT EXISTS {Schema}.view_file(" +
                "file_id integer NOT NULL GENERATED BY DEFAULT AS IDENTITY, " +
                "path varchar NOT NULL, " +
                "contents bytea NULL, " +
                "write_time timestamp with time zone, " +
                "CONSTRAINT pk_view_file PRIMARY KEY (file_id), " +
                "CONSTRAINT un_view_file_path UNIQUE (path))";
        }

        /// <summary>
        /// Gets a SQL script to create the application table.
        /// </summary>
        private static string GetAppTableDDL()
        {
            return
                $"CREATE TABLE IF NOT EXISTS {Schema}.app(" +
                "app_id integer NOT NULL, " +
                "name varchar(100) NOT NULL, " +
                "CONSTRAINT pk_app PRIMARY KEY (app_id))";
        }

        /// <summary>
        /// Gets a SQL script to create the application configuration table.
        /// </summary>
        private static string GetAppConfigTableDDL()
        {
            return
                $"CREATE TABLE IF NOT EXISTS {Schema}.app_config(" +
                "file_id integer NOT NULL GENERATED BY DEFAULT AS IDENTITY, " +
                "app_id integer NOT NULL, " +
                "path varchar NOT NULL, " +
                "contents varchar NULL, " +
                "write_time timestamp with time zone, " +
                "CONSTRAINT pk_app_config PRIMARY KEY (file_id), " +
                "CONSTRAINT un_app_config_app_path UNIQUE (app_id, path), " +
                $"CONSTRAINT fk_app_config_app FOREIGN KEY (app_id) REFERENCES {Schema}.app (app_id)); " +
                $"CREATE INDEX IF NOT EXISTS fki_app_config_app_fkey ON {Schema}.app_config (app_id);";
        }

        /// <summary>
        /// Gets the database type name corresponding to the specified property type.
        /// </summary>
        private static string GetDbTypeName(Type propType)
        {
            Type type = propType.IsNullable() ? Nullable.GetUnderlyingType(propType) : propType;

            if (type == typeof(int))
                return "integer";
            else if (type == typeof(double))
                return "double precision";
            else if (type == typeof(bool))
                return "boolean";
            else if (type == typeof(DateTime))
                return "timestamp with time zone";
            else if (type == typeof(string))
                return "character varying";
            else
                throw new ScadaException("Data type {0} is not supported.", type.FullName);
        }

        /// <summary>
        /// Gets the database type corresponding to the specified property type.
        /// </summary>
        private static NpgsqlDbType GetDbType(Type propType)
        {
            Type type = propType.IsNullable() ? Nullable.GetUnderlyingType(propType) : propType;

            if (type == typeof(int))
                return NpgsqlDbType.Integer;
            else if (type == typeof(double))
                return NpgsqlDbType.Double;
            else if (type == typeof(bool))
                return NpgsqlDbType.Boolean;
            else if (type == typeof(DateTime))
                return NpgsqlDbType.TimestampTz;
            else if (type == typeof(string))
                return NpgsqlDbType.Varchar;
            else
                throw new ScadaException("Data type {0} is not supported.", type.FullName);
        }

        /// <summary>
        /// Selects items from the specified table filtered by objects.
        /// </summary>
        private static IEnumerable SelectItems(IBaseTable baseTable, List<int> objNums)
        {
            foreach (int objNum in objNums)
            {
                foreach (object item in baseTable.SelectItems(new TableFilter("ObjNum", objNum), true))
                {
                    yield return item;
                }
            }
        }


        /// <summary>
        /// Uploads the configuration.
        /// </summary>
        public bool Upload()
        {
            if (!profile.DbEnabled)
            {
                throw new ScadaException(Locale.IsRussian ?
                    "База данных не включена в профиле развёртывания." :
                    "Database is not enabled in the deployment profile.");
            }

            transferControl.SetCancelEnabled(true);
            transferControl.WriteMessage(Locale.IsRussian ?
                "Передача конфигурации" :
                "Upload configutation");

            try
            {
                conn = CreateDbConnection(profile.DbConnectionOptions);
                conn.Open();
                CreateSchema();
                CreateAppTable();

                if (uploadOptions.IncludeBase)
                {
                    ClearBase();
                    CreateBase();
                    CreateForeignKeys();
                }

                if (uploadOptions.IncludeView)
                {
                    ClearViews();
                    CreateViews();
                }

                bool clearAllAppConfig = !uploadOptions.IgnoreRegKeys &&
                    (uploadOptions.IncludeServer || !instance.ServerApp.Enabled) &&
                    (uploadOptions.IncludeComm || !instance.CommApp.Enabled) &&
                    (uploadOptions.IncludeWeb || !instance.WebApp.Enabled);

                if (clearAllAppConfig)
                    ClearAllAppConfig();

                void UploadAppConfig(ProjectApp app)
                {
                    if (clearAllAppConfig)
                        transferControl.WriteLine();
                    else
                        ClearAppConfig(app);

                    CreateAppConfig(app);
                }

                if (uploadOptions.IncludeServer && instance.ServerApp.Enabled)
                    UploadAppConfig(instance.ServerApp);

                if (uploadOptions.IncludeComm && instance.CommApp.Enabled)
                    UploadAppConfig(instance.CommApp);

                if (uploadOptions.IncludeWeb && instance.WebApp.Enabled)
                    UploadAppConfig(instance.WebApp);
            }
            finally
            {
                conn?.Close();
                conn = null;
            }

            transferControl.WriteLine();
            transferControl.WriteMessage(Locale.IsRussian ?
                "Передача конфигурации завершена успешно" :
                "Configuration uploaded successfully");
            return true;
        }
    }
}
