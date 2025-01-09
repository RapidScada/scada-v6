// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel;

namespace Scada.Web.Plugins.SchSvgExtComp
{
    /// <summary>
    /// Navigation targets for a link
    /// <para>Цели перехода по ссылке</para>
    /// </summary>
    [TypeConverter(typeof(EnumConverter))]
    public enum LinkTarget
    {
        /// <summary>
        /// То же окно
        /// </summary>
        [Description("Same frame")]
        Self,

        /// <summary>
        /// Новая вкладка
        /// </summary>
        [Description("New tab")]
        Blank,

        /// <summary>
        /// Всплывающее окно
        /// </summary>
        [Description("Popup")]
        Popup
    }
}
