// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Npgsql;
using Scada.Admin.Deployment;
using Scada.Admin.Project;
using Scada.Config;
using Scada.Lang;
using System;
using System.Threading;
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
        public override bool DownloadConfig(ScadaProject project, ProjectInstance instance, DeploymentProfile profile,
            ITransferControl transferControl)
        {
            transferControl.SetCancelEnabled(false);

            for (int i = 1, max = 100; i <= max; i++)
            {
                if (transferControl.CancellationToken.IsCancellationRequested)
                    return false;

                transferControl.WriteMessage($"i = {i} of {max}");
                transferControl.SetProgress(i * 100 / max);
                Thread.Sleep(50);
            }

            return true;
        }

        /// <summary>
        /// Uploads the configuration.
        /// </summary>
        public override bool UploadConfig(ScadaProject project, ProjectInstance instance, DeploymentProfile profile,
            ITransferControl transferControl)
        {
            transferControl.SetCancelEnabled(true);

            for (int i = 1, max = 100; i <= max; i++)
            {
                if (transferControl.CancellationToken.IsCancellationRequested)
                    return false;

                transferControl.WriteMessage($"i = {i} of {max}");
                transferControl.SetProgress(i * 100 / max);
                Thread.Sleep(50);
            }

            return true;
        }
    }
}
