// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Server.Archives;
using Scada.Server.Config;
using Scada.Server.Modules.ModArcInfluxDb.View.Forms;

namespace Scada.Server.Modules.ModArcInfluxDb.View
{
    /// <summary>
    /// Implements the archive user interface.
    /// <para>Реализует пользовательский интерфейс архива.</para>
    /// </summary>
    internal class InfluxArchiveView : ArchiveView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public InfluxArchiveView(ModuleView parentView, ArchiveConfig archiveConfig)
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
                ArchiveKind.Historical => new FrmInfluxHAO(AppDirs, ArchiveConfig),
                _ => null
            };

            return form != null && form.ShowDialog() == DialogResult.OK;
        }
    }
}
