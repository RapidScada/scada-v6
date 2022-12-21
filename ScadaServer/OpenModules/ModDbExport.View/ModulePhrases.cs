// Copyright (c) Rapid Software LLC. All rights reserved.

using Scada.Lang;

namespace Scada.Server.Modules.ModDbExport.View
{
    /// <summary>
    /// The phrases used by the module.
    /// <para>Фразы, используемые модулем.</para>
    /// </summary>
    internal class ModulePhrases
    {
        // Scada.Server.Modules.ModDbExport.View.Forms.FrmModuleConfig
        public static string AddTargets { get; private set; }
        public static string ArchiveExportOptionsNode { get; private set; }
        public static string ConnectionOptionsNode { get; private set; }
        public static string CopySuffix { get; private set; }
        public static string CurrentExportOptionsNode { get; private set; }
        public static string ExportOptionsNode { get; private set; }        
        public static string GeneralOptionsNode { get; private set; }
        public static string TargetNameEmpty { get; private set; }
        public static string TargetNameNotUnique { get; private set; }

        public static void Init()
        {
            LocaleDict dict = Locale.GetDictionary("Scada.Server.Modules.ModDbExport.View.Forms.FrmModuleConfig");
            ArchiveExportOptionsNode = dict[nameof(ArchiveExportOptionsNode)];
            AddTargets = dict[nameof(AddTargets)];
            ConnectionOptionsNode = dict[nameof(ConnectionOptionsNode)];
            CopySuffix = dict[nameof(CopySuffix)];
            CurrentExportOptionsNode = dict[nameof(CurrentExportOptionsNode)];        
            ExportOptionsNode = dict[nameof(ExportOptionsNode)];
            GeneralOptionsNode = dict[nameof(GeneralOptionsNode)];
            TargetNameEmpty = dict[nameof(TargetNameEmpty)];
            TargetNameNotUnique = dict[nameof(TargetNameNotUnique)];
        }
    }
}
