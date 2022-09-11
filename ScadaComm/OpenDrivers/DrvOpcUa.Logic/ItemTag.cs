// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Devices;
using Scada.Comm.Drivers.DrvOpcUa.Config;

namespace Scada.Comm.Drivers.DrvOpcUa.Logic
{
    /// <summary>
    /// Represents metadata about a monitored item.
    /// <para>Представляет метаданные об отслеживаемом элементе.</para>
    /// </summary>
    internal class ItemTag
    {
        /// <summary>
        /// Gets the device tag corresponding to the item.
        /// </summary>
        public DeviceTag DeviceTag { get; init; }

        /// <summary>
        /// Gets the item configuration.
        /// </summary>
        public ItemConfig ItemConfig { get; init; }
    }
}
