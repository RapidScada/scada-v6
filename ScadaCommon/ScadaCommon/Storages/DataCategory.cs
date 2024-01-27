/*
 * Copyright 2024 Rapid Software LLC
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
 * Module   : ScadaCommon
 * Summary  : Sets the categories of data in the storage
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

namespace Scada.Storages
{
    /// <summary>
    /// Specifies the categories of data in the storage.
    /// <para>Задаёт категории данных в хранилище.</para>
    /// </summary>
    public enum DataCategory
    {
        /// <summary>
        /// Application configuration.
        /// </summary>
        Config,

        /// <summary>
        /// Application storage.
        /// </summary>
        Storage,

        /// <summary>
        /// Views.
        /// </summary>
        View
    }
}
