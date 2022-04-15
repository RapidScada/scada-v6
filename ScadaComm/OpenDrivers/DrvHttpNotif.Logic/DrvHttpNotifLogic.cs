// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.Devices;

namespace Scada.Comm.Drivers.DrvHttpNotif.Logic
{
    /// <summary>
    /// Implements the driver logic.
    /// <para>Реализует логику драйвера.</para>
    /// </summary>
    public class DrvHttpNotifLogic : DriverLogic
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DrvHttpNotifLogic(ICommContext commContext)
            : base(commContext)
        {
        }

        /// <summary>
        /// Gets the driver code.
        /// </summary>
        public override string Code
        {
            get
            {
                return DriverUtils.DriverCode;
            }
        }

        /// <summary>
        /// Creates a new device.
        /// </summary>
        public override DeviceLogic CreateDevice(ILineContext lineContext, DeviceConfig deviceConfig)
        {
            return new DevHttpNotifLogic(CommContext, lineContext, deviceConfig);
        }
    }
}
