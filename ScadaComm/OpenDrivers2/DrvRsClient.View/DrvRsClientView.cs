// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Forms;
using Scada.Lang;

namespace Scada.Comm.Drivers.DrvRsClient.View
{
    /// <summary>
    /// Implements the driver user interface.
    /// <para>Реализует пользовательский интерфейс драйвера.</para>
    /// </summary>
    public class DrvRsClientView : DriverView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DrvRsClientView()
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
                    "Клиент Rapid SCADA" :
                    "Rapid SCADA Client";
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
                    "Передаёт данные каналов из Сервера в Коммуникатор, а команды из Коммуникатора на Сервер." :
                    "Transmits channel data from Server to Communicator, and commands from Communicator to Server.";
            }
        }


        /// <summary>
        /// Loads language dictionaries.
        /// </summary>
        public override void LoadDictionaries()
        {
            if (!Locale.LoadDictionaries(AppDirs.LangDir, DriverUtils.DriverCode, out string errMsg))
                ScadaUiUtils.ShowError(errMsg);

            //DriverPhrases.Init();
            //AttrTranslator.Translate(typeof(DeviceOptions));
            //AttrTranslator.Translate(typeof(ItemConfig));
        }

        /// <summary>
        /// Creates a new device user interface.
        /// </summary>
        public override DeviceView CreateDeviceView(LineConfig lineConfig, DeviceConfig deviceConfig)
        {
            return new DevRsClientView(this, lineConfig, deviceConfig);
        }
    }
}
