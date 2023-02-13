// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvModbus.View;
using Scada.Lang;

namespace Scada.Comm.Drivers.DrvEnronModbus.View
{
    /// <summary>
    /// Implements the driver user interface.
    /// <para>Реализует пользовательский интерфейс драйвера.</para>
    /// </summary>
    public class DrvEnronModbusView : DrvModbusView
    {
        /// <summary>
        /// Gets the driver name.
        /// </summary>
        public override string Name
        {
            get
            {
                return Locale.IsRussian ?
                    "Драйвер Enron Modbus" :
                    "Enron Modbus Driver";
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
                    "Взаимодействует с контроллерами по протоколу Enron Modbus.\n" +
                    "Поддерживаемые параметры линии связи и команды ТУ аналогичны DrvModbus.dll.\n" +
                    "Описание протокола доступно на https://www.simplymodbus.ca/" :

                    "Interacts with controllers via Enron Modbus protocol.\n" +
                    "Supported communication line parameters and commands are similar to DrvModbus.dll.\n" +
                    "Protocol description is available at https://www.simplymodbus.ca/";
            }
        }


        /// <summary>
        /// Gets a UI customization object.
        /// </summary>
        protected override CustomUi GetCustomUi()
        {
            return new EnronUi();
        }
    }
}
