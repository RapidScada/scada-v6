// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Npgsql;
using NpgsqlTypes;
using Scada.Admin.Deployment;
using Scada.Admin.Lang;
using Scada.Admin.Project;
using Scada.Agent.Client;
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
    /// Uploads configuration.
    /// <para>Передаёт конфигурацию.</para>
    /// </summary>
    internal class Uploader
    {
        /// <summary>
        /// The number of upload tasks.
        /// </summary>
        private const int TaskCount = 15;

        private readonly ScadaProject project;
        private readonly ProjectInstance instance;
        private readonly DeploymentProfile profile;
        private readonly ITransferControl transferControl;
        private readonly UploadOptions uploadOptions;
        private readonly ProgressTracker progressTracker;
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
            progressTracker = new ProgressTracker(transferControl) { TaskCount = TaskCount };
            conn = null;
        }


        /// <summary>
        /// Creates a database schema.
        /// </summary>
        private void CreateSchema()
        {
            transferControl.ThrowIfCancellationRequested();
            transferControl.WriteMessage(ExtensionPhrases.CreateSchema);

            string sql = "CREATE SCHEMA IF NOT EXISTS " + Schema;
            new NpgsqlCommand(sql, conn).ExecuteNonQuery();
            progressTracker.TaskIndex++;
        }

        /// <summary>
        /// Creates and fills the application dictionary.
        /// </summary>
        private void CreateAppTable()
        {
            transferControl.ThrowIfCancellationRequested();
            transferControl.WriteMessage(ExtensionPhrases.CreateAppDict);
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
                progressTracker.TaskIndex++;
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
            transferControl.ThrowIfCancellationRequested();
            transferControl.WriteLine();
            transferControl.WriteMessage(ExtensionPhrases.ClearBase);
            NpgsqlTransaction trans = null;

            try
            {
                trans = conn.BeginTransaction();
                progressTracker.SubtaskCount = project.ConfigDatabase.AllTables.Length;

                foreach (IBaseTable baseTable in project.ConfigDatabase.AllTables)
                {
                    transferControl.ThrowIfCancellationRequested();
                    transferControl.WriteMessage(string.Format(ExtensionPhrases.DeleteTable, baseTable.Name));

                    string sql = $"DROP TABLE IF EXISTS {GetBaseTableName(baseTable)} CASCADE";
                    new NpgsqlCommand(sql, conn, trans).ExecuteNonQuery();
                    progressTracker.SubtaskIndex++;
                }

                trans.Commit();
                progressTracker.TaskIndex++;
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
            transferControl.ThrowIfCancellationRequested();
            transferControl.WriteLine();
            transferControl.WriteMessage(ExtensionPhrases.CreateBase);
            NpgsqlTransaction trans = null;

            try
            {
                trans = conn.BeginTransaction();
                progressTracker.SubtaskCount = project.ConfigDatabase.AllTables.Length;

                foreach (IBaseTable baseTable in project.ConfigDatabase.AllTables)
                {
                    transferControl.ThrowIfCancellationRequested();
                    transferControl.WriteMessage(string.Format(ExtensionPhrases.CreateTable, baseTable.Name));

                    string sql = GetBaseTableDDL(baseTable);
                    new NpgsqlCommand(sql, conn, trans).ExecuteNonQuery();
                    InsertRows(baseTable, trans);
                    progressTracker.SubtaskIndex++;
                }

                trans.Commit();
                progressTracker.TaskIndex++;
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
            transferControl.ThrowIfCancellationRequested();
            transferControl.WriteLine();
            transferControl.WriteMessage(ExtensionPhrases.CreateFKs);
            NpgsqlTransaction trans = null;

            try
            {
                trans = conn.BeginTransaction();
                progressTracker.SubtaskCount = project.ConfigDatabase.AllTables.Length;

                foreach (IBaseTable baseTable in project.ConfigDatabase.AllTables)
                {
                    transferControl.ThrowIfCancellationRequested();
                    transferControl.WriteMessage(string.Format(ExtensionPhrases.CreateTableFKs, baseTable.Name));

                    foreach (TableRelation relation in baseTable.DependsOn)
                    {
                        string sql = GetBaseForeignKeyDDL(relation);
                        new NpgsqlCommand(sql, conn, trans).ExecuteNonQuery();
                    }

                    progressTracker.SubtaskIndex++;
                }

                trans.Commit();
                progressTracker.TaskIndex++;
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
            transferControl.ThrowIfCancellationRequested();
            transferControl.WriteLine();
            transferControl.WriteMessage(ExtensionPhrases.ClearViews);

            string sql = $"DROP TABLE IF EXISTS {Schema}.view_file CASCADE";
            new NpgsqlCommand(sql, conn).ExecuteNonQuery();
            progressTracker.TaskIndex++;
        }

        /// <summary>
        /// Creates and fills the view table.
        /// </summary>
        private void CreateViews()
        {
            transferControl.ThrowIfCancellationRequested();
            transferControl.WriteMessage(ExtensionPhrases.CreateViews);
            NpgsqlTransaction trans = null;

            try
            {
                trans = conn.BeginTransaction();
                new NpgsqlCommand(GetViewTableDDL(), conn, trans).ExecuteNonQuery();
                ICollection<FileInfo> viewFiles = GetViewFiles();

                if (viewFiles.Count > 0)
                {
                    string sql = $"INSERT INTO {Schema}.view_file (path, contents, write_time) " +
                        "VALUES (@path, @contents, @writeTime)";
                    NpgsqlCommand cmd = new(sql, conn, trans);
                    NpgsqlParameter pathParam = cmd.Parameters.Add("path", NpgsqlDbType.Varchar);
                    NpgsqlParameter contentsParam = cmd.Parameters.Add("contents", NpgsqlDbType.Bytea);
                    NpgsqlParameter writeTimeParam = cmd.Parameters.Add("writeTime", NpgsqlDbType.TimestampTz);

                    int viewDirLen = project.Views.ViewDir.Length;
                    progressTracker.SubtaskCount = viewFiles.Count;

                    foreach (FileInfo fileInfo in viewFiles)
                    {
                        transferControl.ThrowIfCancellationRequested();
                        transferControl.WriteMessage(string.Format(ExtensionPhrases.CreateView, fileInfo.Name));

                        pathParam.Value = fileInfo.FullName[viewDirLen..];
                        contentsParam.Value = File.ReadAllBytes(fileInfo.FullName);
                        writeTimeParam.Value = fileInfo.LastWriteTimeUtc;
                        cmd.ExecuteNonQuery();
                        progressTracker.SubtaskIndex++;
                    }
                }

                trans.Commit();
                transferControl.WriteMessage(string.Format(AdminPhrases.FileCount, viewFiles.Count));
                progressTracker.TaskIndex++;
            }
            catch
            {
                trans?.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Gets a collection of view files.
        /// </summary>
        private ICollection<FileInfo> GetViewFiles()
        {
            if (uploadOptions.ObjectFilter.Count > 0)
            {
                List<FileInfo> fileInfoList = new();
                HashSet<string> pathSet = new(); // ensures uniqueness

                foreach (View view in SelectItems(project.ConfigDatabase.ViewTable, uploadOptions.ObjectFilter))
                {
                    if (pathSet.Add(view.Path))
                    {
                        string fileName = Path.Combine(project.Views.ViewDir, view.Path);
                        FileInfo fileInfo = new(fileName);

                        if (fileInfo.Exists)
                            fileInfoList.Add(fileInfo);
                    }
                }

                return fileInfoList;
            }
            else
            {
                DirectoryInfo viewDirInfo = new(project.Views.ViewDir);
                return viewDirInfo.Exists 
                    ? viewDirInfo.GetFiles("*", SearchOption.AllDirectories) 
                    : Array.Empty<FileInfo>();
            }
        }

        /// <summary>
        /// Clears the configuration of all applications.
        /// </summary>
        private void ClearAllAppConfig()
        {
            transferControl.ThrowIfCancellationRequested();
            transferControl.WriteLine();
            transferControl.WriteMessage(ExtensionPhrases.ClearAllAppConfig);

            string sql = $"DROP TABLE IF EXISTS {Schema}.app_config CASCADE";
            new NpgsqlCommand(sql, conn).ExecuteNonQuery();
            progressTracker.TaskIndex++;
        }

        /// <summary>
        /// Clears the configuration of the specified application.
        /// </summary>
        private void ClearAppConfig(ProjectApp app)
        {
            transferControl.ThrowIfCancellationRequested();
            transferControl.WriteLine();
            transferControl.WriteMessage(string.Format(ExtensionPhrases.ClearAppConfig, app.AppName));

            string sql = $"DELETE FROM {Schema}.app_config WHERE app_id = @appID" +
                (uploadOptions.IgnoreRegKeys ? $" AND path NOT LIKE '%{ScadaUtils.RegFileSuffix}'" : "");
            NpgsqlCommand cmd = new(sql, conn);
            cmd.Parameters.AddWithValue("appID", (int)app.ServiceApp);
            cmd.ExecuteNonQuery();
            progressTracker.TaskIndex++;
        }

        /// <summary>
        /// Creates and fills the configuration of the specified application.
        /// </summary>
        private void CreateAppConfig(ProjectApp app)
        {
            transferControl.ThrowIfCancellationRequested();
            transferControl.WriteMessage(string.Format(ExtensionPhrases.CreateAppConfig, app.AppName));
            NpgsqlTransaction trans = null;

            try
            {
                trans = conn.BeginTransaction();
                new NpgsqlCommand(GetAppConfigTableDDL(), conn, trans).ExecuteNonQuery();
                ICollection<FileInfo> configFiles = GetAppConfigFiles(app);

                if (configFiles.Count > 0)
                {
                    string sql = $"INSERT INTO {Schema}.app_config (app_id, path, contents, write_time) " +
                        "VALUES (@appID, @path, @contents, @writeTime)";
                    NpgsqlCommand cmd = new(sql, conn, trans);
                    cmd.Parameters.AddWithValue("appID", (int)app.ServiceApp);
                    NpgsqlParameter pathParam = cmd.Parameters.Add("path", NpgsqlDbType.Varchar);
                    NpgsqlParameter contentsParam = cmd.Parameters.Add("contents", NpgsqlDbType.Varchar);
                    NpgsqlParameter writeTimeParam = cmd.Parameters.Add("writeTime", NpgsqlDbType.TimestampTz);

                    int configDirLen = app.ConfigDir.Length;
                    progressTracker.SubtaskCount = configFiles.Count;

                    foreach (FileInfo fileInfo in configFiles)
                    {
                        transferControl.ThrowIfCancellationRequested();
                        transferControl.WriteMessage(string.Format(ExtensionPhrases.CreateConfigFile, fileInfo.Name));

                        pathParam.Value = fileInfo.FullName[configDirLen..];
                        contentsParam.Value = File.ReadAllText(fileInfo.FullName, Encoding.UTF8);
                        writeTimeParam.Value = fileInfo.LastWriteTimeUtc;
                        cmd.ExecuteNonQuery();
                        progressTracker.SubtaskIndex++;
                    }
                }

                trans.Commit();
                transferControl.WriteMessage(string.Format(AdminPhrases.FileCount, configFiles.Count));
                progressTracker.TaskIndex++;
            }
            catch
            {
                trans?.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Gets a collection of application configuration files.
        /// </summary>
        private ICollection<FileInfo> GetAppConfigFiles(ProjectApp app)
        {
            DirectoryInfo configDirInfo = new(app.ConfigDir);

            if (configDirInfo.Exists)
            {
                if (uploadOptions.IgnoreRegKeys)
                {
                    List<FileInfo> fileInfoList = new();

                    foreach (FileInfo fileInfo in configDirInfo.EnumerateFiles("*", SearchOption.AllDirectories))
                    {
                        if (!fileInfo.Name.EndsWith(ScadaUtils.RegFileSuffix, StringComparison.OrdinalIgnoreCase))
                            fileInfoList.Add(fileInfo);
                    }

                    return fileInfoList;
                }
                else
                {
                    return configDirInfo.GetFiles("*", SearchOption.AllDirectories);
                }
            }
            else
            {
                return Array.Empty<FileInfo>();
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
        public void Upload()
        {
            if (!profile.DbEnabled)
                throw new ScadaException(AdminPhrases.DbNotEnabled);

            transferControl.SetCancelEnabled(true);
            transferControl.WriteMessage(AdminPhrases.UploadConfig);

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
                else
                {
                    progressTracker.SkipTask(3);
                }

                if (uploadOptions.IncludeView)
                {
                    ClearViews();
                    CreateViews();
                }
                else
                {
                    progressTracker.SkipTask(2);
                }

                bool clearAllAppConfig = !uploadOptions.IgnoreRegKeys &&
                    (uploadOptions.IncludeServer || !instance.ServerApp.Enabled) &&
                    (uploadOptions.IncludeComm || !instance.CommApp.Enabled) &&
                    (uploadOptions.IncludeWeb || !instance.WebApp.Enabled);

                if (clearAllAppConfig)
                    ClearAllAppConfig();
                else
                    progressTracker.SkipTask();

                void UploadAppConfig(bool includeApp, ProjectApp app)
                {
                    if (includeApp && app.Enabled)
                    {
                        if (clearAllAppConfig)
                        {
                            progressTracker.SkipTask();
                            transferControl.WriteLine();
                        }
                        else
                        {
                            ClearAppConfig(app);
                        }

                        CreateAppConfig(app);
                    }
                    else
                    {
                        progressTracker.SkipTask(2);
                    }
                }

                UploadAppConfig(uploadOptions.IncludeServer, instance.ServerApp);
                UploadAppConfig(uploadOptions.IncludeComm, instance.CommApp);
                UploadAppConfig(uploadOptions.IncludeWeb, instance.WebApp);

                if (!uploadOptions.RestartAnyService)
                {
                    progressTracker.SkipTask();
                }
                else if (profile.AgentEnabled)
                {
                    AgentClient agentClient = new AgentClient(profile.AgentConnectionOptions);
                    new ServiceStarter(agentClient, uploadOptions, transferControl, progressTracker)
                        .SetProcessTimeout(profile.AgentConnectionOptions.Timeout)
                        .RestartServices();
                    agentClient.TerminateSession();
                }
                else
                {
                    transferControl.WriteLine();
                    transferControl.WriteError(ExtensionPhrases.UnableRestartServices);
                    progressTracker.SkipTask();
                }
            }
            finally
            {
                conn?.Close();
                conn = null;
            }

            progressTracker.SetCompleted();
            transferControl.WriteLine();
            transferControl.WriteMessage(AdminPhrases.UploadConfigCompleted);
        }
    }
}
