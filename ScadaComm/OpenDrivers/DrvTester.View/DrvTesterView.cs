// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
                    "1, SendBin - отправить бинарные данные;\n" +
                    "2, SendStr - отправить строку." :

                    "Designed for testing communication channels.\n\n" +
                    "Commands:\n" +
                    "1, SendBin - send binary data;\n" +
                    "2, SendStr - send string.";
            }
        }
    }
}
