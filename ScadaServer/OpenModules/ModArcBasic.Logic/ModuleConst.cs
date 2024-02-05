// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Scada.Server.Modules.ModArcBasic.Logic
{
    /// <summary>
    /// Specifies the constants used by the module.
    /// <para>Задаёт константы, используемые модулем.</para>
    /// </summary>
    internal static class ModuleConst
    {
        /// <summary>
        ///  The maximum number of entries that can be stored in the cache.
        /// </summary>
        public const int CacheCapacity = 1000;

        /// <summary>
        /// Determines how long an item is stored in the cache.
        /// </summary>
        public static readonly TimeSpan CacheExpiration = TimeSpan.FromMinutes(1);

        /// <summary>
        /// The number of slices written in one iteration.
        /// </summary>
        public const int SlicesPerIteration = 10;

        /// <summary>
        /// The number of events written in one iteration.
        /// </summary>
        public const int EventsPerIteration = 10;
    }
}
