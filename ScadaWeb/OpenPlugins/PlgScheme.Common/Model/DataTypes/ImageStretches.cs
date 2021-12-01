// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel;

namespace Scada.Web.Plugins.PlgScheme.Model.DataTypes
{
    /// <summary>
    /// Ways of stretching image
    /// <para>Способы растяжения изображения</para>
    /// </summary>
    [TypeConverter(typeof(EnumConverter))]
    public enum ImageStretches
    {
        /// <summary>
        /// Не задано
        /// </summary>
        [Description("None")]
        None,

        /// <summary>
        /// Заполнить заданный размер
        /// </summary>
        [Description("Fill")]
        Fill,

        /// <summary>
        /// Растянуть пропорционально в рамках заданного размера
        /// </summary>
        [Description("Zoom")]
        Zoom
    }
}
