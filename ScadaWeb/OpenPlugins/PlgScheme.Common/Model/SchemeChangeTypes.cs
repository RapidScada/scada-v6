// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgScheme.Model
{
    /// <summary>
    /// Types of scheme changes
    /// <para>Типы изменений схемы</para>
    /// </summary>
    public enum SchemeChangeTypes
    {
        /// <summary>
        /// Нет изменений
        /// </summary>
        None,

        /// <summary>
        /// Изменён документ схемы
        /// </summary>
        SchemeDocChanged,

        /// <summary>
        /// Добавлен компонент
        /// </summary>
        ComponentAdded,

        /// <summary>
        /// Изменён компонент
        /// </summary>
        ComponentChanged,

        /// <summary>
        /// Удалён компонент
        /// </summary>
        ComponentDeleted,

        /// <summary>
        /// Добавлено изображение
        /// </summary>
        ImageAdded,

        /// <summary>
        /// Переименовано изображение
        /// </summary>
        ImageRenamed,

        /// <summary>
        /// Удалено изображение
        /// </summary>
        ImageDeleted
    }
}
