// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvModbus.Protocol;

namespace Scada.Comm.Drivers.DrvEnronModbus.Logic.Protocol
{
    /// <summary>
    /// Represents an Enron Modbus command.
    /// <para>Представляет команду Enron Modbus.</para>
    /// </summary>
    internal class EnronCmd : ModbusCmd
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EnronCmd(DataBlock dataBlock, bool multiple)
            : base(dataBlock, multiple)
        {
        }
    }
}
