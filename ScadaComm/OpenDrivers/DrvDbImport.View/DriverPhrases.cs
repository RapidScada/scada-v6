// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.Comm.Drivers.DrvDbImport.View
{
    /// <summary>
    /// The phrases used by the driver.
    /// <para>Фразы, используемые драйвером.</para>
    /// </summary>
    internal class DriverPhrases
    {
        // Scada.Comm.Drivers.DrvDbImport.View.Forms.FrmDeviceConfig
        public static string ConnectionNode { get; private set; }
        public static string CommandsNode { get; private set; }
        public static string QueriesNode { get; private set; }

        public static void Init()
        {
            LocaleDict dict = Locale.GetDictionary("Scada.Comm.Drivers.DrvDbImport.View.Forms.FrmDeviceConfig");
            ConnectionNode = dict[nameof(ConnectionNode)];
            CommandsNode = dict[nameof(CommandsNode)];
            QueriesNode = dict[nameof(QueriesNode)];
        }
    }
}
