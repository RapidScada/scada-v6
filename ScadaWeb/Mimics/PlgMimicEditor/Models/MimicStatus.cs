// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgMimicEditor.Models
{
    /// <summary>
    /// Represents a mimic status.
    /// <para>Представляет статус мнемосхемы.</para>
    /// </summary>
    public class MimicStatus
    {
        /// <summary>
        /// The status indicating no errors.
        /// </summary>
        public static readonly MimicStatus Good = new();


        /// <summary>
        /// Gets or sets a value indicating whether a mimic with the given key is not found.
        /// </summary>
        public bool NotFound { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether a mimic is open in multiple browser windows.
        /// </summary>
        public bool Duplicated { get; set; } = false;
    }
}
