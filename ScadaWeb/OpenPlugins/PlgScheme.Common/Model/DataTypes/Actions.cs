// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel;

namespace Scada.Web.Plugins.PlgScheme.Model.DataTypes
{
    /// <summary>
    /// Actions of a dynamic component
    /// <para>Действия динамического элемента</para>
    /// </summary>
    [TypeConverter(typeof(EnumConverter))]
    public enum Actions
    {
        /// <summary>
        /// Не задано
        /// </summary>
        [Description("None")]
        None,

        /// <summary>
        /// Построить график
        /// </summary>
        [Description("Draw diagram")]
        DrawDiagram,

        /// <summary>
        /// Отправить команду
        /// </summary>
        [Description("Send command")]
        SendCommand,

        /// <summary>
        /// Отправить команду сразу
        /// </summary>
        [Description("Send command now")]
        SendCommandNow
    }
}
