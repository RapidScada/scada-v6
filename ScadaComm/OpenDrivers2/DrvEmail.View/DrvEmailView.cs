// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Forms;
using Scada.Lang;

namespace Scada.Comm.Drivers.DrvEmail.View
{
    /// <summary>
    /// Implements the driver user interface.
    /// <para>Реализует пользовательский интерфейс драйвера.</para>
    /// </summary>
    internal class DrvEmailView : DriverView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DrvEmailView()
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
                return "Email";
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
                    "Отправляет уведомления по электронной почте.\n\n" +
                    "Команды ТУ:\n" +
                    "1, Mail - отправить электронное письмо;\n" +
                    "2, MailAttach - отправить электронное письмо с вложениями.\n\n" +
                    "Примеры текста команды:\n" +
                    "имя_группы;тема;сообщение;вложения\n" +
                    "имя_контакта;тема;сообщение;вложения\n" +
                    "эл_почта;тема;сообщение;вложения\n" +
                    "вложения - это список путей к файлам, разделенных запятыми.":

                    "Sends email notifications.\n\n" +
                    "Commands:\n" +
                    "1, Mail - send the email;\n" +
                    "2, MailAttach - send the email with attachments.\n\n" +
                    "Command text examples:\n" +
                    "group_name;subject;message;attachments\n" +
                    "contact_name;subject;message;attachments\n" +
                    "email;subject;message;attachments\n" +
                    "attachments is a comma-separated list of file paths.";
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
            return new DevEmailView(this, lineConfig, deviceConfig);
        }
    }
}
