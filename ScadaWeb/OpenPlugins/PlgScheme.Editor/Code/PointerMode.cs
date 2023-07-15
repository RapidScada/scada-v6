// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgScheme.Editor.Code
{
    /// <summary>
    /// Specifies the modes of the editor mouse pointer.
    /// <para>Определяет режимы указателя мыши редактора.</para>
    /// </summary>
    public enum PointerMode
    {
        /// <summary>
        /// Select components.
        /// </summary>
        Select,

        /// <summary>
        /// Create a new component.
        /// </summary>
        Create,

        /// <summary>
        /// Paste of the copied components.
        /// </summary>
        Paste,

        /// <summary>
        /// Special paste of the copied components.
        /// </summary>
        PasteSpecial
    }
}
