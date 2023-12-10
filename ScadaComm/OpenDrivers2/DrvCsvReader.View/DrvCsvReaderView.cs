// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Lang;

namespace Scada.Comm.Drivers.DrvCsvReader.View
{
    /// <summary>
    /// Implements the driver user interface.
    /// <para>Реализует пользовательский интерфейс драйвера.</para>
    /// </summary>
    public class DrvCsvReaderView : DriverView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DrvCsvReaderView()
            : base()
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
                return Locale.IsRussian ?
                    "CSV-считыватель" :
                    "CSV Reader";
            }
        }

        /// <summary>
        /// Gets the driver description.
        /// </summary>
        public override string Descr
        {
            get
            {
                return (Locale.IsRussian ?
                    "Считывает данные из CSV-файла.\n\n" +
                    "Пример файла данных:\n":
                    "Reads data from a CSV file.\n\n" +
                    "Data file example:\n") +
                    "Timestamp,TagA,TagB,TagC\n" +
                    "2001-01-01 00:00:00,1.23,1,0";
            }
        }


        /// <summary>
        /// Creates a new device user interface.
        /// </summary>
        public override DeviceView CreateDeviceView(LineConfig lineConfig, DeviceConfig deviceConfig)
        {
            return new DevCsvReaderView(this, lineConfig, deviceConfig);
        }
    }
}
