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
 * Module   : ScadaWebCommon
 * Summary  : Defines functionality to operate with a tree structure on a web page
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2023
 */

using System.Collections;

namespace Scada.Web.TreeView
{
    /// <summary>
    /// Defines functionality to operate with a tree structure on a web page.
    /// <para>Определяет функциональность для работы с древовидной структурой на веб-странице.</para>
    /// </summary>
    public interface IWebTreeNode
    {
        /// <summary>
        /// Gets or sets the parent tree node.
        /// </summary>
        IWebTreeNode Parent { get; set; }

        /// <summary>
        /// Gets the child tree nodes.
        /// </summary>
        IList Children { get; }

        /// <summary>
        /// Gets a value indicating whether the node is hidden.
        /// </summary>
        bool IsHidden { get; }

        /// <summary>
        /// Gets the icon URL.
        /// </summary>
        string IconUrl { get; }

        /// <summary>
        /// Gets the node text.
        /// </summary>
        string Text { get; }

        /// <summary>
        /// Gets the URL to open when the node is clicked.
        /// </summary>
        string Url { get; }

        /// <summary>
        /// Gets or sets the nesting level.
        /// </summary>
        int Level { get; set; }

        /// <summary>
        /// Gets the data attributes as key/value pairs.
        /// </summary>
        IDictionary<string, string> DataAttrs { get; }


        /// <summary>
        /// Determines that the node represents the specified object.
        /// </summary>
        bool Represents(object obj);
    }
}
