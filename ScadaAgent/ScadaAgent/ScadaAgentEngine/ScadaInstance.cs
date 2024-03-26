/*
 * Copyright 2024 Rapid Software LLC
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * 
 * Product  : Rapid SCADA
 * Module   : ScadaAgentEngine
 * Summary  : Controls an instance that includes of one or more applications
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2023
 */

using Scada.Agent.Config;
using Scada.Config;
using Scada.Data.Models;
using Scada.Lang;
using Scada.Log;
using Scada.Protocol;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;

namespace Scada.Agent.Engine
{
    /// <summary>
    /// Controls an instance that includes of one or more applications.
    /// <para>Управляет экземпляром, включающим из одно или несколько приложений.</para>
    /// </summary>
    internal class ScadaInstance
    {
        /// <summary>
        /// The number of milliseconds to wait for lock.
        /// </summary>
        private const int LockTimeout = 1000;

        private readonly ILog log;                        // the application log
        private readonly InstanceOptions instanceOptions; // the instance options from the Agent configuration
        private readonly InstanceConfig instanceConfig;   // the instance configuration used by the applications
        private readonly ReaderWriterLockSlim configLock; // synchronizes access to the instance configuration


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ScadaInstance(AppDirs appDirs, ILog log, InstanceOptions instanceOptions, InstanceConfig instanceConfig)
        {
            if (appDirs == null)
                throw new ArgumentNullException(nameof(appDirs));

            this.log = log ?? throw new ArgumentNullException(nameof(log));
            this.instanceOptions = instanceOptions ?? throw new ArgumentNullException(nameof(instanceOptions));
            this.instanceConfig = instanceConfig ?? throw new ArgumentNullException(nameof(instanceConfig));
            configLock = new ReaderWriterLockSlim();
            PathBuilder = new PathBuilder(instanceOptions.Directory, instanceConfig.LogDir, appDirs.ExeDir);
        }


        /// <summary>
        /// Gets the instance name.
        /// </summary>
        public string Name => instanceOptions.Name;

        /// <summary>
        /// Gets a value indicating whether the instance is deployed on another host.
        /// </summary>
        public bool ProxyMode => instanceOptions.ProxyMode;

        /// <summary>
        /// Gets the path builder.
        /// </summary>
        public PathBuilder PathBuilder { get; }


        /// <summary>
        /// Gets the file name that contains the service status.
        /// </summary>
        private string GetStatusFileName(ServiceApp serviceApp)
        {
            switch (serviceApp)
            {
                case ServiceApp.Server:
                    return "ScadaServer.txt";
                case ServiceApp.Comm:
                    return "ScadaComm.txt";
                default:
                    throw new ArgumentException("Service not supported.");
            }
        }

        /// <summary>
        /// Gets the file name of the service command.
        /// </summary>
        private string GetCommandFileName(ServiceCommand command)
        {
            string ext = ScadaUtils.IsRunningOnWin ? ".bat" : ".sh";

            switch (command)
            {
                case ServiceCommand.Start:
                    return "svc_start" + ext;
                case ServiceCommand.Stop:
                    return "svc_stop" + ext;
                case ServiceCommand.Restart:
                    return "svc_restart" + ext;
                default:
                    throw new ArgumentException("Command not supported.");
            }
        }

        /// <summary>
        /// Gets the directories affected by the uploaded configuration.
        /// </summary>
        private List<string> GetAffectedDirectories(TransferOptions uploadOptions)
        {
            List<string> dirs = new List<string>();

            void AddDir(RelativePath relativePath)
            {
                dirs.Add(ScadaUtils.NormalDir(PathBuilder.GetAbsolutePath(relativePath)));
            }

            if (uploadOptions.IncludeBase)
                AddDir(new RelativePath(TopFolder.Base));

            if (uploadOptions.IncludeView)
                AddDir(new RelativePath(TopFolder.View));

            if (uploadOptions.IncludeServer)
                AddDir(new RelativePath(TopFolder.Server, AppFolder.Config));

            if (uploadOptions.IncludeComm)
                AddDir(new RelativePath(TopFolder.Comm, AppFolder.Config));

            if (uploadOptions.IncludeWeb)
                AddDir(new RelativePath(TopFolder.Web, AppFolder.Config));

            return dirs;
        }

        /// <summary>
        /// Recursively packs the directory to the archive.
        /// </summary>
        private static void PackDirectory(ZipArchive zipArchive, string path, string entryPrefix)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(path);

