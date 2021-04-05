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
 * Module   : ScadaCommon.Forms
 * Summary  : Specifies the behaviors when moving a node
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

namespace Scada.Forms
{
    /// <summary>
    /// Specifies the behaviors when moving a node.
    /// <para>Задает поведение при перемещении узла.</para>
    /// </summary>
    public enum TreeNodeBehavior
    {
        /// <summary>
        /// A node can only move within its parent node.
        /// </summary>
        WithinParent,

        /// <summary>
        /// A node can move within its parent node and from one parent to another of the same type.
        /// </summary>
        ThroughSimilarParents
    }
}
