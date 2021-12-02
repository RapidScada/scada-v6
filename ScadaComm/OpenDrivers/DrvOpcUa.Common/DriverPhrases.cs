// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.Comm.Drivers.DrvOpcUa
{
    /// <summary>
    /// The phrases used by the driver.
    /// <para>Фразы, используемые драйвером.</para>
    /// </summary>
    public static class DriverPhrases
    {
        // Scada.Comm.Drivers.DrvOpcUa.View.Forms.FrmConfig
        public static string ConnectServerError { get; private set; }
        public static string DisconnectServerError { get; private set; }
        public static string BrowseServerError { get; private set; }
        public static string GetDataTypeError { get; private set; }
        public static string ServerUrlRequired { get; private set; }
        public static string EmptyNode { get; private set; }
        public static string SubscriptionsNode { get; private set; }
        public static string CommandsNode { get; private set; }
        public static string EmptySubscription { get; private set; }
        public static string EmptyItem { get; private set; }
        public static string EmptyCommand { get; private set; }
        public static string UnknownDataType { get; private set; }

        // Scada.Comm.Drivers.DrvOpcUa.View.Forms.FrmNodeAttr
        public static string ReadAttrError { get; private set; }

        public static void Init()
        {
            LocaleDict dict = Locale.GetDictionary("Scada.Comm.Drivers.DrvOpcUa.View.Forms.FrmConfig");
            ConnectServerError = dict[nameof(ConnectServerError)];
            DisconnectServerError = dict[nameof(DisconnectServerError)];
            BrowseServerError = dict[nameof(BrowseServerError)];
            GetDataTypeError = dict[nameof(GetDataTypeError)];
            ServerUrlRequired = dict[nameof(ServerUrlRequired)];
            EmptyNode = dict[nameof(EmptyNode)];
            SubscriptionsNode = dict[nameof(SubscriptionsNode)];
            CommandsNode = dict[nameof(CommandsNode)];
            EmptySubscription = dict[nameof(EmptySubscription)];
            EmptyItem = dict[nameof(EmptyItem)];
            EmptyCommand = dict[nameof(EmptyCommand)];
            UnknownDataType = dict[nameof(UnknownDataType)];

            dict = Locale.GetDictionary("Scada.Comm.Drivers.DrvOpcUa.View.Forms.FrmNodeAttr");
            ReadAttrError = dict[nameof(ReadAttrError)];
        }
    }
}
