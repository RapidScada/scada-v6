// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Npgsql;
using Scada.Admin.Deployment;
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
    /// Downloads the configuration.
    /// <para>Скачивает конфигурацию.</para>
    /// </summary>
    internal class Downloader
    {
        private readonly ScadaProject project;
        private readonly ProjectInstance instance;
        private readonly DeploymentProfile profile;
        private readonly ITransferControl transferControl;
        private readonly DownloadOptions downloadOptions;
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
            conn = null;
        }


        /// <summary>
        /// Downloads the configuration database.
        /// </summary>
        private void DownloadBase()
        {
            transferControl.WriteLine();
            transferControl.WriteMessage(Locale.IsRussian ?
                "Скачивание базы конфигурации" :
                "Download the configuration database");

            foreach (IBaseTable baseTable in project.ConfigBase.AllTables)
            {
                transferControl.WriteMessage(string.Format(Locale.IsRussian ?
                    "Скачивание таблицы {0}" :
                    "Download the {0} table", baseTable.Name));
                ReadBaseTable(baseTable, conn);
            }
        }

        /// <summary>
        /// Downloads view files.
        /// </summary>
        private void DownloadViews()
        {
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
                string path = reader.GetString(0);
                transferControl.WriteMessage(string.Format(Locale.IsRussian ?
                    "Скачивание представления \"{0}\"" :
                    "Download view \"{0}\"", path));

                string absolutePath = Path.Combine(project.Views.ViewDir, path);
                Directory.CreateDirectory(Path.GetDirectoryName(absolutePath));
                File.WriteAllBytes(absolutePath, (byte[])reader[1]);
                fileCount++;
            }

            transferControl.WriteMessage(string.Format(ExtensionPhrases.FileCount, fileCount));
        }

        /// <summary>
        /// Downloads the configuration of the specified application.
        /// </summary>
        private void DownloadAppConfig(ProjectApp app)
        {
            transferControl.WriteLine();
            transferControl.WriteMessage(string.Format(Locale.IsRussian ?
                "Скачивание конфигурации приложения {0}" :
                "Download configuration of the {0} application", app.AppName));

            string sql = $"SELECT path, contents FROM {Schema}.app_config WHERE app_id = @appID" +
                (downloadOptions.IgnoreRegKeys ? " AND path NOT LIKE '%_Reg.xml'" : "");
            NpgsqlCommand cmd = new(sql, conn);
            cmd.Parameters.AddWithValue("appID", (int)app.ServiceApp);
            int fileCount = 0;
            using NpgsqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read() && !reader.IsDBNull(1))
            {
                string path = reader.GetString(0);
                transferControl.WriteMessage(string.Format(Locale.IsRussian ?
                    "Скачивание файла конфигурации \"{0}\"" :
                    "Download configuration file \"{0}\"", path));

                string absolutePath = Path.Combine(app.ConfigDir, path);
                Directory.CreateDirectory(Path.GetDirectoryName(absolutePath));
                File.WriteAllText(absolutePath, reader.GetString(1), Encoding.UTF8);
                fileCount++;
            }

            transferControl.WriteMessage(string.Format(ExtensionPhrases.FileCount, fileCount));
        }


        /// <summary>
        /// Downloads the configuration.
        /// </summary>
        public bool Download()
        {
            if (!profile.DbEnabled)
                throw new ScadaException(ExtensionPhrases.DbNotEnabled);

            transferControl.SetCancelEnabled(true);
            transferControl.WriteMessage(Locale.IsRussian ?
                "Скачивание конфигурации" :
                "Download configutation");

            try
            {
                conn = CreateDbConnection(profile.DbConnectionOptions);
                conn.Open();

                if (downloadOptions.IncludeBase)
                    DownloadBase();

                if (downloadOptions.IncludeView)
                    DownloadViews();

                if (downloadOptions.IncludeServer)
                    DownloadAppConfig(instance.ServerApp);

                if (downloadOptions.IncludeComm)
                    DownloadAppConfig(instance.CommApp);

                if (downloadOptions.IncludeWeb)
                    DownloadAppConfig(instance.WebApp);
            }
            finally
            {
                conn?.Close();
                conn = null;
            }

            transferControl.WriteLine();
            transferControl.WriteMessage(Locale.IsRussian ?
                "Скачивание конфигурации завершено успешно" :
                "Configuration downloaded successfully");
            return true;
        }
    }
}
