// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel;

namespace Scada.Web.Plugins.PlgScheme.Model.DataTypes
{
    /// <summary>
    /// Logical operators
    /// <para>Логические операторы</para>
    /// </summary>
    [TypeConverter(typeof(EnumConverter))]
    public enum LogicalOperators
    {
        /// <summary>
        /// Не задан
        /// </summary>
        [Description("None")]
        None,

        /// <summary>
        /// И
        /// </summary>
        [Description("And")]
        And,

        /// <summary>
        /// Или
        /// </summary>
        [Description("Or")]
        Or
    }
}
