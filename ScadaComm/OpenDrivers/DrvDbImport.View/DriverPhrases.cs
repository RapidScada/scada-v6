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
        public static string CommandName { get; private set; }
        public static string CommandsNode { get; private set; }
        public static string QueryName { get; private set; }
        public static string QueriesNode { get; private set; }
        public static string QueryNameEmpty { get; private set; }
        public static string CommandNameEmpty { get; private set; }
        public static string QueryNameNotUnique { get; private set; }
        public static string CommandNameNotUnique { get; private set; }
        public static string SelectChildNode { get; private set; }

        // Scada.Comm.Drivers.DrvDbImport.View.Controls.CtrlQuery
        public static string SingleRow { get; private set; }
        public static string NoSingleRow { get; private set; }


        public static void Init()
        {
            LocaleDict dict = Locale.GetDictionary("Scada.Comm.Drivers.DrvDbImport.View.Forms.FrmDeviceConfig");
            ConnectionNode = dict[nameof(ConnectionNode)];
            CommandName = dict[nameof(CommandName)];
            CommandsNode = dict[nameof(CommandsNode)];
            QueryName = dict[nameof(QueryName)];
            QueriesNode = dict[nameof(QueriesNode)];
            QueryNameEmpty = dict[nameof(QueryNameEmpty)];
            CommandNameEmpty = dict[nameof(CommandNameEmpty)];
            QueryNameNotUnique = dict[nameof(QueryNameNotUnique)];
            CommandNameNotUnique = dict[nameof(CommandNameNotUnique)];
            SelectChildNode = dict[nameof(SelectChildNode)];

            dict = Locale.GetDictionary("Scada.Comm.Drivers.DrvDbImport.View.Controls.CtrlQuery");
            SingleRow = dict[nameof(SingleRow)];
            NoSingleRow = dict[nameof(NoSingleRow)];
        }
    }
}
