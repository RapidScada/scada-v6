// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Forms;
using Scada.Lang;

namespace Scada.Comm.Drivers.DrvHttpNotif.View
{
    /// <summary>
    /// Implements the driver user interface.
    /// <para>Реализует пользовательский интерфейс драйвера.</para>
    /// </summary>
    public class DrvHttpNotifView : DriverView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DrvHttpNotifView()
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
                return Locale.IsRussian ? "HTTP уведомления" : "HTTP Notifications";
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
                    "Отправляет уведомления с помощью HTTP-запросов.\n\n" +
                    "Команды ТУ:\n" +
                    "1, Notif - отправить уведомление.\n" +
                    "Примеры текста команды:\n" +
                    "имя_группы;сообщение\n" +
                    "имя_контакта;сообщение\n" +
                    "эл_почта;сообщение\n\n" +
                    "2, Request - отправить произвольный запрос.\n" +
                    "Текст команды содержит аргументы:\n" +
                    "arg1=value1\\n\n" +
                    "arg2=value2" :

                    "Sends notifications via HTTP requests.\n\n" +
                    "Commands:\n" +
                    "1, Notif - send notification.\n" +
                    "Command text examples:\n" +
                    "group_name;message\n" +
                    "contact_name;message\n" +
                    "email;message\n\n" +
                    "2, Request - send custom request.\n" +
                    "Command text contains arguments:\n" +
                    "arg1=value1\\n\n" +
                    "arg2=value2";
            }
        }


        /// <summary>
        /// Loads language dictionaries.
        /// </summary>
        public override void LoadDictionaries()
        {
            if (!Locale.LoadDictionaries(AppDirs.LangDir, DriverUtils.DriverCode, out string errMsg))
                ScadaUiUtils.ShowError(errMsg);
        }

        /// <summary>
        /// Creates a new device user interface.
        /// </summary>
        public override DeviceView CreateDeviceView(LineConfig lineConfig, DeviceConfig deviceConfig)
        {
            return new DevHttpNotifView(this, lineConfig, deviceConfig);
        }
    }
}
