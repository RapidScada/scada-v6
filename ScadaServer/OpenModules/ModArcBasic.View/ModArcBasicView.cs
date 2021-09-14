// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;
using Scada.Server.Archives;
using Scada.Server.Config;

namespace Scada.Server.Modules.ModArcBasic.View
{
    /// <summary>
    /// Implements the server module user interface.
    /// <para>Реализует пользовательский интерфейс серверного модуля.</para>
    /// </summary>
    public class ModArcBasicView : ModuleView
    {
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
            return archiveConfig.Kind switch
            {
                ArchiveKind.Historical => new BasicHAV(this, archiveConfig),
                _ => null
            };
        }
    }
}
