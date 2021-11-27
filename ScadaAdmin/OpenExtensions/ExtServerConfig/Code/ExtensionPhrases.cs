// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.Admin.Extensions.ExtServerConfig.Code
{
    /// <summary>
    /// The phrases used by the extension.
    /// <para>Фразы, используемые расширением.</para>
    /// </summary>
    internal class ExtensionPhrases
    {
        // Scada.Admin.Extensions.ExtServerConfig.ExtServerConfigLogic
        public static string GeneralOptionsNode { get; private set; }
        public static string ModulesNode { get; private set; }
        public static string ArchivesNode { get; private set; }
        public static string LogsNode { get; private set; }

        // Scada.Admin.Extensions.ExtServerConfig.Forms.FrmArchives
        public static string ModuleNotSpecified { get; private set; }
        public static string ArchiveNotSupported { get; private set; }
        public static string UnableCreateArchiveView { get; private set; }
        public static string NoArchiveProperties { get; private set; }

        // Scada.Admin.Extensions.ExtServerConfig.Forms.FrmServerLogs
        public static string AppFilter { get; private set; }
        public static string ModulesFilter { get; private set; }
        public static string AllFilesFilter { get; private set; }

        public static void Init()
        {
            LocaleDict dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtServerConfig.ExtServerConfigLogic");
            GeneralOptionsNode = dict["GeneralOptionsNode"];
            ModulesNode = dict["ModulesNode"];
            ArchivesNode = dict["ArchivesNode"];
            LogsNode = dict["LogsNode"];

            dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtServerConfig.Forms.FrmArchives");
            ModuleNotSpecified = dict["ModuleNotSpecified"];
            ArchiveNotSupported = dict["ArchiveNotSupported"];
            UnableCreateArchiveView = dict["UnableCreateArchiveView"];
            NoArchiveProperties = dict["NoArchiveProperties"];

            dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtServerConfig.Forms.FrmServerLogs");
            AppFilter = dict["AppFilter"];
            ModulesFilter = dict["ModulesFilter"];
            AllFilesFilter = dict["AllFilesFilter"];
        }
    }
}
