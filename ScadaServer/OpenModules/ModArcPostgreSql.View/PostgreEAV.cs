// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Server.Archives;
using Scada.Server.Config;
using System.Windows.Forms;

namespace Scada.Server.Modules.ModArcPostgreSql.View
{
    /// <summary>
    /// Implements the event data archive user interface.
    /// <para>Реализует пользовательский интерфейс архива событий.</para>
    /// </summary>
    public class PostgreEAV : ArchiveView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PostgreEAV(ModuleView parentView, ArchiveConfig archiveConfig)
            : base(parentView, archiveConfig)
        {
            CanShowProperties = true;
        }

        /// <summary>
        /// Shows a modal dialog box for editing archive properties.
        /// </summary>
        public override bool ShowProperties()
        {
            //return new FrmEAO(AppDirs, ArchiveConfig).ShowDialog() == DialogResult.OK;
            return false;
        }
    }
}
