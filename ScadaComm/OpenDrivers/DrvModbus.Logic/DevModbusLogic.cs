// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.Devices;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Comm.Drivers.DrvModbus.Logic
{
    /// <summary>
    /// Implements the device logic.
    /// <para>Реализует логику КП.</para>
    /// </summary>
    public class DevModbusLogic : DeviceLogic
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DevModbusLogic(ICommContext commContext, ILineContext lineContext, DeviceConfig deviceConfig)
            : base(commContext, lineContext, deviceConfig)
        {
        }
    }
}
