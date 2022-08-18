// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Server.Archives;
using Scada.Server.Config;
using Scada.Server.Modules.ModArcPostgreSql.View.Forms;

namespace Scada.Server.Modules.ModArcPostgreSql.View
{
    /// <summary>
    /// Implements the archive user interface.
    /// <para>Реализует пользовательский интерфейс архива.</para>
    /// </summary>
    internal class PostgreArchiveView : ArchiveView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PostgreArchiveView(ModuleView parentView, ArchiveConfig archiveConfig)
            : base(parentView, archiveConfig)
        {
            CanShowProperties = true;
        }

        /// <summary>
        /// Shows a modal dialog box for editing archive properties.
        /// </summary>
        public override bool ShowProperties()
        {
            Form form = ArchiveConfig.Kind switch
            {
                ArchiveKind.Current => new FrmPostgreCAO(AppDirs, ArchiveConfig),
                ArchiveKind.Historical => new FrmPostgreHAO(AppDirs, ArchiveConfig),
                ArchiveKind.Events => new FrmPostgreEAO(AppDirs, ArchiveConfig),
                _ => null
            };

            return form != null && form.ShowDialog() == DialogResult.OK;
        }
    }
}
