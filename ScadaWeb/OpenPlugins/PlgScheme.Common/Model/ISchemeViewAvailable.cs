// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgScheme.Model
{
    /// <summary>
    /// Defines the scheme view reference.
    /// <para>Определяет ссылку на представление схемы.</para>
    /// </summary>
    public interface ISchemeViewAvailable
    {
        /// <summary>
        /// Gets or sets the reference to a scheme view.
        /// </summary>
        SchemeView SchemeView { get; set; }
    }
}
