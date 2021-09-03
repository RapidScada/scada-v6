// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.Admin.Extensions.ExtServerConfig.Code
{
    /// <summary>
    /// The phrases used by the extension.
    /// <para>Фразы, используемые расширением.</para>
    /// </summary>
    public class ExtensionPhrases
    {
        // Scada.Server.Shell.Code.ServerShell
        public static string CommonParamsNode { get; private set; }
        public static string SaveParamsNode { get; private set; }
        public static string ArchiveNode { get; private set; }
        public static string CurDataNode { get; private set; }
        public static string MinDataNode { get; private set; }
        public static string HourDataNode { get; private set; }
        public static string EventsNode { get; private set; }
        public static string ModulesNode { get; private set; }
        public static string GeneratorNode { get; private set; }
        public static string StatsNode { get; private set; }

        // Scada.Server.Shell.Forms
        public static string SetProfile { get; private set; }
        public static string Loading { get; private set; }
        public static string CsvFileFilter { get; private set; }
        public static string ExportToCsvError { get; private set; }

        // Scada.Server.Shell.Forms.FrmArchive
        public static string CurDataTitle { get; private set; }
        public static string MinDataTitle { get; private set; }
        public static string HourDataTitle { get; private set; }
        public static string EventsTitle { get; private set; }
        public static string ArcLocal { get; private set; }

        // Scada.Server.Shell.Forms.FrmCommonParams
        public static string ChooseItfDir { get; private set; }
        public static string ChooseArcDir { get; private set; }
        public static string ChooseArcCopyDir { get; private set; }

        // Scada.Server.Shell.Forms.FrmEventTable
        public static string ViewEventsTitle { get; private set; }
        public static string EditEventsTitle { get; private set; }
        public static string SaveEventsConfirm { get; private set; }
        public static string IncorrectEventFilter { get; private set; }
        public static string LoadEventTableError { get; private set; }
        public static string SaveEventTableError { get; private set; }

        // Scada.Server.Shell.Forms.FrmGenData
        public static string IncorrectCnlNum { get; private set; }
        public static string IncorrectCnlVal { get; private set; }

        // Scada.Server.Shell.Forms.FrmGenEvent
        public static string IncorrectOldCnlVal { get; private set; }
        public static string IncorrectNewCnlVal { get; private set; }

        // Scada.Server.Shell.Forms.FrmModules
        public static string ModuleNotFound { get; private set; }

        // Scada.Server.Shell.Forms.FrmSnapshotTable
        public static string ViewSnapshotsTitle { get; private set; }
        public static string EditSnapshotsTitle { get; private set; }
        public static string SaveSnapshotsConfirm { get; private set; }
        public static string IncorrectSnapshotFilter { get; private set; }
        public static string LoadSnapshotTableError { get; private set; }
        public static string SaveSnapshotTableError { get; private set; }

        public static void Init()
        {
            LocaleDict dict = Locale.GetDictionary("Scada.Server.Shell.Code.ServerShell");
            CommonParamsNode = dict["CommonParamsNode"];
            SaveParamsNode = dict["SaveParamsNode"];
            ArchiveNode = dict["ArchiveNode"];
            CurDataNode = dict["CurDataNode"];
            MinDataNode = dict["MinDataNode"];
            HourDataNode = dict["HourDataNode"];
            EventsNode = dict["EventsNode"];
            ModulesNode = dict["ModulesNode"];
            GeneratorNode = dict["GeneratorNode"];
            StatsNode = dict["StatsNode"];

            dict = Locale.GetDictionary("Scada.Server.Shell.Forms");
            SetProfile = dict["SetProfile"];
            Loading = dict["Loading"];
            CsvFileFilter = dict["CsvFileFilter"];
            ExportToCsvError = dict["ExportToCsvError"];

            dict = Locale.GetDictionary("Scada.Server.Shell.Forms.FrmArchive");
            CurDataTitle = dict["CurDataTitle"];
            MinDataTitle = dict["MinDataTitle"];
            HourDataTitle = dict["HourDataTitle"];
            EventsTitle = dict["EventsTitle"];
            ArcLocal = dict["ArcLocal"];

            dict = Locale.GetDictionary("Scada.Server.Shell.Forms.FrmCommonParams");
            ChooseItfDir = dict["ChooseItfDir"];
            ChooseArcDir = dict["ChooseArcDir"];
            ChooseArcCopyDir = dict["ChooseArcCopyDir"];

            dict = Locale.GetDictionary("Scada.Server.Shell.Forms.FrmEventTable");
            ViewEventsTitle = dict["ViewEventsTitle"];
            EditEventsTitle = dict["EditEventsTitle"];
            SaveEventsConfirm = dict["SaveEventsConfirm"];
            IncorrectEventFilter = dict["IncorrectEventFilter"];
            LoadEventTableError = dict["LoadEventTableError"];
            SaveEventTableError = dict["SaveEventTableError"];

            dict = Locale.GetDictionary("Scada.Server.Shell.Forms.FrmGenData");
            IncorrectCnlNum = dict["IncorrectCnlNum"];
            IncorrectCnlVal = dict["IncorrectCnlVal"];

            dict = Locale.GetDictionary("Scada.Server.Shell.Forms.FrmGenEvent");
            IncorrectOldCnlVal = dict["IncorrectOldCnlVal"];
            IncorrectNewCnlVal = dict["IncorrectNewCnlVal"];

            dict = Locale.GetDictionary("Scada.Server.Shell.Forms.FrmModules");
            ModuleNotFound = dict["ModuleNotFound"];

            dict = Locale.GetDictionary("Scada.Server.Shell.Forms.FrmSnapshotTable");
            ViewSnapshotsTitle = dict["ViewSnapshotsTitle"];
            EditSnapshotsTitle = dict["EditSnapshotsTitle"];
            SaveSnapshotsConfirm = dict["SaveSnapshotsConfirm"];
            IncorrectSnapshotFilter = dict["IncorrectSnapshotFilter"];
            LoadSnapshotTableError = dict["LoadSnapshotTableError"];
            SaveSnapshotTableError = dict["SaveSnapshotTableError"];
        }
    }
}
