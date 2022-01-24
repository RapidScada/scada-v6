// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Forms;
using Scada.Lang;
using Scada.Server.Archives;
using Scada.Server.Config;
using Scada.Server.Modules.ModArcBasic.View.Forms;
using System.Windows.Forms;

namespace Scada.Server.Modules.ModArcBasic.View
{
    /// <summary>
    /// Implements the server module user interface.
    /// <para>Реализует пользовательский интерфейс серверного модуля.</para>
    /// </summary>
    public class ModArcBasicView : ModuleView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ModArcBasicView()
        {
            CanShowProperties = true;
        }


        /// <summary>
        /// Gets the module name.
        /// </summary>
        public override string Name
        {
            get
            {
                return Locale.IsRussian ? "Базовый архив" : "Basic Archive";
            }
        }

        /// <summary>
        /// Gets the module description.
        /// </summary>
        public override string Descr
        {
            get
            {
                return Locale.IsRussian ?
                    "Модуль предоставляет быстрое и надёжное архивирование данных в файлы." :
                    "The module provides fast and reliable data archiving into files.";
            }
        }


        /// <summary>
        /// Loads language dictionaries.
        /// </summary>
        public override void LoadDictionaries()
        {
            if (!Locale.LoadDictionaries(AppDirs.LangDir, ModuleUtils.ModuleCode, out string errMsg))
                ScadaUiUtils.ShowError(errMsg);
        }

        /// <summary>
        /// Shows a modal dialog box for editing module properties.
        /// </summary>
        public override bool ShowProperties()
        {
            return new FrmArcDir(AppDirs).ShowDialog() == DialogResult.OK;
        }

        /// <summary>
        /// Indicates whether the module can create an archive of the specified kind.
        /// </summary>
        public override bool CanCreateArchive(ArchiveKind kind)
        {
            return kind == ArchiveKind.Current || kind == ArchiveKind.Historical || kind == ArchiveKind.Events;
        }

        /// <summary>
        /// Creates a new archive user interface.
        /// </summary>
        public override ArchiveView CreateArchiveView(ArchiveConfig archiveConfig)
        {
            return new BasicArchiveView(this, archiveConfig);
        }
    }
}
