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
        public static string LogsNode { get; private set; }

        // Scada.Admin.Extensions.ExtCommConfig.Code.ExtensionUtils
        public static string DeviceNotSupported { get; private set; }
        public static string UnableCreateDeviceView { get; private set; }
        public static string NoDeviceProperties { get; private set; }

        // Scada.Admin.Extensions.ExtCommConfig.Controls.CtrlCnlCreate1
        public static string AllCommLines { get; private set; }
        public static string DeviceInfo { get; private set; }
        public static string DeviceNotFound { get; private set; }
        public static string NoDeviceSelected { get; private set; }

        // Scada.Admin.Extensions.ExtCommConfig.Controls.CtrlExtensionMenu
        public static string ConfirmDeleteLine { get; private set; }
        public static string CnlNodeNotFound { get; private set; }

        // Scada.Admin.Extensions.ExtCommConfig.Controls.CtrlLineMain
        public static string UndefinedChannelType { get; private set; }
        public static string ChannelNotSupported { get; private set; }
        public static string UnableCreateChannelView { get; private set; }
        public static string NoChannelProperties { get; private set; }

        // Scada.Admin.Extensions.ExtCommConfig.Forms.FrmCnlCreate
        public static string CreateCnlsStep1 { get; private set; }
        public static string CreateCnlsStep2 { get; private set; }
        public static string CreateCnlsStep3 { get; private set; }
        public static string CreateCnlsCompleted { get; private set; }

        // Scada.Admin.Extensions.ExtCommConfig.Forms.FrmCommLogs
        public static string AppFilter { get; private set; }
        public static string LinesFilter { get; private set; }
        public static string DevicesFilter { get; private set; }

        // Scada.Admin.Extensions.ExtCommConfig.Forms.FrmDataSources
        public static string DriverNotSpecified { get; private set; }
        public static string DataSourceNotSupported { get; private set; }
        public static string UnableCreateDataSourceView { get; private set; }
        public static string NoDataSourceProperties { get; private set; }

        // Scada.Admin.Extensions.ExtProjectTools.Forms.FrmDeviceAdd
        public static string DeviceExistsInConfigDatabase { get; private set; }
        public static string DeviceExistsInLineConfig { get; private set; }
        public static string LineNotFoundInCommConfig { get; private set; }
        public static string ChooseLine { get; private set; }

        // Scada.Admin.Extensions.ExtProjectTools.Forms.FrmLineAdd
        public static string LineExistsInConfigDatabase { get; private set; }
        public static string LineExistsInCommConfig { get; private set; }

        // Scada.Admin.Extensions.ExtCommConfig.Forms.FrmLineConfig
        public static string LineConfigTitle { get; private set; }

        // Scada.Admin.Extensions.ExtCommConfig.Forms.FrmLineStats
        public static string LineStatsTitle { get; private set; }

        // Scada.Admin.Extensions.ExtCommConfig.Forms.FrmSync
        public static string SyncCompleted { get; private set; }
        public static string NoDataToSync { get; private set; }
        public static string SyncCompletedWithError { get; private set; }


        public static void Init()
        {
            LocaleDict dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtCommConfig.ExtCommConfigLogic");
            GeneralOptionsNode = dict["GeneralOptionsNode"];
            DriversNode = dict["DriversNode"];
            DataSourcesNode = dict["DataSourcesNode"];
            LinesNode = dict["LinesNode"];
            LineOptionsNode = dict["LineOptionsNode"];
            LineStatsNode = dict["LineStatsNode"];
            LogsNode = dict["LogsNode"];

            dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtCommConfig.Code.ExtensionUtils");
            DeviceNotSupported = dict["DeviceNotSupported"];
            UnableCreateDeviceView = dict["UnableCreateDeviceView"];
            NoDeviceProperties = dict["NoDeviceProperties"];

            dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtCommConfig.Controls.CtrlCnlCreate1");
            AllCommLines = dict["AllCommLines"];
            DeviceInfo = dict["DeviceInfo"];
            DeviceNotFound = dict["DeviceNotFound"];
            NoDeviceSelected = dict["NoDeviceSelected"];

            dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtCommConfig.Controls.CtrlExtensionMenu");
            ConfirmDeleteLine = dict["ConfirmDeleteLine"];
            CnlNodeNotFound = dict["CnlNodeNotFound"];

            dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtCommConfig.Controls.CtrlLineMain");
            UndefinedChannelType = dict["UndefinedChannelType"];
            ChannelNotSupported = dict["ChannelNotSupported"];
            UnableCreateChannelView = dict["UnableCreateChannelView"];
            NoChannelProperties = dict["NoChannelProperties"];

            dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtCommConfig.Forms.FrmCnlCreate");
            CreateCnlsStep1 = dict["CreateCnlsStep1"];
            CreateCnlsStep2 = dict["CreateCnlsStep2"];
            CreateCnlsStep3 = dict["CreateCnlsStep3"];
            CreateCnlsCompleted = dict["CreateCnlsCompleted"];

            dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtCommConfig.Forms.FrmCommLogs");
            AppFilter = dict["AppFilter"];
            LinesFilter = dict["LinesFilter"];
            DevicesFilter = dict["DevicesFilter"];

            dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtCommConfig.Forms.FrmDataSources");
            DriverNotSpecified = dict["DriverNotSpecified"];
            DataSourceNotSupported = dict["DataSourceNotSupported"];
            UnableCreateDataSourceView = dict["UnableCreateDataSourceView"];
            NoDataSourceProperties = dict["NoDataSourceProperties"];

            dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtCommConfig.Forms.FrmDeviceAdd");
            DeviceExistsInConfigDatabase = dict["DeviceExistsInConfigDatabase"];
            DeviceExistsInLineConfig = dict["DeviceExistsInLineConfig"];
            LineNotFoundInCommConfig = dict["LineNotFoundInCommConfig"];
            ChooseLine = dict["ChooseLine"];

            dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtCommConfig.Forms.FrmLineAdd");
            LineExistsInConfigDatabase = dict["LineExistsInConfigDatabase"];
            LineExistsInCommConfig = dict["LineExistsInCommConfig"];

            dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtCommConfig.Forms.FrmLineConfig");
            LineConfigTitle = dict["LineConfigTitle"];

            dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtCommConfig.Forms.FrmLineStats");
            LineStatsTitle = dict["LineStatsTitle"];

            dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtCommConfig.Forms.FrmSync");
            SyncCompleted = dict["SyncCompleted"];
            SyncCompletedWithError = dict["SyncCompletedWithError"];
            NoDataToSync = dict["NoDataToSync"];
        }
    }
}
