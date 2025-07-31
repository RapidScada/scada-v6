// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Models;

namespace Scada.Comm.Drivers.DrvMqttPublisher
{
    /// <summary>
    /// The class provides helper methods for the driver.
    /// <para>Класс, предоставляющий вспомогательные методы для драйвера.</para>
    /// </summary>
    internal static class DriverUtils
    {
        /// <summary>
        /// The driver code.
        /// </summary>
        public const string DriverCode = "DrvMqttPublisher";

        /// <summary>
        /// Gets or sets the configuration database required for editing the driver configuration.
        /// </summary>
        public static ConfigDataset ConfigDataset { get; set; } = null;
    }
}
