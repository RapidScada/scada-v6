// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Deployment;
using Scada.Admin.Lang;
using Scada.Admin.Project;
using Scada.Agent;
using Scada.Agent.Client;
using Scada.Client;
using Scada.Data.Adapters;
using Scada.Data.Entities;
using Scada.Data.Tables;
using Scada.Lang;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Scada.Admin.Extensions.ExtDepAgent
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
        private const int TaskCount = 4;

        private readonly AdminDirs appDirs;
        private readonly ScadaProject project;
        private readonly ProjectInstance instance;
        private readonly DeploymentProfile profile;
        private readonly ITransferControl transferControl;
        private readonly UploadOptions uploadOptions;
        private readonly ProgressTracker progressTracker;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Uploader(AdminDirs appDirs, ScadaProject project, ProjectInstance instance, 
            DeploymentProfile profile, ITransferControl transferControl)
        {
            this.appDirs = appDirs ?? throw new ArgumentNullException(nameof(appDirs));
            this.project = project ?? throw new ArgumentNullException(nameof(project));
            this.instance = instance ?? throw new ArgumentNullException(nameof(instance));
            this.profile = profile ?? throw new ArgumentNullException(nameof(profile));
            this.transferControl = transferControl ?? throw new ArgumentNullException(nameof(transferControl));
            uploadOptions = profile.UploadOptions;
            progressTracker = new ProgressTracker(transferControl) { TaskCount = TaskCount };
        }


        /// <summary>
        /// Gets a name for a temporary file.
        /// </summary>
        private string GetTempFileName()
        {
            return Path.Combine(appDirs.TempDir, 
                AgentConst.UploadConfigPrefix + ScadaUtils.GenerateUniqueID() + ".zip");
        }

        /// <summary>
        /// Tests the connection with the Agent service.
        /// </summary>
        private void TestAgentConnection(IAgentClient agentClient)
        {
            transferControl.WriteMessage(ExtensionPhrases.TestAgentConn);

            if (!agentClient.TestConnection(out string errMsg))
                throw new ScadaException(errMsg);

            progressTracker.TaskIndex++;
        }

        /// <summary>
        /// Compresses the configuration into the specified file.
        /// </summary>
        private void CompressConfig(string destFileName)
        {
            transferControl.ThrowIfCancellationRequested();
            transferControl.WriteLine();
            transferControl.WriteMessage(ExtensionPhrases.CompressConfig);

            // archive path separator is '/'
            FileStream fileStream = null;
            ZipArchive zipArchive = null;

            try
            {
                fileStream = new FileStream(destFileName, FileMode.Create, FileAccess.Write, FileShare.Read);
                zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Create);

                bool ignoreRegKeys = uploadOptions.IgnoreRegKeys;
                bool filterByObj = uploadOptions.ObjectFilter.Count > 0;

                // archive the configuration database
                if (uploadOptions.IncludeBase)
                {
                    transferControl.ThrowIfCancellationRequested();
                    transferControl.WriteMessage(ExtensionPhrases.CompressBase);

                    foreach (IBaseTable srcTable in project.ConfigDatabase.AllTables)
                    {
                        transferControl.ThrowIfCancellationRequested();

                        // filter source table by objects if needed
                        IBaseTable baseTable = srcTable;

                        if (filterByObj)
                        {
                            if (srcTable.ItemType == typeof(Cnl))
                                baseTable = GetFilteredTable<Cnl>(srcTable, uploadOptions.ObjectFilter);
                            else if (srcTable.ItemType == typeof(View))
                                baseTable = GetFilteredTable<View>(srcTable, uploadOptions.ObjectFilter);
                        }

                        // convert table to DAT format
                        using Stream entryStream =
                            zipArchive.CreateEntry("BaseDAT/" + srcTable.FileNameDat, CompressionLevel.Fastest).Open();
                        BaseTableAdapter baseAdapter = new() { Stream = entryStream };
                        baseAdapter.Update(baseTable);
                    }
                }

                // archive views
                if (uploadOptions.IncludeView)
                {
                    transferControl.ThrowIfCancellationRequested();
                    transferControl.WriteMessage(ExtensionPhrases.CompressViews);

                    if (filterByObj)
                    {
                        PackFiles(zipArchive, project.Views.ViewDir, GetFilteredViews(project.ConfigDatabase.ViewTable, 
                            project.Views.ViewDir, uploadOptions.ObjectFilter), "Views/");
                    }
                    else
                    {
                        PackDirectory(zipArchive, project.Views.ViewDir, "Views/", ignoreRegKeys);
                    }
                }

                // archive Server configuration
                if (uploadOptions.IncludeServer && instance.ServerApp.Enabled)
                {
                    transferControl.ThrowIfCancellationRequested();
                    transferControl.WriteMessage(string.Format(ExtensionPhrases.CompressAppConfig, 
                        CommonPhrases.ServerAppName));
                    PackDirectory(zipArchive, instance.ServerApp.AppDir, "ScadaServer/", ignoreRegKeys);
                }

                // archive Communicator configuration
                if (uploadOptions.IncludeComm && instance.CommApp.Enabled)
                {
                    transferControl.ThrowIfCancellationRequested();
                    transferControl.WriteMessage(string.Format(ExtensionPhrases.CompressAppConfig,
                        CommonPhrases.CommAppName));
                    PackDirectory(zipArchive, instance.CommApp.AppDir, "ScadaComm/", ignoreRegKeys);
                }

                // archive Webstation configuration
                if (uploadOptions.IncludeWeb && instance.WebApp.Enabled)
                {
                    transferControl.ThrowIfCancellationRequested();
                    transferControl.WriteMessage(string.Format(ExtensionPhrases.CompressAppConfig,
                        CommonPhrases.WebAppName));
                    PackDirectory(zipArchive, instance.WebApp.AppDir, "ScadaWeb/", ignoreRegKeys);
                }

                // add project information
                transferControl.ThrowIfCancellationRequested();
                transferControl.WriteMessage(ExtensionPhrases.AddProjectInfo);

                using (Stream entryStream = 
                    zipArchive.CreateEntry(AgentConst.ProjectInfoEntry, CompressionLevel.Fastest).Open())
                {
                    using StreamWriter writer = new(entryStream, Encoding.UTF8);
                    writer.Write(project.GetInfo());
                }

                // add transfer options
                transferControl.ThrowIfCancellationRequested();
                transferControl.WriteMessage(ExtensionPhrases.AddTransferOptions);

                using (Stream entryStream = 
                    zipArchive.CreateEntry(AgentConst.UploadOptionsEntry, CompressionLevel.Fastest).Open())
                {
                    uploadOptions.Save(entryStream);
                }

                progressTracker.TaskIndex++;
            }
            finally
            {
                zipArchive?.Dispose();
                fileStream?.Dispose();
            }
        }

        /// <summary>
        /// Transfers the configuration file to the Agent appliction.
        /// </summary>
        private void TransferConfig(string srcFileName, IAgentClient agentClient)
        {
            transferControl.ThrowIfCancellationRequested();
            transferControl.WriteLine();
            transferControl.WriteMessage(ExtensionPhrases.TransferConfig);
            transferControl.WriteMessage(string.Format(ExtensionPhrases.ArchiveSize, 
                new FileInfo(srcFileName).Length.ToString("N0", Locale.Culture)));

            agentClient.UploadConfig(srcFileName, transferControl.CancellationToken);
            progressTracker.TaskIndex++;
        }

        /// <summary>
        /// Gets a new table from the source table filtered by objects.
        /// </summary>
        private static IBaseTable GetFilteredTable<T>(IBaseTable srcTable, List<int> objNums)
        {
            IBaseTable destTable = new BaseTable<T>(srcTable.PrimaryKey, srcTable.Title);

            foreach (int objNum in objNums)
            {
                foreach (object item in srcTable.SelectItems(new TableFilter("ObjNum", objNum), true))
                {
                    destTable.AddObject(item);
                }
            }

            return destTable;
        }

        /// <summary>
        /// Gets the existing view files filtered by objects.
        /// </summary>
        private static IEnumerable<string> GetFilteredViews(BaseTable<View> viewTable, string viewDir, 
            List<int> objNums)
        {
            foreach (int objNum in objNums)
            {
                foreach (View view in viewTable.SelectItems(new TableFilter("ObjNum", objNum), true))
                {
                    string fileName = Path.Combine(viewDir, view.Path);

                    if (File.Exists(fileName))
                        yield return fileName;
                }
            }
        }

        /// <summary>
        /// Adds the specified files to the archive.
        /// </summary>
        private void PackFiles(ZipArchive zipArchive, string srcDir, IEnumerable<string> srcFileNames, 
            string entryPrefix)
        {
            int srcDirLen = srcDir.Length;

            foreach (string srcFileName in srcFileNames)
            {
                transferControl.ThrowIfCancellationRequested();
                string entryName = entryPrefix + srcFileName[srcDirLen..].Replace('\\', '/');
                zipArchive.CreateEntryFromFile(srcFileName, entryName, CompressionLevel.Fastest);
            }
        }

        /// <summary>
        /// Adds the directory content to the archive.
        /// </summary>
        private void PackDirectory(ZipArchive zipArchive, string srcDir, string entryPrefix, bool ignoreRegKeys)
        {
            DirectoryInfo srcDirInfo = new(srcDir);

            if (srcDirInfo.Exists)
            {
                int srcDirLen = srcDir.Length;

                foreach (FileInfo fileInfo in srcDirInfo.EnumerateFiles("*", SearchOption.AllDirectories))
                {
                    transferControl.ThrowIfCancellationRequested();

                    if (ignoreRegKeys && fileInfo.Name.EndsWith(ScadaUtils.RegFileSuffix))
                        continue;

                    string entryName = entryPrefix + fileInfo.FullName[srcDirLen..].Replace('\\', '/');
                    zipArchive.CreateEntryFromFile(fileInfo.FullName, entryName, CompressionLevel.Fastest);
                }
            }
        }


        /// <summary>
        /// Uploads the configuration.
        /// </summary>
        public void Upload()
        {
            if (!profile.AgentEnabled)
                throw new ScadaException(AdminPhrases.AgentNotEnabled);

            transferControl.SetCancelEnabled(true);
            transferControl.WriteMessage(AdminPhrases.UploadConfig);

            string tempFileName = GetTempFileName();

            try
            {
                AgentClient agentClient = new(profile.AgentConnectionOptions);
                agentClient.Progress += (object sender, ProgressEventArgs e) =>
                    progressTracker.UpdateSubtask(e.BlockNumber - 1, e.BlockCount);

                TestAgentConnection(agentClient);
                CompressConfig(tempFileName);
                TransferConfig(tempFileName, agentClient);

                new ServiceStarter(agentClient, uploadOptions, transferControl, progressTracker)
                    .SetProcessTimeout(profile.AgentConnectionOptions.Timeout)
                    .RestartServices();

                agentClient.TerminateSession();
            }
            finally
            {
                try { File.Delete(tempFileName); }
                catch { }
            }

            progressTracker.SetCompleted();
            transferControl.WriteLine();
            transferControl.WriteMessage(AdminPhrases.UploadConfigCompleted);
        }
    }
}
