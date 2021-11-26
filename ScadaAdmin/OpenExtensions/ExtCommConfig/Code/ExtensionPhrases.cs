// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.Admin.Extensions.ExtCommConfig.Code
{
    /// <summary>
    /// The phrases used by the extension.
    /// <para>Фразы, используемые расширением.</para>
    /// </summary>
    internal class ExtensionPhrases
    {
        // Scada.Admin.Extensions.ExtCommConfig.ExtCommConfigLogic
        public static string GeneralOptionsNode { get; private set; }
        public static string DriversNode { get; private set; }
        public static string DataSourcesNode { get; private set; }
        public static string LinesNode { get; private set; }
        public static string LineOptionsNode { get; private set; }
        public static string LineStatsNode { get; private set; }

        // Scada.Admin.Extensions.ExtCommConfig.Code.ExtensionUtils
        public static string DeviceNotSupported { get; private set; }
        public static string UnableCreateDeviceView { get; private set; }
        public static string NoDeviceProperties { get; private set; }

        // Scada.Admin.Extensions.ExtCommConfig.Controls.CtrlExtensionMenu
        public static string ConfirmDeleteLine { get; private set; }

        // Scada.Admin.Extensions.ExtCommConfig.Controls.CtrlLineMain
        public static string UndefinedChannelType { get; private set; }
        public static string ChannelNotSupported { get; private set; }
        public static string UnableCreateChannelView { get; private set; }
        public static string NoChannelProperties { get; private set; }

        // Scada.Admin.Extensions.ExtCommConfig.Forms.FrmDataSources
        public static string DriverNotSpecified { get; private set; }
        public static string DataSourceNotSupported { get; private set; }
        public static string UnableCreateDataSourceView { get; private set; }
        public static string NoDataSourceProperties { get; private set; }

        // Scada.Admin.Extensions.ExtProjectTools.Forms.FrmDeviceAdd
        public static string DeviceExistsInConfigBase { get; private set; }
        public static string DeviceExistsInLineConfig { get; private set; }
        public static string LineNotFoundInCommConfig { get; private set; }
        public static string ChooseLine { get; private set; }

        // Scada.Admin.Extensions.ExtProjectTools.Forms.FrmDeviceCommand
        public static string SendCommandError { get; private set; }

        // Scada.Admin.Extensions.ExtProjectTools.Forms.FrmLineAdd
        public static string LineExistsInConfigBase { get; private set; }
        public static string LineExistsInCommConfig { get; private set; }

        // Scada.Admin.Extensions.ExtCommConfig.Forms.FrmLineConfig
        public static string LineConfigTitle { get; private set; }

        // Scada.Admin.Extensions.ExtCommConfig.Forms.FrmLineStats
        public static string LineStatsTitle { get; private set; }

        public static void Init()
        {
            LocaleDict dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtCommConfig.ExtCommConfigLogic");
            GeneralOptionsNode = dict["GeneralOptionsNode"];
            DriversNode = dict["DriversNode"];
            DataSourcesNode = dict["DataSourcesNode"];
            LinesNode = dict["LinesNode"];
            LineOptionsNode = dict["LineOptionsNode"];
            LineStatsNode = dict["LineStatsNode"];

            dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtCommConfig.Code.ExtensionUtils");
            DeviceNotSupported = dict["DeviceNotSupported"];
            UnableCreateDeviceView = dict["UnableCreateDeviceView"];
            NoDeviceProperties = dict["NoDeviceProperties"];

            dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtCommConfig.Controls.CtrlExtensionMenu");
            ConfirmDeleteLine = dict["ConfirmDeleteLine"];

            dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtCommConfig.Controls.CtrlLineMain");
            UndefinedChannelType = dict["UndefinedChannelType"];
            ChannelNotSupported = dict["ChannelNotSupported"];
            UnableCreateChannelView = dict["UnableCreateChannelView"];
            NoChannelProperties = dict["NoChannelProperties"];

            dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtCommConfig.Forms.FrmDataSources");
            DriverNotSpecified = dict["DriverNotSpecified"];
            DataSourceNotSupported = dict["DataSourceNotSupported"];
            UnableCreateDataSourceView = dict["UnableCreateDataSourceView"];
            NoDataSourceProperties = dict["NoDataSourceProperties"];

            dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtCommConfig.Forms.FrmDeviceAdd");
            DeviceExistsInConfigBase = dict["DeviceExistsInConfigBase"];
            DeviceExistsInLineConfig = dict["DeviceExistsInLineConfig"];
            LineNotFoundInCommConfig = dict["LineNotFoundInCommConfig"];
            ChooseLine = dict["ChooseLine"];

            dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtCommConfig.Forms.FrmDeviceCommand");
            SendCommandError = dict["SendCommandError"];

            dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtCommConfig.Forms.FrmLineAdd");
            LineExistsInConfigBase = dict["LineExistsInConfigBase"];
            LineExistsInCommConfig = dict["LineExistsInCommConfig"];

            dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtCommConfig.Forms.FrmLineConfig");
            LineConfigTitle = dict["LineConfigTitle"];

            dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtCommConfig.Forms.FrmLineStats");
            LineStatsTitle = dict["LineStatsTitle"];
        }
    }
}
