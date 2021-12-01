// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel;

namespace Scada.Web.Plugins.PlgScheme.Model.DataTypes
{
    /// <summary>
    /// Compare operators
    /// <para>Операторы сравнения</para>
    /// </summary>
    [TypeConverter(typeof(EnumConverter))]
    public enum CompareOperators
    {
        /// <summary>
        /// Равно
        /// </summary>
        [Description("=")]
        Equal,

        /// <summary>
        /// Не равно
        /// </summary>
        [Description("<>")]
        NotEqual,

        /// <summary>
        /// Меньше
        /// </summary>
        [Description("<")]
        LessThan,

        /// <summary>
        /// Меньше или равно
        /// </summary>
        [Description("<=")]
        LessThanEqual,

        /// <summary>
        /// Больше
        /// </summary>
        [Description(">")]
        GreaterThan,

        /// <summary>
        /// Больше или равно
        /// </summary>
        [Description(">=")]
        GreaterThanEqual
    }
}