            if (dirInfo.Exists)
            {
                // pack subdirectories
                foreach (DirectoryInfo subdirInfo in dirInfo.EnumerateDirectories("*", SearchOption.TopDirectoryOnly))
                {
                    PackDirectory(zipArchive, subdirInfo.FullName, entryPrefix + subdirInfo.Name + '/');
                }

                // pack files
                int dirLen = ScadaUtils.NormalDir(path).Length;
                foreach (FileInfo fileInfo in dirInfo.EnumerateFiles("*", SearchOption.TopDirectoryOnly))
                {
                    string entryName = entryPrefix + fileInfo.FullName.Substring(dirLen).Replace('\\', '/');
                    zipArchive.CreateEntryFromFile(fileInfo.FullName, entryName, CompressionLevel.Fastest);
                }
            }
        }

        /// <summary>
        /// Clears the specified directory.
        /// </summary>
        private static void ClearDirectory(string path, bool keepRegKeys)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(path);

            if (dirInfo.Exists)
                ClearDirectory(dirInfo, keepRegKeys, out _);
        }

        /// <summary>
        /// Recursively clears the specified directory.
        /// </summary>
        private static void ClearDirectory(DirectoryInfo dirInfo, bool keepRegKeys, out bool dirEmpty)
        {
            // delete subdirectories
            foreach (DirectoryInfo subdirInfo in dirInfo.EnumerateDirectories("*", SearchOption.TopDirectoryOnly))
            {
                ClearDirectory(subdirInfo, keepRegKeys, out bool subdirEmpty);

                if (subdirEmpty)
                    subdirInfo.Delete();
            }

            // delete files
            dirEmpty = true;

            foreach (FileInfo fileInfo in dirInfo.EnumerateFiles("*", SearchOption.AllDirectories))
            {
                if (keepRegKeys &&
                    fileInfo.Name.EndsWith(ScadaUtils.RegFileSuffix, StringComparison.OrdinalIgnoreCase))
                {
                    dirEmpty = false;
                }
                else
                {
                    fileInfo.Delete();
                }
            }
        }


        /// <summary>
        /// Validates the username and password.
        /// </summary>
        public UserValidationResult ValidateUser(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return UserValidationResult.Fail(Locale.IsRussian ?
                    "Имя пользователя или пароль не может быть пустым" :
                    "Username or password can not be empty");
            }

            if (string.Equals(instanceOptions.AdminUser.Username, username, StringComparison.OrdinalIgnoreCase))
            {
                if (instanceOptions.AdminUser.Password == password)
                {
                    return new UserValidationResult
                    {
                        IsValid = true,
                        RoleID = AgentRoleID.Administrator
                    };
                }
            }

            if (instanceOptions.ProxyMode && 
                string.Equals(instanceOptions.AgentUser.Username, username, StringComparison.OrdinalIgnoreCase))
            {
                if (instanceOptions.AgentUser.Password == password)
                {
                    return new UserValidationResult
                    {
                        IsValid = true,
                        RoleID = AgentRoleID.Agent
                    };
                }
            }

            return UserValidationResult.Fail(Locale.IsRussian ?
                "Неверное имя пользователя или пароль" :
                "Invalid username or password");
        }

        /// <summary>
        /// Gets the current status of the specified service.
        /// </summary>
        public ServiceStatus GetServiceStatus(ServiceApp serviceApp)
        {
            try
            {
                string statusFileName = PathBuilder.GetAbsolutePath(
                    new RelativePath(serviceApp, AppFolder.Log, GetStatusFileName(serviceApp)));

                if (File.Exists(statusFileName))
                {
                    using (FileStream stream = 
                        new FileStream(statusFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                        {
                            const int MaxLineCount = 10;
                            int lineCount = 0;

                            while (!reader.EndOfStream && lineCount < MaxLineCount)
                            {
                                string line = reader.ReadLine();
                                lineCount++;

                                if (line.StartsWith("Status", StringComparison.Ordinal) ||
                                    line.StartsWith("Статус", StringComparison.Ordinal))
                                {
                                    int colonIdx = line.IndexOf(':');

                                    if (colonIdx >= 0)
                                    {
                                        string s = line.Substring(colonIdx + 1).Trim();
                                        return ScadaUtils.ParseServiceStatus(s);
                                    }

                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.WriteError(ex, Locale.IsRussian ?
                   "Ошибка при получении статуса службы" :
                   "Error getting service status");
            }

            return ServiceStatus.Undefined;
        }

        /// <summary>
        /// Sends the command to the service.
        /// </summary>
        public bool ControlService(ServiceApp serviceApp, ServiceCommand cmd, int timeout)
        {
            try
            {
                if (configLock.TryEnterReadLock(LockTimeout))
                {
                    try
                    {
                        string batchFileName = PathBuilder.GetAbsolutePath(
                            new RelativePath(serviceApp, AppFolder.Root, GetCommandFileName(cmd)));

                        if (File.Exists(batchFileName))
                        {
                            ProcessStartInfo startInfo = new ProcessStartInfo
                            {
                                FileName = batchFileName,
                                UseShellExecute = false
                            };

                            using (Process process = Process.Start(startInfo))
                            {
                                if (timeout <= 0)
                                {
                                    return true;
                                }
                                else if (!process.WaitForExit(timeout))
                                {
                                    log.WriteError(Locale.IsRussian ?
                                        "Процесс не завершился за {0} мс. Файл {0}" :
                                        "Process did not complete in {0} ms. File {0}", timeout, batchFileName);
                                }
                                else if (process.ExitCode == 0)
                                {
                                    return true;
                                }
                                else
                                {
                                    log.WriteError(Locale.IsRussian ?
                                        "Процесс завершён с ошибкой. Файл {0}" :
                                        "Process completed with an error. File {0}", batchFileName);
                                }
                            }
                        }
                        else
                        {
                            log.WriteError(Locale.IsRussian ?
                                "Не найден файл команды управления службой {0}" :
                                "Service control command file not found {0}", batchFileName);
                        }
                    }
                    finally
                    {
                        configLock.ExitReadLock();
                    }
                }
                else
                {
                    log.WriteError(EnginePhrases.InstanceLocked, nameof(ControlService), Name);
                }
            }
            catch (Exception ex)
            {
                log.WriteError(ex, Locale.IsRussian ?
                   "Ошибка при отправке команды управления службой" :
                   "Error sending service control command");
            }

            return false;
        }

        /// <summary>
        /// Packs the configuration to the archive.
        /// </summary>
        public bool PackConfig(string destFileName, RelativePath configFolder)
        {
            try
            {
                configFolder.Path = "";
                string configDir = ScadaUtils.NormalDir(PathBuilder.GetAbsolutePath(configFolder));
                int instanceDirLen = ScadaUtils.NormalDir(instanceOptions.Directory).Length;
                string entryPrefix = configDir.Substring(instanceDirLen).Replace('\\', '/');

                using (FileStream fileStream =
                    new FileStream(destFileName, FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                    using (ZipArchive zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Create))
                    {
                        PackDirectory(zipArchive, configDir, entryPrefix);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при упаковке конфигурации в архив" :
                    "Error packing configuration into archive");
                return false;
            }
        }

        /// <summary>
        /// Unpacks the configuration from the archive.
        /// </summary>
        public bool UnpackConfig(string srcFileName)
        {
            try
            {
                using (FileStream fileStream =
                    new FileStream(srcFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (ZipArchive zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Read))
                    {
                        // get upload options
                        TransferOptions uploadOptions = new TransferOptions();
                        ZipArchiveEntry optionsEntry = zipArchive.GetEntry(AgentConst.UploadOptionsEntry) ??
                            throw new ScadaException(Locale.IsRussian ?
                                "Параметры передачи не найдены." :
                                "Upload options not found.");

                        using (Stream optionsStream = optionsEntry.Open())
                        {
                            uploadOptions.Load(optionsStream);
                        }

                        // delete existing configuration
                        List<string> affectedDirs = GetAffectedDirectories(uploadOptions);
                        affectedDirs.ForEach(dir => ClearDirectory(dir, uploadOptions.IgnoreRegKeys));
                        File.Delete(Path.Combine(instanceOptions.Directory, AgentConst.ProjectInfoEntry));

                        // unpack configuration
                        foreach (ZipArchiveEntry entry in zipArchive.Entries)
                        {
                            string entryPath = entry.FullName.Replace('/', Path.DirectorySeparatorChar);
                            string destFileName = Path.Combine(instanceOptions.Directory, entryPath);

                            if (affectedDirs.Any(s => destFileName.StartsWith(s, StringComparison.Ordinal)) ||
                                entry.FullName == AgentConst.ProjectInfoEntry)
                            {
                                Directory.CreateDirectory(Path.GetDirectoryName(destFileName));
                                entry.ExtractToFile(destFileName, true);
                            }
                        }

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при распаковке конфигурации из архива" :
                    "Error unpacking configuration from archive");
                return false;
            }
        }
    }
}
