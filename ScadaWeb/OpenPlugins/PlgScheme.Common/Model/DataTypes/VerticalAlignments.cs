// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel;

namespace Scada.Web.Plugins.PlgScheme.Model.DataTypes
{
    /// <summary>
    /// Vertical alignments
    /// <para>Вертикальные выравнивания</para>
    /// </summary>
    [TypeConverter(typeof(EnumConverter))]
    public enum VerticalAlignments
    {
        /// <summary>
        /// Сверху
        /// </summary>
        [Description("Top")]
        Top,

        /// <summary>
        /// По центру
        /// </summary>
        [Description("Center")]
        Center,

        /// <summary>
        /// Снизу
        /// </summary>
        [Description("Bottom")]
        Bottom
    }
}
