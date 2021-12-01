// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel;

namespace Scada.Web.Plugins.PlgScheme.Model.DataTypes
{
    /// <summary>
    /// Kinds of displaying input channel value of a dynamic component
    /// <para>Виды отображения значения входного канала динамического компонента</para>
    /// </summary>
    [TypeConverter(typeof(EnumConverter))]
    public enum ShowValueKinds
    {
        /// <summary>
        /// Нет
        /// </summary>
        [Description("No")]
        NotShow,

        /// <summary>
        /// С размерностью
        /// </summary>
        [Description("With unit")]
        ShowWithUnit,

        /// <summary>
        /// Без размерности
        /// </summary>
        [Description("Without unit")]
        ShowWithoutUnit
    }
}
