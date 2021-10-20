// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Comm.Drivers.DrvCnlBasic.Logic
{
    /// <summary>
    /// Specifies the codes of the channel types supported by the driver.
    /// <para>Задает коды типов каналов, поддерживаемые драйвером.</para>
    /// </summary>
    internal static class ChannelTypeCode
    {
        public const string Serial = "Serial";
        public const string TcpClient = "TcpClient";
        public const string TcpServer = "TcpServer";
        public const string Udp = "Udp";
    }
}
