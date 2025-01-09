// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel;

namespace Scada.Web.Plugins.SchSvgExtComp
{
    /// <summary>
    /// Possible widths of a popup
    /// <para>Варианты ширины всплывающего окна</para>
    /// </summary>
    [TypeConverter(typeof(EnumConverter))]
    public enum PopupWidth
    {
        /// <summary>
        /// Нормальная
        /// </summary>
        [Description("Normal")]
        Normal,

        /// <summary>
        /// Маленькая
        /// </summary>
        [Description("Small")]
        Small,

        /// <summary>
        /// Большая
        /// </summary>
        [Description("Large")]
        Large
    }
}
