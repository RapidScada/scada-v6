/*
 * Copyright 2021 Rapid Software LLC
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
 * Modified : 2021
 */

using Scada.Admin.Extensions;
using Scada.Admin.Lang;
using Scada.Agent;
using Scada.Log;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        private readonly object extensionLock;            // synchronizes access to the extensions


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ExtensionHolder(ILog log)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            extensions = new List<ExtensionLogic>();
            extensionMap = new Dictionary<string, ExtensionLogic>();
            extensionLock = new object();
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
        public ICollection<ToolStripMenuItem> GetMainMenuItems()
        {
            lock (extensionLock)
            {
                List<ToolStripMenuItem> items = new();

                foreach (ExtensionLogic extensionLogic in extensions)
                {
                    try
                    {
                        items.AddRange(extensionLogic.GetMainMenuItems());
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
        public ICollection<ToolStripButton> GetToobarButtons()
        {
            lock (extensionLock)
            {
                List<ToolStripButton> items = new();

                foreach (ExtensionLogic extensionLogic in extensions)
                {
                    try
                    {
                        items.AddRange(extensionLogic.GetToobarButtons());
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
                        foreach (KeyValuePair<string, Image> pair in extensionLogic.GetTreeViewImages())
                        {
                            if (!items.ContainsKey(pair.Key))
                                items.Add(pair.Key, pair.Value);
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
    }
}
