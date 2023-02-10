// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

namespace Scada.Comm.Drivers.DrvModbus.Protocol
{
    /// <summary>
    /// Specifies the codes of the supported Modbus functions.
    /// <para>Задаёт коды поддерживаемых функций Modbus.</para>
    /// </summary>
    public static class FuncCodeConst
    {
        public const byte ReadCoils = 0x01;
        public const byte ReadDiscreteInputs = 0x02;
        public const byte ReadInputRegisters = 0x04;
        public const byte ReadHoldingRegisters = 0x03;

        public const byte WriteSingleCoil = 0x05;
        public const byte WriteSingleRegister = 0x06;
        public const byte WriteMultipleCoils = 0x0F;
        public const byte WriteMultipleRegisters = 0x10;
    }
}
