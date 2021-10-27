// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Scada.Comm.Drivers.DrvModbus.View
{
    /// <summary>
    /// Specifies the types of updating the device template tree.
    /// <para>Задаёт типы обновления дерева шаблона устройства.</para>
    /// </summary>
    [Flags]
    public enum TreeUpdateTypes
    {
        /// <summary>
        /// No update.
        /// </summary>
        None = 0,

        /// <summary>
        /// Update only current node.
        /// </summary>
        CurrentNode = 1,

        /// <summary>
        /// Update child nodes, not including the current node.
        /// </summary>
        ChildNodes = 2,

        /// <summary>
        /// Update siblings after the current node.
        /// </summary>
        NextSiblings = 4,

        /// <summary>
        /// Number of child nodes changed.
        /// </summary>
        ChildCount = 8
    }
}
