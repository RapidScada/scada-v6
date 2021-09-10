// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Scada.Server.Modules.ModArcBasic.Logic
{
    /// <summary>
    /// The class provides helper methods for the module.
    /// <para>Класс, предоставляющий вспомогательные методы для модуля.</para>
    /// </summary>
    internal static class ModuleUtils
    {
        /// <summary>
        /// The module code.
        /// </summary>
        public const string ModuleCode = "ModArcBasic";
        /// <summary>
        ///  The maximum number of entries that can be stored in the cache.
        /// </summary>
        public const int CacheCapacity = 100;
        /// <summary>
        /// Determines how long an item is stored in the cache.
        /// </summary>
        public static readonly TimeSpan CacheExpiration = TimeSpan.FromMinutes(1);
    }
}
