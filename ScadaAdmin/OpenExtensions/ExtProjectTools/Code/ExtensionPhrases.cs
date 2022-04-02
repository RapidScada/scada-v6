// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.Admin.Extensions.ExtProjectTools.Code
{
    /// <summary>
    /// The phrases used by the extension.
    /// <para>Фразы, используемые расширением.</para>
    /// </summary>
    internal class ExtensionPhrases
    {
        // Scada.Admin.Extensions.ExtProjectTools.Code.ChannelMap
        public static string MapByDeviceTitle { get; private set; }
        public static string MapByObjectTitle { get; private set; }
        public static string ChannelsCaption { get; private set; }
        public static string NoChannels { get; private set; }
        public static string GenerateChannelMapError { get; private set; }

        // Scada.Admin.Extensions.ExtProjectTools.Code.DeviceMap
        public static string DeviceMapTitle { get; private set; }
        public static string DevicesCaption { get; private set; }
        public static string NoDevices { get; private set; }
        public static string EmptyCommLine { get; private set; }
        public static string GenerateDeviceMapError { get; private set; }

        // Scada.Admin.Extensions.ExtProjectTools.Code.IntegrityCheck
        public static string IntegrityCheckTitle { get; private set; }
        public static string TableCorrect { get; private set; }
        public static string TableHasErrors { get; private set; }
        public static string LostPrimaryKeys { get; private set; }
        public static string BaseCorrect { get; private set; }
        public static string BaseHasErrors { get; private set; }
        public static string IntegrityCheckError { get; private set; }

        // Scada.Admin.Extensions.ExtProjectTools.Forms.FrmCnlClone
        public static string KeepUnchanged { get; private set; }
        public static string CloneChannelsCompleted { get; private set; }
        public static string CloneChannelsError { get; private set; }

        // Scada.Admin.Extensions.ExtProjectTools.Forms.FrmTableExport
        public static string ExportTableFilter { get; private set; }
        public static string ExportTableError { get; private set; }

        // Scada.Admin.Extensions.ExtProjectTools.Forms.FrmTableImport
        public static string ImportTableFilter { get; private set; }
        public static string ImportTableCompleted { get; private set; }
        public static string ImportTableError { get; private set; }

        public static void Init()
        {
            LocaleDict dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtProjectTools.Code.ChannelMap");
            MapByDeviceTitle = dict["MapByDeviceTitle"];
            MapByObjectTitle = dict["MapByObjectTitle"];
            ChannelsCaption = dict["ChannelsCaption"];
            NoChannels = dict["NoChannels"];
            GenerateChannelMapError = dict["GenerateChannelMapError"];

            dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtProjectTools.Code.DeviceMap");
            DeviceMapTitle = dict["DeviceMapTitle"];
            DevicesCaption = dict["DevicesCaption"];
            NoDevices = dict["NoDevices"];
            EmptyCommLine = dict["EmptyCommLine"];
            GenerateDeviceMapError = dict["GenerateDeviceMapError"];

            dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtProjectTools.Code.IntegrityCheck");
            IntegrityCheckTitle = dict["IntegrityCheckTitle"];
            TableCorrect = dict["TableCorrect"];
            TableHasErrors = dict["TableHasErrors"];
            LostPrimaryKeys = dict["LostPrimaryKeys"];
            BaseHasErrors = dict["BaseHasErrors"];
            IntegrityCheckError = dict["IntegrityCheckError"];

            dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtProjectTools.Forms.FrmCnlClone");
            KeepUnchanged = dict["KeepUnchanged"];
            CloneChannelsCompleted = dict["CloneChannelsCompleted"];
            CloneChannelsError = dict["CloneChannelsError"];

            dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtProjectTools.Forms.FrmTableExport");
            ExportTableFilter = dict["ExportTableFilter"];
            ExportTableError = dict["ExportTableError"];

            dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtProjectTools.Forms.FrmTableImport");
            ImportTableFilter = dict["ImportTableFilter"];
            ImportTableCompleted = dict["ImportTableCompleted"];
            ImportTableError = dict["ImportTableError"];
        }
    }
}
