// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Comm.Drivers.DrvModbus.Protocol
{
    /// <summary>
    /// Represents a Modbus device model.
    /// <para>Представляет модель устройства Modbus.</para>
    /// </summary>
    public class DeviceModel
    {
        /// <summary>
        /// Найти команду по номеру
        /// </summary>
        public ModbusCmd FindCmd(int cmdNum)
        {
            /*foreach (ModbusCmd cmd in Cmds)
            {
                if (cmd.CmdNum == cmdNum)
                    return cmd;
            }*/

            return null;
        }

        /// <summary>
        /// Creates a new group of Modbus elements.
        /// </summary>
        public virtual ElemGroup CreateElemGroup(DataBlock dataBlock)
        {
            return new ElemGroup(dataBlock);
        }

        /// <summary>
        /// Creates a new Modbus command.
        /// </summary>
        public virtual ModbusCmd CreateModbusCmd(DataBlock dataBlock, bool multiple)
        {
            return new ModbusCmd(dataBlock, multiple);
        }
    }
}
