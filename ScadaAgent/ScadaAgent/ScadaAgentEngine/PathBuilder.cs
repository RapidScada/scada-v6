/*
 * Copyright 2023 Rapid Software LLC
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
 * Summary  : Converts logical objects of file paths to their string representations
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2023
 */

using Scada.Protocol;
using System;
using System.Collections.Generic;
using System.IO;

namespace Scada.Agent.Engine
{
    /// <summary>
    /// Converts logical objects of file paths to their string representations.
    /// <para>Преобразует логические объекты файловых путей в их строковые представления.</para>
    /// </summary>
    internal class PathBuilder
    {
        private readonly string instanceDir;    // the instance directory
        private readonly string instanceLogDir; // the instance log directory
        private readonly string agentDir;       // the full name of the Agent directory


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PathBuilder(string instanceDir, string instanceLogDir, string agentDir)
        {
            this.instanceDir = instanceDir ?? throw new ArgumentNullException(nameof(instanceDir));
            this.instanceLogDir = instanceLogDir ?? throw new ArgumentNullException(nameof(instanceLogDir));
            this.agentDir = agentDir ?? throw new ArgumentNullException(nameof(agentDir));
        }


        /// <summary>
        /// Converts the top folder to a string.
        /// </summary>
        private bool FolderToString(TopFolder topFolder, out string path)
        {
            switch (topFolder)
            {
                case TopFolder.Root:
                    path = "";
                    return false;

                case TopFolder.Base:
                    path = "BaseDAT";
                    return true;

                case TopFolder.View:
                    path = "Views";
                    return true;

                case TopFolder.Server:
                    path = "ScadaServer";
                    return true;

                case TopFolder.Comm:
                    path = "ScadaComm";
                    return true;

                case TopFolder.Web:
                    path = "ScadaWeb";
                    return true;

                case TopFolder.Agent:
                    path = "ScadaAgent";
                    return true;

                default:
                    throw new ScadaException("Top folder {0} not supported.", topFolder);
            }
        }

        /// <summary>
        /// Converts the application folder to a string.
        /// </summary>
        private bool FolderToString(AppFolder appFolder, out string path)
        {
            switch (appFolder)
            {
                case AppFolder.Root:
                    path = "";
                    return false;

                case AppFolder.Cmd:
                    path = "Cmd";
                    return true;

                case AppFolder.Config:
                    path = "Config";
                    return true;

                case AppFolder.Lang:
                    path = "Lang";
                    return true;

                case AppFolder.Log:
                    path = "Log";
                    return true;

                case AppFolder.Storage:
                    path = "Storage";
                    return true;

                case AppFolder.Temp:
                    path = "Temp";
                    return true;

                default:
                    throw new ScadaException("Application folder {0} not supported.", appFolder);
            }
        }

        /// <summary>
        /// Converts the application folder to a string.
        /// </summary>
        private bool FolderToString(AppFolder appFolder, bool lowercase, out string path)
        {
            if (FolderToString(appFolder, out path))
            {
                if (lowercase)
                    path = path.ToLowerInvariant();
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Gets the absolute path for the specified relative path.
        /// </summary>
        public string GetAbsolutePath(RelativePath relativePath)
        {
            List<string> paths = new List<string>();

            if (relativePath.TopFolder == TopFolder.Agent)
            {
                paths.Add(agentDir); // this Agent application
            }
            else
            {
                if (relativePath.AppFolder == AppFolder.Log && !string.IsNullOrEmpty(instanceLogDir))
                    paths.Add(instanceLogDir);
                else
                    paths.Add(instanceDir);

                if (FolderToString(relativePath.TopFolder, out string topPath))
                    paths.Add(topPath);
            }

            if (FolderToString(relativePath.AppFolder, relativePath.TopFolder == TopFolder.Web, out string appPath))
                paths.Add(appPath);

            if (!string.IsNullOrEmpty(relativePath.Path))
                paths.Add(relativePath.Path);

            return Path.Combine(paths.ToArray());
        }
    }
}
