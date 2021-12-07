// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Web.Plugins.PlgScheme.Model;
using System;

namespace Scada.Web.Plugins.PlgScheme.Models
{
    /// <summary>
    /// Represents a package containing scheme document properties.
    /// <para>Представляет пакет, содержащий свойства документа схемы.</para>
    /// </summary>
    public class DocumentPacket : SchemePacket
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DocumentPacket(SchemeView schemeView)
        {
            ArgumentNullException.ThrowIfNull(schemeView, nameof(schemeView));
            ViewStamp = schemeView.ViewStamp.ToString();
            SchemeDoc = schemeView.SchemeDoc;
        }

        /// <summary>
        /// Gets the scheme document properties.
        /// </summary>
        public SchemeDocument SchemeDoc { get; }
    }
}
