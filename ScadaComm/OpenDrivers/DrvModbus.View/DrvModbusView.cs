// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Drivers.DrvModbus.View.Forms;
using Scada.Forms;
using Scada.Lang;

namespace Scada.Comm.Drivers.DrvModbus.View
{
    /// <summary>
    /// Implements the driver user interface.
    /// <para>Реализует пользовательский интерфейс драйвера.</para>
    /// </summary>
    public class DrvModbusView : DriverView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DrvModbusView()
        {
            CanShowProperties = true;
            CanCreateDevice = true;
        }


        /// <summary>
        /// Gets the driver name.
        /// </summary>
        public override string Name
        {
            get
            {
                return Locale.IsRussian ? "Драйвер Modbus" : "Modbus Driver";
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
                    "Обеспечивает взаимодействие с контроллерами по протоколу Modbus.\n\n" +
                    "Пользовательский параметр линии связи:\n" +
                    "TransMode - режим передачи данных (RTU, ASCII, TCP).\n\n" +
                    "Параметр командной строки:\n" +
                    "имя файла шаблона устройства.\n\n" +
                    "Команды ТУ:\n" +
                    "определяются шаблоном устройства." :

                    "Provides interaction with controllers via Modbus protocol.\n\n" +
                    "Custom communication line parameter:\n" +
                    "TransMode - data transmission mode (RTU, ASCII, TCP).\n\n" +
                    "Command line parameter:\n" +
                    "device template file name.\n\n" +
                    "Commands:\n" +
                    "defined by device template.";
            }
        }


        /// <summary>
        /// Gets a UI customization object.
        /// </summary>
        protected virtual UiCustomization GetUiCustomization()
        {
            return new UiCustomization();
        }

        /// <summary>
        /// Loads language dictionaries.
        /// </summary>
        public override void LoadDictionaries()
        {
            if (!Locale.LoadDictionaries(AppDirs.LangDir, DriverUtils.DriverCode, out string errMsg))
                ScadaUiUtils.ShowError(errMsg);

            DriverPhrases.Init();
        }

        /// <summary>
        /// Shows a modal dialog box for editing driver properties.
        /// </summary>
        public override bool ShowProperties()
        {
            FrmDevTemplate.ShowDialog(AppDirs, GetUiCustomization());
            return false;
        }

        /// <summary>
        /// Creates a new device user interface.
        /// </summary>
        public override DeviceView CreateDeviceView(LineConfig lineConfig, DeviceConfig deviceConfig)
        {
            return new DevModbusView(this, lineConfig, deviceConfig, GetUiCustomization());
        }
    }
}
