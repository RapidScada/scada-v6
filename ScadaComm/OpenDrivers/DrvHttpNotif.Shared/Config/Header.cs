// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Comm.Drivers.DrvHttpNotif.Config
{
    /// <summary>
    /// Represents a request header.
    /// <para>Представляет заголовок запроса.</para>
    /// </summary>
    internal class Header
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
