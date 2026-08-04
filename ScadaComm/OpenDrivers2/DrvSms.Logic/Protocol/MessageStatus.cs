// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Comm.Drivers.DrvSms.Logic.Protocol
{
    /// <summary>
    /// Specifies the message statuses provided by the device. See AT+CMGL command.
    /// </summary>
    public class MessageStatus
    {
        public const int Unread = 0;
        public const int Read = 1;
        public const int Unsent = 2;
        public const int Sent = 3;
        public const int All = 4;
    }
}
