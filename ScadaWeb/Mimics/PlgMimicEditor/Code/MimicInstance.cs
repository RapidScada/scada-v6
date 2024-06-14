// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Web.Plugins.PlgMimic.MimicModel;

namespace Scada.Web.Plugins.PlgMimicEditor.Code
{
    /// <summary>
    /// Represents an instance of a mimic diagram being edited.
    /// <para>Представляет экземпляр реактируемой мнемосхемы.</para>
    /// </summary>
    public class MimicInstance
    {
        /// <summary>
        /// Gets the mimic file name.
        /// </summary>
        public string FileName { get; init; }

        /// <summary>
        /// Gets the mimic model.
        /// </summary>
        public Mimic Mimic { get; init; }

        /// <summary>
        /// Gets the mimic key provided by the editor.
        /// </summary>
        public long MimicKey { get; init; }
    }
}
