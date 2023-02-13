// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvEnronModbus.Config;
using Scada.Comm.Drivers.DrvModbus.Config;
using Scada.Comm.Drivers.DrvModbus.View;

namespace Scada.Comm.Drivers.DrvEnronModbus.View
{
    /// <summary>
    /// Implements Enron Modbus user interface features.
    /// <para>Реализует особенности пользовательского интерфейса Enron Modbus.</para>
    /// </summary>
    internal class EnronUi : CustomUi
    {
        /// <summary>
        /// Creates a new device template.
        /// </summary>
        public override DeviceTemplate CreateDeviceTemplate() => new EnronDeviceTemplate();
    }
}
