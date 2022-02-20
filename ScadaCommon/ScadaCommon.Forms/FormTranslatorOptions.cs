// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Windows.Forms;

namespace Scada.Forms
{
    /// <summary>
    /// Represents form translator options.
    /// <para>Представляет параметры переводчика форм.</para>
    /// </summary>
    public class FormTranslatorOptions
    {
        /// <summary>
        /// Gets the tool tip component to translate.
        /// </summary>
        public ToolTip ToolTip { get; init; } = null;

        /// <summary>
        /// Gets the context menus to translate.
        /// </summary>
        public ContextMenuStrip[] ContextMenus { get; init; } = null;

        /// <summary>
        /// Gets a value indicating whether translation of user controls should be skipped.
        /// </summary>
        public bool SkipUserControls { get; init; } = true;
    }
}
