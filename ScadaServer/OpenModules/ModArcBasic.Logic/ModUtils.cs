/*
 * Copyright 2020 Mikhail Shiryaev
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * 
 * Product  : Rapid SCADA
 * Module   : ModArcBasic
 * Summary  : The class provides helper methods for the module
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using System;

namespace Scada.Server.Modules.ModArcBasic.Logic
{
    /// <summary>
    /// The class provides helper methods for the module.
    /// <para>Класс, предоставляющий вспомогательные методы для модуля.</para>
    /// </summary>
    internal static class ModUtils
    {
        /// <summary>
        /// The module code.
        /// </summary>
        public const string ModCode = "ModArcBasic";
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
