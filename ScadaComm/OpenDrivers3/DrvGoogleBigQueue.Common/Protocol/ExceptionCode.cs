// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

namespace Scada.Comm.Drivers.DrvGoogleBigQueue.Protocol
{
    /// <summary>
    /// Specifies the Modbus exception codes.
    /// <para>Задаёт коды исключений Modbus.</para>
    /// </summary>
    public static class ExceptionCode
    {
        public const byte IllegalFunction = 0x01;
        public const byte IllegalDataAddress = 0x02;
        public const byte IllegalDataValue = 0x03;
        public const byte SlaveDeviceFailure = 0x04;
        public const byte Acknowledge = 0x05;
        public const byte SlaveDeviceBusy = 0x06;
        public const byte MemoryParityError = 0x08;
        public const byte GatewayPathUnavailable = 0x0A;
        public const byte GatewayTargetFailed = 0x0B;
    }
}
