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
 * Module   : ScadaServerCommon
 * Summary  : Represents options of a current data archive
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using System;
using Scada.Config;

namespace Scada.Server.Archives
{
    /// <summary>
    /// Represents options of a current data archive.
    /// <para>Представляет параметры архива текущих данных.</para>
    /// </summary>
    public abstract class CurrentArchiveOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CurrentArchiveOptions(CustomOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            WritingPeriod = options.GetValueAsInt("WritingPeriod", 10);
            LogEnabled = options.GetValueAsBool("LogEnabled");
        }


        /// <summary>
        /// Gets or sets the period of writing data to a file, sec.
        /// </summary>
        public int WritingPeriod { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to write the archive log.
        /// </summary>
        public bool LogEnabled { get; set; }
    }
}
