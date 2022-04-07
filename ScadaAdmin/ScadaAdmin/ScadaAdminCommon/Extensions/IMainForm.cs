/*
 * Copyright 2022 Rapid Software LLC
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
 * Summary  : Defines functionality to access the explorer tree
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2022
 */

using Scada.Agent;
using System;
using System.Windows.Forms;

namespace Scada.Admin.Extensions
{
    /// <summary>
    /// Defines functionality to access the main form of the application.
    /// <para>Определяет функциональность для доступа к главной форме приложения.</para>
    /// </summary>
    public interface IMainForm
    {
        /// <summary>
        /// Gets the explorer tree.
        /// </summary>
        TreeView ExplorerTree { get; }

        /// <summary>
        /// Gets the selected node of the explorer tree.
        /// </summary>
        TreeNode SelectedNode { get; }

        /// <summary>
        /// Gets the item type of the configuration database table of the active child form.
        /// </summary>
        Type ActiveBaseTable { get; }

        /// <summary>
        /// Gets or sets the cursor.
        /// </summary>
        Cursor Cursor { get; set; }


        /// <summary>
        /// Closes the specified child form.
        /// </summary>
        void CloseChildForm(Form form, bool saveChanges);

        /// <summary>
        /// Closes child forms corresponding to the specified tree node and its subnodes.
        /// </summary>
        void CloseChildForms(TreeNode treeNode, bool saveChanges);

        /// <summary>
        /// Updates hints of child form tabs corresponding to the specified tree node and its subnodes.
        /// </summary>
        void UpdateChildFormHints(TreeNode treeNode);

        /// <summary>
        /// Refreshes child forms that contains a configuration database table with the specified item type.
        /// </summary>
        void RefreshBaseTables(Type itemType, bool saveChanges);

        /// <summary>
        /// Finds a tree node that represents a configuration database table.
        /// </summary>
        TreeNode FindBaseTableNode(Type itemType, object filterArgument);

        /// <summary>
        /// Finds an instance tree node by instance name.
        /// </summary>
        TreeNode FindInstanceNode(string instanceName, out bool justPrepared);

        /// <summary>
        /// Gets the Agent client corresponding to the specified tree node.
        /// </summary>
        /// <remarks>The existing client must be synchronized in case of multi-threaded access.</remarks>
        IAgentClient GetAgentClient(TreeNode treeNode, bool createNew);

        /// <summary>
        /// Gets the Agent client corresponding to the selected tree node.
        /// </summary>
        IAgentClient GetAgentClient(bool createNew)
        {
            return GetAgentClient(SelectedNode, createNew);
        }
    }
}
