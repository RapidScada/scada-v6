// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Npgsql;
using Scada.Admin.Deployment;
using Scada.Admin.Lang;
using Scada.Admin.Project;
using Scada.Data.Tables;
using Scada.Lang;
using System;
using System.IO;
using System.Text;
using static Scada.Storages.PostgreSqlStorage.PostgreSqlStorageShared;

namespace Scada.Admin.Extensions.ExtDepPostgreSql
{
    /// <summary>
    /// Downloads configuration.
    /// <para>Скачивает конфигурацию.</para>
    /// </summary>
    internal class Downloader
    {
        /// <summary>
        /// The number of download tasks.
        /// </summary>
        private const int TaskCount = 5;

        private readonly ScadaProject project;
        private readonly ProjectInstance instance;
        private readonly DeploymentProfile profile;
        private readonly ITransferControl transferControl;
        private readonly DownloadOptions downloadOptions;
        private readonly ProgressTracker progressTracker;
        private NpgsqlConnection conn;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Downloader(ScadaProject project, ProjectInstance instance, DeploymentProfile profile,
            ITransferControl transferControl)
        {
            this.project = project ?? throw new ArgumentNullException(nameof(project));
            this.instance = instance ?? throw new ArgumentNullException(nameof(instance));
            this.profile = profile ?? throw new ArgumentNullException(nameof(profile));
            this.transferControl = transferControl ?? throw new ArgumentNullException(nameof(transferControl));
            downloadOptions = profile.DownloadOptions;
            progressTracker = new ProgressTracker(transferControl) { TaskCount = TaskCount };
            conn = null;
        }


        /// <summary>
        /// Downloads the configuration database.
        /// </summary>
        private void DownloadBase()
        {
            transferControl.ThrowIfCancellationRequested();
            transferControl.WriteLine();
            transferControl.WriteMessage(Locale.IsRussian ?
                "Скачивание базы конфигурации" :
                "Download the configuration database");
            progressTracker.SubtaskCount = project.ConfigBase.AllTables.Length;

            foreach (IBaseTable baseTable in project.ConfigBase.AllTables)
            {
                transferControl.ThrowIfCancellationRequested();
                transferControl.WriteMessage(string.Format(Locale.IsRussian ?
                    "Скачивание таблицы {0}" :
                    "Download the {0} table", baseTable.Name));
                ReadBaseTable(baseTable, conn);
                progressTracker.SubtaskIndex++;
            }

            progressTracker.TaskIndex++;
        }

        /// <summary>
        /// Downloads view files.
        /// </summary>
        private void DownloadViews()
        {
            transferControl.ThrowIfCancellationRequested();
            transferControl.WriteLine();
            transferControl.WriteMessage(Locale.IsRussian ?
                "Скачивание представлений" :
                "Download views");

            string sql = $"SELECT path, contents FROM {Schema}.view_file";
            NpgsqlCommand cmd = new(sql, conn);
            int fileCount = 0;
            using NpgsqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read() && !reader.IsDBNull(1))
            {
                transferControl.ThrowIfCancellationRequested();
                string path = reader.GetString(0);
                transferControl.WriteMessage(string.Format(Locale.IsRussian ?
                    "Скачивание представления \"{0}\"" :
                    "Download view \"{0}\"", path));

                string absolutePath = Path.Combine(project.Views.ViewDir, path);
                Directory.CreateDirectory(Path.GetDirectoryName(absolutePath));
                File.WriteAllBytes(absolutePath, (byte[])reader[1]);
                fileCount++;
            }

            transferControl.WriteMessage(string.Format(AdminPhrases.FileCount, fileCount));
            progressTracker.TaskIndex++;
        }

        /// <summary>
        /// Downloads the configuration of the specified application.
        /// </summary>
        private void DownloadAppConfig(ProjectApp app)
        {
            transferControl.ThrowIfCancellationRequested();
            transferControl.WriteLine();
            transferControl.WriteMessage(string.Format(Locale.IsRussian ?
                "Скачивание конфигурации приложения {0}" :
                "Download configuration of the {0} application", app.AppName));

            string sql = $"SELECT path, contents FROM {Schema}.app_config WHERE app_id = @appID" +
                (downloadOptions.IgnoreRegKeys ? $" AND path NOT LIKE '%{ScadaUtils.RegFileSuffix}'" : "");
            NpgsqlCommand cmd = new(sql, conn);
            cmd.Parameters.AddWithValue("appID", (int)app.ServiceApp);
            int fileCount = 0;
            using NpgsqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read() && !reader.IsDBNull(1))
            {
                transferControl.ThrowIfCancellationRequested();
                string path = reader.GetString(0);
                transferControl.WriteMessage(string.Format(Locale.IsRussian ?
                    "Скачивание файла конфигурации \"{0}\"" :
                    "Download configuration file \"{0}\"", path));

                string absolutePath = Path.Combine(app.ConfigDir, path);
                Directory.CreateDirectory(Path.GetDirectoryName(absolutePath));
                File.WriteAllText(absolutePath, reader.GetString(1), Encoding.UTF8);
                fileCount++;
            }

            transferControl.WriteMessage(string.Format(AdminPhrases.FileCount, fileCount));
            progressTracker.TaskIndex++;
        }


        /// <summary>
        /// Downloads the configuration.
        /// </summary>
        public void Download()
        {
            if (!profile.DbEnabled)
                throw new ScadaException(AdminPhrases.DbNotEnabled);

            transferControl.SetCancelEnabled(true);
            transferControl.WriteMessage(AdminPhrases.DownloadConfig);

            try
            {
                conn = CreateDbConnection(profile.DbConnectionOptions);
                conn.Open();

                if (downloadOptions.IncludeBase)
                    DownloadBase();
                else
                    progressTracker.SkipTask();

                if (downloadOptions.IncludeView)
                    DownloadViews();
                else
                    progressTracker.SkipTask();

                if (downloadOptions.IncludeServer)
                    DownloadAppConfig(instance.ServerApp);
                else
                    progressTracker.SkipTask();

                if (downloadOptions.IncludeComm)
                    DownloadAppConfig(instance.CommApp);
                else
                    progressTracker.SkipTask();

                if (downloadOptions.IncludeWeb)
                    DownloadAppConfig(instance.WebApp);
                else
                    progressTracker.SkipTask();
            }
            finally
            {
                conn?.Close();
                conn = null;
            }

            progressTracker.SetCompleted();
            transferControl.WriteLine();
            transferControl.WriteMessage(AdminPhrases.DownloadConfigCompleted);
        }
    }
}
