// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvModbus.Protocol;

namespace Scada.Comm.Drivers.DrvEnronModbus.Logic.Protocol
{
    /// <summary>
    /// Represents an Enron Modbus device model.
    /// <para>Представляет модель устройства Enron Modbus.</para>
    /// </summary>
    internal class EnronDeviceModel : DeviceModel
    {
        /// <summary>
        /// Creates a new group of Modbus elements.
        /// </summary>
        public override ElemGroup CreateElemGroup(DataBlock dataBlock)
        {
            return new EnronElemGroup(dataBlock);
        }

        /// <summary>
        /// Creates a new Modbus command.
        /// </summary>
        public override ModbusCmd CreateModbusCmd(DataBlock dataBlock, bool multiple)
        {
            return new EnronCmd(dataBlock, multiple);
        }
    }
}
