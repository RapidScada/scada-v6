// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgScheme.Models
{
    /// <summary>
    /// Represents a package for transferring a part of scheme view.
    /// <para>Представляет собой пакет для передачи части представления схемы.</para>
    /// </summary>
    public abstract class SchemePacket
    {
        /// <summary>
        /// Gets or sets the unique view stamp.
        /// </summary>
        /// <remarks>String is used instead of long because JavaScript unable to decode long.</remarks>
        public string ViewStamp { get; set; }
    }
}
