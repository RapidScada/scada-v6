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
 * Module   : ScadaCommon
 * Summary  : Defines functionality to operate with a tree structure
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2021
 */

using System.Collections;

namespace Scada
{
    /// <summary>
    /// Defines functionality to operate with a tree structure.
    /// <para>Определяет функциональность для работы с древовидной структурой.</para>
    /// </summary>
    public interface ITreeNode
    {
        /// <summary>
        /// Gets or sets the parent tree node.
        /// </summary>
        /// <remarks>If a class is serializable, do not serialize the parent.</remarks>
        ITreeNode Parent { get; set; }

        /// <summary>
        /// Gets the child tree nodes.
        /// </summary>
        IList Children { get; }
    }
}
