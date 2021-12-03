// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Comm.Drivers.DrvOpcUa.Config
{
    /// <summary>
    /// Specifies the rules to set tag codes by default.
    /// <para>Задаёт правила для установки кодов тегов по умолчанию.</para>
    /// </summary>
    public enum DefaultTagCode
    {
        NodeID,
        BrowseName,
        DisplayName
    }
}
