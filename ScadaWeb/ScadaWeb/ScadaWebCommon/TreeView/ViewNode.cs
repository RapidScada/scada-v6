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
 * Module   : ScadaWebCommon
 * Summary  : Represents a node of the view explorer
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2021
 */

using System.Collections;
using System.Collections.Generic;

namespace Scada.Web.TreeView
{
    /// <summary>
    /// Represents a node of the view explorer.
    /// </summary>
    public class ViewNode : IWebTreeNode
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ViewNode(int viewID)
        {
            Parent = null;
            IsHidden = false;
            IconUrl = "";
            Text = "";
            Url = "";
            Script = "";
            Level = -1;
            DataAttrs = new SortedList<string, string>() { { "view", viewID.ToString() } };

            ViewID = viewID;
            ViewUrl = "";
            ChildNodes = new List<ViewNode>();
        }


        #region IWebTreeNode
        /// <summary>
        /// Gets or sets the parent tree node.
        /// </summary>
        public IWebTreeNode Parent { get; set; }

        /// <summary>
        /// Gets the child tree nodes.
        /// </summary>
        public IList Children => ChildNodes;

        /// <summary>
        /// Gets or sets a value indicating whether the node is hidden.
        /// </summary>
        public bool IsHidden { get; set; }

        /// <summary>
        /// Gets the icon URL.
        /// </summary>
        public string IconUrl { get; set; }

        /// <summary>
        /// Gets or sets the node text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets the URL to open when the node is clicked.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets the script to execute when the node is clicked.
        /// </summary>
        /// <remarks>
        /// The script has a higher priority than the URL.
        /// It allows to open a page in a new tab using the browser context menu.
        /// </remarks>
        public string Script { get; set; }

        /// <summary>
        /// Gets or sets the nesting level.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Gets the data attributes as key/value pairs.
        /// </summary>
        public IDictionary<string, string> DataAttrs { get; }


        /// <summary>
        /// Determines that the node represents the specified object.
        /// </summary>
        public bool Represents(object obj)
        {
            return obj is int viewID && viewID > 0 && viewID == ViewID;
        }
        #endregion


        /// <summary>
        /// Gets or sets the view ID.
        /// </summary>
        public int ViewID { get; set; }

        /// <summary>
        /// Gets or sets the view frame URL.
        /// </summary>
        public string ViewUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the node has no associated view.
        /// </summary>
        public bool IsEmpty => ViewID <= 0 || string.IsNullOrEmpty(ViewUrl);

        /// <summary>
        /// Gets the child view nodes.
        /// </summary>
        public List<ViewNode> ChildNodes { get; }
    }
}
