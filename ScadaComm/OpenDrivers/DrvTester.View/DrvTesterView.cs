// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Lang;

namespace Scada.Comm.Drivers.DrvTester.View
{
    /// <summary>
    /// Implements the driver user interface.
    /// <para>Реализует пользовательский интерфейс драйвера.</para>
    /// </summary>
    public class DrvTesterView : DriverView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DrvTesterView()
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
                return Locale.IsRussian ? "Тестер каналов связи" : "Communication Channel Tester";
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
                    "Предназначен для проверки каналов связи.\n\n" +
                    "Команды ТУ:\n" +
                    "1, SendStr - отправить строку;\n" +
                    "2, SendBin - отправить бинарные данные." :

                    "Designed for testing communication channels.\n\n" +
                    "Commands:\n" +
                    "1, SendStr - send string;\n" +
                    "2, SendBin - send binary data.";
            }
        }


        /// <summary>
        /// Creates a new device user interface.
        /// </summary>
        public override DeviceView CreateDeviceView(LineConfig lineConfig, DeviceConfig deviceConfig)
        {
            return new DevTesterView(this, lineConfig, deviceConfig);
        }
    }
}
