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
        /// Обновление не требуется
        /// </summary>
        None = 0,

        /// <summary>
        /// Текущий узел
        /// </summary>
        CurrentNode = 1,

        /// <summary>
        /// Дочерние узлы
        /// </summary>
        ChildNodes = 2,

        /// <summary>
        /// Узлы того же уровня, следующие за текущим
        /// </summary>
        NextSiblings = 4,

        /// <summary>
        /// Обновить сигналы
        /// </summary>
        UpdateSignals = 8
    }
}
