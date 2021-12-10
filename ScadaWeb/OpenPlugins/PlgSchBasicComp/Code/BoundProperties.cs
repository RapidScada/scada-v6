// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel;

namespace Scada.Web.Plugins.PlgSchBasicComp.Code
{
    /// <summary>
    /// Component properties that can be bound to an input channel
    /// <para>Свойства компонента, которые могут быть привязаны к входному каналу</para>
    /// </summary>
    [TypeConverter(typeof(EnumConverter))]
    public enum BoundProperties
    {
        /// <summary>
        /// Не задано
        /// </summary>
        [Description("None")]
        None,

        /// <summary>
        /// Доступность
        /// </summary>
        [Description("Enabled")]
        Enabled,

        /// <summary>
        /// Видимость
        /// </summary>
        [Description("Visible")]
        Visible
    }
}
