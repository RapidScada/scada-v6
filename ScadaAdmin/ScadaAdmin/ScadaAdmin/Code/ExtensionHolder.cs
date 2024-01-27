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
 * Module   : Administrator
 * Summary  : Holds active extensions and helps to call their methods
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2022
 */

using Scada.Admin.Extensions;
using Scada.Admin.Lang;
using Scada.Agent;
using Scada.Log;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Scada.Admin.App.Code
{
    /// <summary>
    /// Holds active extensions and helps to call their methods.
    /// <para>Содержит активные расширения и помогает вызывать их методы.</para>
    /// </summary>
    public class ExtensionHolder
    {
        private readonly ILog log;                        // the application log
        private readonly List<ExtensionLogic> extensions; // all the extensions
        private readonly Dictionary<string, ExtensionLogic> extensionMap; // the extensions accessed by code
        private readonly Dictionary<string, ExtensionLogic> fileExtMap;   // the extensions accessed by file extension
        private readonly object extensionLock;            // synchronizes access to the extensions


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ExtensionHolder(ILog log)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            extensions = new List<ExtensionLogic>();
            extensionMap = new Dictionary<string, ExtensionLogic>();
            fileExtMap = new Dictionary<string, ExtensionLogic>();
            extensionLock = new object();
        }


        /// <summary>
        /// Retrieves file extensions from the specified extension and adds them to the map.
        /// </summary>
        private void RetrieveFileExtensions(ExtensionLogic extensionLogic)
        {
            ICollection<string> fileExtensions = extensionLogic.FileExtensions;

            if (fileExtensions != null)
            {
                foreach (string ext in fileExtensions)
                {
                    if (!string.IsNullOrEmpty(ext))
                    {
                        string extL = ext.ToLowerInvariant();

                        if (!fileExtMap.ContainsKey(extL))
                        {
                            fileExtMap.Add(extL, extensionLogic);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Adds the specified extension to the lists.
        /// </summary>
        public void AddExtension(ExtensionLogic extensionLogic)
        {
            if (extensionLogic == null)
                throw new ArgumentNullException(nameof(extensionLogic));

            if (extensionMap.ContainsKey(extensionLogic.Code))
                throw new ScadaException("Extension already exists.");

            extensions.Add(extensionLogic);
            extensionMap.Add(extensionLogic.Code, extensionLogic);
            RetrieveFileExtensions(extensionLogic);
        }

        /// <summary>
        /// Gets the extension by code.
        /// </summary>
        public bool GetExtension(string extensionCode, out ExtensionLogic extensionLogic)
        {
            return extensionMap.TryGetValue(extensionCode, out extensionLogic);
        }

        /// <summary>
        /// Calls the LoadDictionaries method of the plugins.
        /// </summary>
        public void LoadDictionaries()
        {
            lock (extensionLock)
            {
                foreach (ExtensionLogic extensionLogic in extensions)
                {
                    try
                    {
                        extensionLogic.LoadDictionaries();
                    }
                    catch (Exception ex)
                    {
                        log.WriteError(ex, AdminPhrases.ErrorInExtension, 
                            nameof(LoadDictionaries), extensionLogic.Code);
                    }
                }
            }
        }

        /// <summary>
        /// Calls the LoadConfig method of the plugins.
        /// </summary>
        public void LoadConfig()
        {
            lock (extensionLock)
            {
                foreach (ExtensionLogic extensionLogic in extensions)
                {
                    try
                    {
                        extensionLogic.LoadConfig();
                    }
                    catch (Exception ex)
                    {
                        log.WriteError(ex, AdminPhrases.ErrorInExtension, nameof(LoadConfig), extensionLogic.Code);
                    }
                }
            }
        }

        /// <summary>
        /// Calls the GetMainMenuItems method of the extensions.
        /// </summary>
        public ICollection<ToolStripItem> GetMainMenuItems()
        {
            lock (extensionLock)
            {
                List<ToolStripItem> items = new();

                foreach (ExtensionLogic extensionLogic in extensions)
                {
                    try
                    {
                        if (extensionLogic.GetMainMenuItems() is ToolStripItem[] menuItems)
                            items.AddRange(menuItems);
                    }
                    catch (Exception ex)
                    {
                        log.WriteError(ex, AdminPhrases.ErrorInExtension, 
                            nameof(GetMainMenuItems), extensionLogic.Code);
                    }
                }

                return items;
            }
        }

        /// <summary>
        /// Calls the GetToobarButtons method of the extensions.
        /// </summary>
        public ICollection<ToolStripItem> GetToobarButtons()
        {
            lock (extensionLock)
            {
                List<ToolStripItem> items = new();

                foreach (ExtensionLogic extensionLogic in extensions)
                {
                    try
                    {
                        if (extensionLogic.GetToobarButtons() is ToolStripItem[] buttons)
                        {
                            items.Add(new ToolStripSeparator());
                            items.AddRange(buttons);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteError(ex, AdminPhrases.ErrorInExtension, 
                            nameof(GetToobarButtons), extensionLogic.Code);
                    }
                }

                return items;
            }
        }

        /// <summary>
        /// Calls the GetTreeNodes method of the extensions.
        /// </summary>
        public ICollection<TreeNode> GetTreeNodes(object relatedObject)
        {
            lock (extensionLock)
            {
                List<TreeNode> items = new();

                foreach (ExtensionLogic extensionLogic in extensions)
                {
                    try
                    {
                        if (extensionLogic.GetTreeNodes(relatedObject) is TreeNode[] treeNodes)
                            items.AddRange(treeNodes);
                    }
                    catch (Exception ex)
                    {
                        log.WriteError(ex, AdminPhrases.ErrorInExtension, nameof(GetTreeNodes), extensionLogic.Code);
                    }
                }

                return items;
            }
        }

        /// <summary>
        /// Calls the GetTreeViewImages method of the extensions.
        /// </summary>
        public IDictionary<string, Image> GetTreeViewImages()
        {
            lock (extensionLock)
            {
                Dictionary<string, Image> items = new();

                foreach (ExtensionLogic extensionLogic in extensions)
                {
                    try
                    {
                        if (extensionLogic.GetTreeViewImages() is Dictionary<string, Image> images)
                        {
                            foreach (KeyValuePair<string, Image> pair in images)
                            {
                                if (!items.ContainsKey(pair.Key))
                                    items.Add(pair.Key, pair.Value);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteError(ex, AdminPhrases.ErrorInExtension, 
                            nameof(GetTreeViewImages), extensionLogic.Code);
                    }
                }

                return items;
            }
        }

        /// <summary>
        /// Calls the GetEditorForm method of the corresponding extension.
        /// </summary>
        public Form GetEditorForm(string fileName)
        {
            if (fileExtMap.TryGetValue(AppUtils.GetExtensionLower(fileName), out ExtensionLogic extensionLogic))
            {
                try
                {
                    Monitor.Enter(extensionLock);
                    return extensionLogic.GetEditorForm(fileName);
                }
                catch (Exception ex)
                {
                    log.WriteError(ex, AdminPhrases.ErrorInExtension, nameof(GetEditorForm), extensionLogic.Code);
                }
                finally
                {
                    Monitor.Exit(extensionLock);
                }
            }

            return null;
        }
    }
}
