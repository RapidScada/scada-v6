// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.Drivers.DrvEnronModbus.Config;
using Scada.Comm.Drivers.DrvEnronModbus.Logic.Protocol;
using Scada.Comm.Drivers.DrvModbus.Config;
using Scada.Comm.Drivers.DrvModbus.Logic;
using Scada.Comm.Drivers.DrvModbus.Protocol;

namespace Scada.Comm.Drivers.DrvEnronModbus.Logic
{
    /// <summary>
    /// Implements the device logic.
    /// <para>Реализует логику устройства.</para>
    /// </summary>
    internal class DevEnronModbusLogic : DevModbusLogic
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DevEnronModbusLogic(ICommContext commContext, ILineContext lineContext, DeviceConfig deviceConfig)
            : base(commContext, lineContext, deviceConfig)
        {
        }


        /// <summary>
        /// Gets the shared data key of the template dictionary.
        /// </summary>
        protected override string TemplateDictKey => "EnronModbus.Templates";


        /// <summary>
        /// Create a new device template.
        /// </summary>
        protected override DeviceTemplate CreateDeviceTemplate()
        {
            return new EnronDeviceTemplate();
        }

        /// <summary>
        /// Create a new device model.
        /// </summary>
        protected override DeviceModel CreateDeviceModel()
        {
            return new EnronDeviceModel();
        }
    }
}
