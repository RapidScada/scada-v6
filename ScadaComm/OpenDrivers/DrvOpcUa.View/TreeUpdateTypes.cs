// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Comm.Drivers.DrvOpcUa.View
{
    /// <summary>
    /// Specifies the types of updating the device configuration tree.
    /// <para>Задаёт типы обновления дерева конфигурации КП.</para>
    /// </summary>
    [Flags]
    public enum TreeUpdateTypes
    {
        /// <summary>
        /// No update.
        /// </summary>
        None = 0,

        /// <summary>
        /// Update current node.
        /// </summary>
        CurrentNode = 1,

        /// <summary>
        /// Update tag numbers.
        /// </summary>
        UpdateTagNums = 2
    }
}
