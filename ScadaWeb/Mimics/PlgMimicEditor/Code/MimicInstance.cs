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

        /// <summary>
        /// Gets the group containing the mimic.
        /// </summary>
        public MimicGroup ParentGroup { get; init; }

        /// <summary>
        /// Gets the timestamp when the mimic was accessed by the client.
        /// </summary>
        public DateTime ClientAccessTime { get; private set; } = DateTime.MinValue;


        /// <summary>
        /// Registers access to the mimic from the client side.
        /// </summary>
        public void RegisterClientActivity()
        {
            ClientAccessTime = DateTime.UtcNow;
        }
    }
}
