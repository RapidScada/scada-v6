// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Comm.Drivers.DrvModbus.Protocol
{
    /// <summary>
    /// Specifies the data transfer modes.
    /// <para>Задаёт режимы передачи данных.</para>
    /// </summary>
    public enum TransMode
    {
        /// <summary>
        /// Binary data format with CRC.
        /// </summary>
        RTU,

        /// <summary>
        /// ASCII encoded data.
        /// </summary>
        ASCII,

        /// <summary>
        /// Binary data format without CRC for TCP data transfer.
        /// </summary>
        TCP
    }
}
