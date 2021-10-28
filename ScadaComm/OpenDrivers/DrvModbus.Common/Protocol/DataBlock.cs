// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Comm.Drivers.DrvModbus.Protocol
{
    /// <summary>
    /// Specifies the Modbus data blocks.
    /// <para>Задаёт блоки данных Modbus.</para>
    /// </summary>
    public enum DataBlock
    {
        /// <summary>
        /// Discrete inputs, 1 bit, read only, 1X addressing.
        /// </summary>
        DiscreteInputs,

        /// <summary>
        /// Coils, 1 bit, read and write, 0X addressing.
        /// </summary>
        Coils,

        /// <summary>
        /// Input registers, 16 bits, read only, 3X addressing.
        /// </summary>
        InputRegisters,

        /// <summary>
        /// Holding registers, 16 bits, read and write, 4X addressing.
        /// </summary>
        HoldingRegisters,

        /// <summary>
        /// Custom data block.
        /// </summary>
        Custom
    }
}
