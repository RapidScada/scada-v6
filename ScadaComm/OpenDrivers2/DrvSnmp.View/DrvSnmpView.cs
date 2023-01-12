// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Lang;

namespace Scada.Comm.Drivers.DrvSnmp.View
{
    /// <summary>
    /// Implements the driver user interface.
    /// <para>Реализует пользовательский интерфейс драйвера.</para>
    /// </summary>
    public class DrvSnmpView : DriverView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DrvSnmpView()
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
                    "Драйвер SNMP" :
                    "SNMP Driver";
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
                    "Взаимодействует с контроллерами по протоколу SNMP.\n\n" +
                    "Необходимо установить тип канала связи \"Не задан\".\n" +
                    "IP-адрес и порт (опционально) указываются в поле Строковый адрес.\n\n" +
                    "Команды ТУ:\n" +
                    "Команды позволяют установить значение переменных. " +
                    "Номер команды содержит номер тега устройства или код команды содержит наименование переменной.\n" +
                    "Числовое значение команды позволяет установить целое значение переменной.\n" +
                    "Данные команды имеют формат TYPE VALUE, где TYPE принимает значения:\n" +
                    "i - целое со знаком,\n" +
                    "u - мера (целое без знака),\n" +
                    "t - таймер (целое без знака),\n" +
                    "a - IP-адрес,\n" +
                    "o - идентификатор объекта (OID),\n" +
                    "s - строка,\n" +
                    "x - байты в 16-ричной форме через пробел,\n" +
                    "d - байты в десятичной форме через пробел,\n" +
                    "n - пустое значение (null)." :

                    "Interacts with controllers via SNMP protocol.\n\n" +
                    "Communication channel type must be set to \"Undefined\".\n" +
                    "IP address and port (optional) are defined in the String address field.\n\n" +
                    "Commands:\n" +
                    "Command number contains a device tag number, or command code contains a variable name.\n" +
                    "Numeric command value allows to set an integer variable value.\n" +
                    "Command data is in the format TYPE VALUE, where TYPE akes the following values:\n" +
                    "i - signed integer,\n" +
                    "u - gauge (unsigned integer),\n" +
                    "t - time ticks (unsigned integer),\n" +
                    "a - IP address,\n" +
                    "o - object identifier (OID),\n" +
                    "s - string,\n" +
                    "x - bytes in the hexadecimal form separated by spaces,\n" +
                    "d - bytes in the decimal form separated by spaces,\n" +
                    "n - null value.";
            }
        }


        /// <summary>
        /// Loads language dictionaries.
        /// </summary>
        public override void LoadDictionaries()
        {
            /*if (!Locale.LoadDictionaries(AppDirs.LangDir, DriverUtils.DriverCode, out string errMsg))
                ScadaUiUtils.ShowError(errMsg);

            DriverPhrases.Init();
            AttrTranslator.Translate(typeof(DeviceOptions));
            AttrTranslator.Translate(typeof(ItemConfig));*/
        }

        /// <summary>
        /// Creates a new device user interface.
        /// </summary>
        public override DeviceView CreateDeviceView(LineConfig lineConfig, DeviceConfig deviceConfig)
        {
            return new DevSnmpView(this, lineConfig, deviceConfig);
        }
    }
}
