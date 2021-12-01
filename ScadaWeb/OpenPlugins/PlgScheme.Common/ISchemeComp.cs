// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgScheme
{
    /// <summary>
    /// Indicates that a plugin provides scheme components
    /// <para>Показывает, что плагин предоставляет компоненты схем</para>
    /// </summary>
    public interface ISchemeComp
    {
        /// <summary>
        /// Получить спецификацию библиотеки компонентов
        /// </summary>
        CompLibSpec CompLibSpec { get; }
    }
}
