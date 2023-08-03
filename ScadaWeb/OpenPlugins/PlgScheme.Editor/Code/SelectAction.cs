// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgScheme.Editor.Code
{
    /// <summary>
    /// Specifies the actions when selecting scheme components.
    /// <para>Задаёт действия при выборе компонентов схемы.</para>
    /// </summary>
    public enum SelectAction
    {
        /// <summary>
        /// Select component.
        /// </summary>
        Select,

        /// <summary>
        /// Append component to selection.
        /// </summary>
        Append,

        /// <summary>
        /// Deselect component.
        /// </summary>
        Deselect,

        /// <summary>
        /// Deselect all components.
        /// </summary>
        DeselectAll
    }
}
