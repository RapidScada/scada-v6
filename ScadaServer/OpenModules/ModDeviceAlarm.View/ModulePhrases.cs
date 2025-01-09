// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.Server.Modules.ModDeviceAlarm.View
{
    /// <summary>
    /// The phrases used by the module.
    /// <para>Фразы, используемые модулем.</para>
    /// </summary>
    internal class ModulePhrases
    {
        // Scada.Server.Modules.ModDeviceAlarm.View.Forms.FrmModuleConfig
        public static string AddTargets { get; private set; }        
        public static string CopySuffix { get; private set; }
        public static string CurrentExportOptionsNode { get; private set; }
        public static string GeneralOptionsNode { get; private set; }
        public static string SelectChildNode { get; private set; }
        public static string TargetName { get; private set; }
        public static string TargetNameNotUnique { get; private set; }
        public static string TriggerName { get; private set; }
        public static string TriggersNode { get; private set; }
        public static string UnnamedTrigger { get; private set; }

        public static void Init()
        {
            LocaleDict dict = Locale.GetDictionary("Scada.Server.Modules.ModDeviceAlarm.View.Forms.FrmModuleConfig");
            AddTargets = dict[nameof(AddTargets)];
            CopySuffix = dict[nameof(CopySuffix)];
            CurrentExportOptionsNode = dict[nameof(CurrentExportOptionsNode)];        
            GeneralOptionsNode = dict[nameof(GeneralOptionsNode)];
            SelectChildNode = dict[nameof(SelectChildNode)];
            TargetName = dict[nameof(TargetName)];
            TargetNameNotUnique = dict[nameof(TargetNameNotUnique)];
            TriggerName = dict[nameof(TriggerName)];
            TriggersNode = dict[nameof(TriggersNode)];
            UnnamedTrigger = dict[nameof(UnnamedTrigger)];
        }
    }
}
