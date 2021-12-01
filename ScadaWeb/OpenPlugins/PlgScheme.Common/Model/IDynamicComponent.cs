// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Web.Plugins.PlgScheme.Model.DataTypes;

namespace Scada.Web.Plugins.PlgScheme.Model
{
    /// <summary>
    /// Specifies scheme components bound to input or output channels.
    /// <para>Определяет компоненты схемы, привязаные к входным каналам или каналам управления.</para>
    /// </summary>
    public interface IDynamicComponent
    {
        /// <summary>
        /// Получить или установить действие.
        /// </summary>
        Actions Action { get; set; }

        /// <summary>
        /// Получить или установить номер входного канала.
        /// </summary>
        int InCnlNum { get; set; }

        /// <summary>
        /// Получить или установить номер канала управления.
        /// </summary>
        int CtrlCnlNum { get; set; }
    }
}
