// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
