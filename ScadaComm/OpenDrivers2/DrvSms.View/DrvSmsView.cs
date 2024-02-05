// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.AB.Forms;
using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Lang;

namespace Scada.Comm.Drivers.DrvSms.View
{
    /// <summary>
    /// Implements the driver user interface.
    /// <para>Реализует пользовательский интерфейс драйвера.</para>
    /// </summary>
    public class DrvSmsView : DriverView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DrvSmsView()
            : base()
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
                return "SMS";
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
                    "Отправляет и получает SMS-сообщения с помощью AT-команд.\n\n" +
                    "Команды ТУ:\n" +
                    "1, Msg - отправить SMS-сообщение.\n" +
                    "Примеры текста команды:\n" +
                    "имя_группы;сообщение\n" +
                    "имя_контакта;сообщение\n" +
                    "телефон;сообщение\n\n" +
                    "2, AtCmd - отправить произвольную AT-команду.\n" +
                    "Текст команды содержит отправляемую команду как есть." :

                    "Sends and receives SMS messages using AT commands.\n\n" +
                    "Commands:\n" +
                    "1, Msg - send SMS message.\n" +
                    "Command text examples:\n" +
                    "group_name;message\n" +
                    "contact_name;message\n" +
                    "phone_number;message\n\n" +
                    "2, AtCmd - send custom AT command.\n" +
                    "Command text contains the command to be sent.";
            }
        }


        /// <summary>
        /// Shows a modal dialog box for editing driver properties.
        /// </summary>
        public override bool ShowProperties()
        {
            new FrmAddressBook(AppDirs).ShowDialog();
            return false;
        }

        /// <summary>
        /// Creates a new device user interface.
        /// </summary>
        public override DeviceView CreateDeviceView(LineConfig lineConfig, DeviceConfig deviceConfig)
        {
            return new DevSmsView(this, lineConfig, deviceConfig);
        }
    }
}
