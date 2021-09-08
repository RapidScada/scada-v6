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
 * Module   : ScadaAdminCommon
 * Summary  : Represents the base class for extension logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Scada.Admin.Extensions
{
    /// <summary>
    /// Represents the base class for extension logic.
    /// <para>Представляет базовый класс логики расширения.</para>
    /// </summary>
    public abstract class ExtensionLogic
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ExtensionLogic(IAdminContext adminContext)
        {
            AdminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
        }


        /// <summary>
        /// Gets the Administrator context.
        /// </summary>
        protected IAdminContext AdminContext { get; }

        /// <summary>
        /// Gets the extension code.
        /// </summary>
        public abstract string Code { get; }


        /// <summary>
        /// Loads language dictionaries.
        /// </summary>
        public virtual void LoadDictionaries()
        {
        }

        /// <summary>
        /// Loads configuration.
        /// </summary>
        public virtual void LoadConfig()
        {
        }

        /// <summary>
        /// Gets menu items to add to the main menu.
        /// </summary>
        public virtual ToolStripMenuItem[] GetMainMenuItems()
        {
            return null;
        }

        /// <summary>
        /// Gets tool buttons to add to the toolbar.
        /// </summary>
        public virtual ToolStripButton[] GetToobarButtons()
        {
            return null;
        }

        /// <summary>
        /// Gets tree nodes to add to the explorer tree.
        /// </summary>
        public virtual TreeNode[] GetTreeNodes(object relatedObject)
        {
            return null;
        }

        /// <summary>
        /// Gets the images used by the explorer tree.
        /// </summary>
        public virtual Dictionary<string, Image> GetTreeViewImages()
        {
            return null;
        }
    }
}
