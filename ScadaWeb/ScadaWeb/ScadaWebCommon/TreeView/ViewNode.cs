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
 * Module   : ScadaWebCommon
 * Summary  : Represents a node of the view explorer
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2022
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Scada.Web.TreeView
{
    /// <summary>
    /// Represents a node of the view explorer.
    /// </summary>
    public class ViewNode : IWebTreeNode, IComparable<ViewNode>
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ViewNode()
            : this(0)
        {
        }

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
            Level = -1;
            DataAttrs = new SortedList<string, string>() { { "viewID", viewID.ToString() } };

            ViewID = viewID;
            ViewFrameUrl = "";
            ShortPath = "";
            SortOrder = 0;
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
        /// Gets or sets the icon URL.
        /// </summary>
        public string IconUrl { get; set; }

        /// <summary>
        /// Gets or sets the node text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the URL to open when the node is clicked.
        /// </summary>
        public string Url { get; set; }

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
        public string ViewFrameUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the node has no associated view.
        /// </summary>
        public bool IsEmpty => string.IsNullOrEmpty(ViewFrameUrl);

        /// <summary>
        /// Gets the short path of the current node.
        /// </summary>
        public string ShortPath { get; set; }

        /// <summary>
        /// Gets or sets the sort order.
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// Gets the child view nodes.
        /// </summary>
        public List<ViewNode> ChildNodes { get; }


        /// <summary>
        /// Compares the current instance with another object of the same type.
        /// </summary>
        public int CompareTo(ViewNode other)
        {
            if (other == null)
                return 1;

            int compareResult = SortOrder.CompareTo(other.SortOrder);
            if (compareResult != 0)
                return compareResult;

            compareResult = ViewID.CompareTo(other.ViewID);
            if (compareResult != 0)
                return compareResult;

            compareResult = string.Compare(Text, other.Text, StringComparison.OrdinalIgnoreCase);
            if (compareResult != 0)
                return compareResult;

            return string.Compare(Url, other.Url, StringComparison.OrdinalIgnoreCase);
        }
    }
}
