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
 * Summary  : Represents options of a current data archive
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Config;
using Scada.Server.Archives;

namespace Scada.Server.Modules.ModArcBasic.Logic.Options
{
    /// <summary>
    /// Represents options of a current data archive.
    /// <para>Представляет параметры архива текущих данных.</para>
    /// </summary>
    internal class BasicCAO : CurrentArchiveOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public BasicCAO(CustomOptions options)
            : base(options)
        {
            IsCopy = options.GetValueAsBool("IsCopy");
        }

        /// <summary>
        /// Gets or sets a value indicating whether the archive stores a copy of the data.
        /// </summary>
        public bool IsCopy { get; set; }
    }
}
