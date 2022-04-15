// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.Devices;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Comm.Drivers.DrvHttpNotif.Logic
{
    /// <summary>
    /// Implements the device logic.
    /// <para>Реализует логику устройства.</para>
    /// </summary>
    internal class DevHttpNotifLogic : DeviceLogic
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DevHttpNotifLogic(ICommContext commContext, ILineContext lineContext, DeviceConfig deviceConfig)
            : base(commContext, lineContext, deviceConfig)
        {
            CanSendCommands = true;
            ConnectionRequired = false;
        }
    }
}
