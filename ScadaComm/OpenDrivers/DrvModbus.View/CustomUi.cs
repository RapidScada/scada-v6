// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvModbus.Config;

namespace Scada.Comm.Drivers.DrvModbus.View
{
    /// <summary>
    /// Provides flexibility to the driver user interface.
    /// <para>Обеспечивает гибкость пользовательского интерфейса драйвера.</para>
    /// </summary>
    public class CustomUi
    {
        /// <summary>
        /// Creates a new device template.
        /// </summary>
        public virtual DeviceTemplate CreateDeviceTemplate()
        {
            return new DeviceTemplate();
        }

        /// <summary>
        /// Gets a value indicating whether to display the extended options button.
        /// </summary>
        public virtual bool ExtendedOptionsAvailable
        {
            get
            {
                return false;
            }
        }


        /// <summary>
        /// Shows the extended template options as a modal dialog box.
        /// </summary>
        /// <returns>Returns true if the options changed.</returns>
        public virtual bool ShowExtendedOptions(DeviceTemplate deviceTemplate)
        {
            return false;
        }
    }
}
