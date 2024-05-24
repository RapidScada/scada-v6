// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel;

namespace Scada.Web.Plugins.PlgScheme.Model.DataTypes
{
    /// <summary>
    /// Horizontal alignments
    /// <para>Горизонтальные выравнивания</para>
    /// </summary>
    [TypeConverter(typeof(EnumConverter))]
    public enum HorizontalAlignments
    {
        /// <summary>
        /// Слева
        /// </summary>
        [Description("Left")]
        Left,

        /// <summary>
        /// По центру
        /// </summary>
        [Description("Center")]
        Center,

        /// <summary>
        /// Справа
        /// </summary>
        [Description("Right")]
        Right
    }
}
