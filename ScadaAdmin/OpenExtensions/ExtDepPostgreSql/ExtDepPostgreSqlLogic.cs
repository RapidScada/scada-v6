﻿// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Npgsql;
using Scada.Admin.Deployment;
using Scada.Admin.Lang;
using Scada.Admin.Project;
using Scada.Dbms;
using Scada.Lang;
using System;
using static Scada.Storages.PostgreSqlStorage.PostgreSqlStorageShared;

namespace Scada.Admin.Extensions.ExtDepPostgreSql
{
    /// <summary>
    /// Represents an extension logic.
    /// <para>Представляет логику расширения.</para>
    /// </summary>
    public class ExtDepPostgreSqlLogic : ExtensionLogic
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ExtDepPostgreSqlLogic(IAdminContext adminContext)
            : base(adminContext)
        {
            CanDeploy = true;
        }


        /// <summary>
        /// Gets the extension code.
        /// </summary>
        public override string Code
        {
            get
            {
                return "ExtDepPostgreSql";
            }
        }

        /// <summary>
        /// Gets the extension name.
        /// </summary>
        public override string Name
        {
            get
            {
                return Locale.IsRussian ? "Развёртывание в Postgre SQL" : "Deployment to Postgre SQL";
            }
        }

        /// <summary>
        /// Gets the extension description.
        /// </summary>
        public override string Descr
        {
            get
            {
                return Locale.IsRussian ?
                    "Расширение обеспечивает передачу проекта в базу данных Postgre SQL." :
                    "The extension provides project transfer to a Postgre SQL database.";
            }
        }


        /// <summary>
        /// Loads language dictionaries.
        /// </summary>
        public override void LoadDictionaries()
        {
            if (!Locale.LoadDictionaries(AdminContext.AppDirs.LangDir, Code, out string errMsg))
                AdminContext.ErrLog.WriteError(AdminPhrases.ExtensionMessage, Code, errMsg);

            ExtensionPhrases.Init();
        }

        /// <summary>
        /// Tests a database connection.
        /// </summary>
        public override bool TestDbConnection(DbConnectionOptions connectionOptions, out string errMsg)
        {
            if (connectionOptions.KnownDBMS == KnownDBMS.PostgreSQL)
            {
                NpgsqlConnection conn = null;

                try
                {
                    conn = CreateDbConnection(connectionOptions);
                    conn.Open();
                    errMsg = "";
                    return true;
                }
                catch (Exception ex)
                {
                    errMsg = ex.Message;
                    return false;
                }
                finally
                {
                    conn?.Close();
                }
            }
            else
            {
                errMsg = CommonPhrases.DatabaseNotSupported;
                return false;
            }
        }

        /// <summary>
        /// Downloads the configuration.
        /// </summary>
        public override void DownloadConfig(ScadaProject project, ProjectInstance instance, DeploymentProfile profile,
            ITransferControl transferControl)
        {
            new Downloader(project, instance, profile, transferControl).Download();
        }

        /// <summary>
        /// Uploads the configuration.
        /// </summary>
        public override void UploadConfig(ScadaProject project, ProjectInstance instance, DeploymentProfile profile,
            ITransferControl transferControl)
        {
            new Uploader(project, instance, profile, transferControl).Upload();
        }
    }
}
