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
        public static string AddRoots { get; private set; }
        public static string ConnectionNode { get; private set; }
        public static string CommandName { get; private set; }
        public static string CommandsNode { get; private set; }
        public static string QueryName { get; private set; }
        public static string QueriesNode { get; private set; }

        public static void Init()
        {
            LocaleDict dict = Locale.GetDictionary("Scada.Comm.Drivers.DrvDbImport.View.Forms.FrmDeviceConfig");
            AddRoots = dict[nameof(AddRoots)];
            ConnectionNode = dict[nameof(ConnectionNode)];
            CommandName = dict[nameof(CommandName)];
            CommandsNode = dict[nameof(CommandsNode)];
            QueryName = dict[nameof(QueryName)];
            QueriesNode = dict[nameof(QueriesNode)];
        }
    }
}
