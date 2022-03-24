// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Comm.Drivers.DrvMqtt
{
    /// <summary>
    /// Specifies the variables to use in messages.
    /// <para>Задаёт переменные, используемые в сообщениях.</para>
    /// </summary>
    public static class MessageVar
    {
        /// <summary>
        /// Channel value.
        /// </summary>
        public const string Value = "@val";

        /// <summary>
        /// Channel status.
        /// </summary>
        public const string Status = "@stat";
    }
}
