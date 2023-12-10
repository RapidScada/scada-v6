// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Comm.Drivers.DrvCsvReader
{
    /// <summary>
    /// Specifies the reading modes.
    /// <para>Задаёт режимы чтения.</para>
    /// </summary>
    internal enum ReadMode
    {
        /// <summary>
        /// The driver reads data according to the current time
        /// </summary>
        RealTime,

        /// <summary>
        /// The driver reads data cyclically.
        /// </summary>
        Demo
    }
}
