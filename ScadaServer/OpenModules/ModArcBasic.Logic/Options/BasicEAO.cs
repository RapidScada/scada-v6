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
 * Summary  : Represents options of an event archive
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Config;
using System;

namespace Scada.Server.Modules.ModArcBasic.Logic.Options
{
    /// <summary>
    /// Represents options of an event archive.
    /// <para>Представляет параметры архива событий.</para>
    /// </summary>
    internal class BasicEAO
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public BasicEAO(CustomOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            IsCopy = options.GetValueAsBool("IsCopy");
            StoragePeriod = options.GetValueAsInt("StoragePeriod", 365);
            LogEnabled = options.GetValueAsBool("LogEnabled");
        }


        /// <summary>
        /// Gets or sets a value indicating whether the archive stores a copy of the data.
        /// </summary>
        public bool IsCopy { get; set; }

        /// <summary>
        /// Gets or sets the data storage period in days.
        /// </summary>
        public int StoragePeriod { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to write the archive log.
        /// </summary>
        public bool LogEnabled { get; set; }
    }
}
