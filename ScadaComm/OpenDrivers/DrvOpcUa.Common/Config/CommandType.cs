// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Comm.Drivers.DrvOpcUa.Config
{
    /// <summary>
    /// Specifies the command types.
    /// <para>Задает типы команд.</para>
    /// </summary>
    public enum CommandType
    {
        WriteItem,
        CallMethod,
        ReadHistory
    }
}
