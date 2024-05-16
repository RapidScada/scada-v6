// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Web.Plugins.PlgMimic.MimicModel;

namespace Scada.Web.Plugins.PlgMimicEditor.Code
{
    /// <summary>
    /// Represents an instance of a mimic diagram being edited.
    /// <para>Представляет экземпляр реактируемой мнемосхемы.</para>
    /// </summary>
    internal class MimicInstance
    {
        /// <summary>
        /// Gets or sets the mimic file name.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the editor key.
        /// </summary>
        public long EditorKey { get; set; }

        /// <summary>
        /// Gets or sets the mimic model.
        /// </summary>
        public Mimic Mimic { get; set; }
    }
}
