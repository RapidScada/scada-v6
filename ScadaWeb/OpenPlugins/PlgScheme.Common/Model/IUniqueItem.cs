// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgScheme.Model
{
    /// <summary>
    /// Specifies items with unique keys
    /// <para>Определяет элементы с уникальными ключами</para>
    /// </summary>
    public interface IUniqueItem
    {
        /// <summary>
        /// Проверить ключ на уникальность
        /// </summary>
        bool KeyIsUnique(string key);
    }
}
