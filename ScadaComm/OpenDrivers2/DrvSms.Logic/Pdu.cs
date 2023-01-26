// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Comm.Drivers.DrvSms.Logic
{
    /// <summary>
    /// Represents a protocol data unit (PDU).
    /// <para>Представляет protocol data unit (PDU).</para>
    /// </summary>
    internal class Pdu
    {
        /// <summary>
        /// Gets or sets the PDU data as a hexadecimal string.
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Gets or sets the PDU length as required by AT+CMGS command.
        /// </summary>
        public int Length { get; set; }
    }
}
