// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Lang;

namespace Scada.Comm.Drivers.DrvSimulator.View
{
    /// <summary>
    /// Implements the driver user interface.
    /// <para>Реализует пользовательский интерфейс драйвера.</para>
    /// </summary>
    public class DrvSimulatorView : DriverView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DrvSimulatorView()
        {
            CanCreateDevice = true;
        }


        /// <summary>
        /// Gets the driver name.
        /// </summary>
        public override string Name
        {
            get
            {
                return Locale.IsRussian ? "Симулятор устройства" : "Device Simulator";
            }
        }

        /// <summary>
        /// Gets the driver description.
        /// </summary>
        public override string Descr
        {
            get
            {
                return Locale.IsRussian ?
                    "Симулирует значения тегов.\n\n" +
                    "Команды ТУ:\n" +
                    "4, DO - установить состояние реле;\n" +
                    "5, AO - установить аналоговый выход;\n" +
                    "Hist - создать исторические данные;\n" +
                    "Event - создать событие." :

                    "Simulates tag values.\n\n" +
                    "Commands:\n" +
                    "4, DO - set relay state;\n" +
                    "5, AO - set analog output;\n" +
                    "Hist - create historical data;\n" +
                    "Event - create event.";
            }
        }


        /// <summary>
        /// Creates a new device user interface.
        /// </summary>
        public override DeviceView CreateDeviceView(LineConfig lineConfig, DeviceConfig deviceConfig)
        {
            return new DevSimulatorView(this, lineConfig, deviceConfig);
        }
    }
}
