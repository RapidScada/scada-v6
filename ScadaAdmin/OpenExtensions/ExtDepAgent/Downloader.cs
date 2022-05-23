// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Deployment;
using Scada.Admin.Lang;
using Scada.Admin.Project;
using Scada.Agent;
using Scada.Agent.Client;
using Scada.Client;
using Scada.Data.Adapters;
using Scada.Data.Tables;
using Scada.Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Scada.Admin.Extensions.ExtDepAgent
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

        private readonly AdminDirs appDirs;
        private readonly ScadaProject project;
        private readonly ProjectInstance instance;
        private readonly DeploymentProfile profile;
        private readonly ITransferControl transferControl;
        private readonly DownloadOptions downloadOptions;
        private readonly ProgressTracker progressTracker;
        
        private AgentClient agentClient;
        private List<string> tempFileNames;
        private List<string> extractDirs;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Downloader(AdminDirs appDirs, ScadaProject project, ProjectInstance instance, 
            DeploymentProfile profile, ITransferControl transferControl)
        {
            this.appDirs = appDirs ?? throw new ArgumentNullException(nameof(appDirs));
            this.project = project ?? throw new ArgumentNullException(nameof(project));
            this.instance = instance ?? throw new ArgumentNullException(nameof(instance));
            this.profile = profile ?? throw new ArgumentNullException(nameof(profile));
            this.transferControl = transferControl ?? throw new ArgumentNullException(nameof(transferControl));
            downloadOptions = profile.DownloadOptions;
            progressTracker = new ProgressTracker(transferControl) { TaskCount = TaskCount };

            agentClient = null;
            tempFileNames = null;
            extractDirs = null;
        }


        /// <summary>
        /// Gets a name for a temporary file and extraction directory.
        /// </summary>
        private void GetTempFileName(out string tempFileName, out string extractDir)
        {
            string s = AgentConst.DownloadConfigPrefix + ScadaUtils.GenerateUniqueID();
            tempFileName = Path.Combine(appDirs.TempDir, s + ".zip");
            extractDir = Path.Combine(appDirs.TempDir, s);
            tempFileNames.Add(tempFileName);
            extractDirs.Add(extractDir);
        }
        
        /// <summary>
        /// Downloads the configuration database.
        /// </summary>
        private void DownloadBase()
        {
            transferControl.ThrowIfCancellationRequested();
            transferControl.WriteLine();
            transferControl.WriteMessage(AdminPhrases.DownloadBase);

            GetTempFileName(out string tempFileName, out string extractDir);
            agentClient.DownloadConfig(tempFileName, TopFolder.Base);
            ExtractArchive(tempFileName, extractDir, false);

            foreach (IBaseTable baseTable in project.ConfigDatabase.AllTables)
            {
                transferControl.ThrowIfCancellationRequested();
                transferControl.WriteMessage(string.Format(ExtensionPhrases.ImportTable, baseTable.Name));
                string datFileName = Path.Combine(extractDir, "BaseDAT", baseTable.FileNameDat);

                if (File.Exists(datFileName))
                {
                    BaseTableAdapter baseAdapter = new() { FileName = datFileName };
                    IBaseTable srcTable = BaseTableFactory.GetBaseTable(baseTable);
                    baseAdapter.Fill(srcTable);
                    baseTable.Modified = true;

                    foreach (object item in srcTable.EnumerateItems())
                    {
                        baseTable.AddObject(item);
                    }
                }
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
            transferControl.WriteMessage(AdminPhrases.DownloadViews);

            GetTempFileName(out string tempFileName, out string extractDir);
            agentClient.DownloadConfig(tempFileName, TopFolder.View);
            ExtractArchive(tempFileName, extractDir, false);
            MergeDirectory(extractDir, project.ProjectDir);
            progressTracker.TaskIndex++;
        }

        /// <summary>
        /// Downloads the configuration of the specified application.
        /// </summary>
        private void DownloadAppConfig(ProjectApp app)
        {
            transferControl.ThrowIfCancellationRequested();
            transferControl.WriteLine();
            transferControl.WriteMessage(string.Format(AdminPhrases.DownloadAppConfig, app.AppName));

            GetTempFileName(out string tempFileName, out string extractDir);
            agentClient.DownloadConfig(tempFileName, RelativePath.GetTopFolder(app.ServiceApp));
            ExtractArchive(tempFileName, extractDir, downloadOptions.IgnoreRegKeys);
            MergeDirectory(extractDir, instance.InstanceDir);
            progressTracker.TaskIndex++;
        }

        /// <summary>
        /// Extracts the specified archive.
        /// </summary>
        private void ExtractArchive(string srcFileName, string destDir, bool ignoreRegKeys)
        {
            transferControl.ThrowIfCancellationRequested();
            transferControl.WriteMessage(ExtensionPhrases.ExtractArchive);

            using FileStream fileStream = new(srcFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            using ZipArchive zipArchive = new(fileStream, ZipArchiveMode.Read);

            foreach (ZipArchiveEntry zipEntry in zipArchive.Entries)
            {
                transferControl.ThrowIfCancellationRequested();

                if (ignoreRegKeys && zipEntry.Name.EndsWith(ScadaUtils.RegFileSuffix))
                    continue;

                string entryName = zipEntry.FullName.Replace('/', Path.DirectorySeparatorChar);
                string absPath = Path.Combine(destDir, entryName);
                Directory.CreateDirectory(Path.GetDirectoryName(absPath));

                if (entryName[^1] != Path.DirectorySeparatorChar)
                    zipEntry.ExtractToFile(absPath, true);
            }
        }

        /// <summary>
        /// Moves the files overwriting the existing files.
        /// </summary>
        private void MergeDirectory(string srcDirName, string destDirName)
        {
            transferControl.ThrowIfCancellationRequested();

            if (Directory.Exists(srcDirName))
            {
                transferControl.WriteMessage(ExtensionPhrases.MergeDirectory);
                MergeDirectory(new DirectoryInfo(srcDirName), new DirectoryInfo(destDirName));
            }
            else
            {
                transferControl.WriteMessage(ExtensionPhrases.NothingToMerge);
            }
        }

        /// <summary>
        /// Deletes temporary files and directories.
        /// </summary>
        private void DeleteTempFiles()
        {
            foreach (string tempFileName in tempFileNames)
            {
                try { File.Delete(tempFileName); }
                catch { }
            }

            foreach (string extractDir in extractDirs)
            {
                try
                {
                    if (Directory.Exists(extractDir))
                        Directory.Delete(extractDir, true);
                }
                catch
                {
                    // do nothing
                }
            }
        }
        
        /// <summary>
        /// Recursively moves the files overwriting the existing files.
        /// </summary>
        private static void MergeDirectory(DirectoryInfo srcDirInfo, DirectoryInfo destDirInfo)
        {
            // do not interrupt merge
            // create necessary directories
            if (!destDirInfo.Exists)
                destDirInfo.Create();

            foreach (DirectoryInfo srcSubdirInfo in srcDirInfo.GetDirectories())
            {
                DirectoryInfo destSubdirInfo = new DirectoryInfo(
                    Path.Combine(destDirInfo.FullName, srcSubdirInfo.Name));

                MergeDirectory(srcSubdirInfo, destSubdirInfo);
            }

            // move files
            foreach (FileInfo srcFileInfo in srcDirInfo.GetFiles())
            {
                string destFileName = Path.Combine(destDirInfo.FullName, srcFileInfo.Name);

                if (File.Exists(destFileName))
                    File.Delete(destFileName);

                srcFileInfo.MoveTo(destFileName);
            }
        }


        /// <summary>
        /// Downloads the configuration.
        /// </summary>
        public void Download()
        {
            if (!profile.AgentEnabled)
                throw new ScadaException(AdminPhrases.AgentNotEnabled);

            transferControl.SetCancelEnabled(true);
            transferControl.WriteMessage(AdminPhrases.DownloadConfig);

            tempFileNames = new List<string>();
            extractDirs = new List<string>();

            try
            {
                agentClient = new AgentClient(profile.AgentConnectionOptions);
                agentClient.Progress += (object sender, ProgressEventArgs e) =>
                    progressTracker.UpdateSubtask(e.BlockNumber - 1, e.BlockCount);

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

                agentClient.TerminateSession();
            }
            finally
            {
                DeleteTempFiles();
            }

            progressTracker.SetCompleted();
            transferControl.WriteLine();
            transferControl.WriteMessage(AdminPhrases.DownloadConfigCompleted);
        }
    }
}
