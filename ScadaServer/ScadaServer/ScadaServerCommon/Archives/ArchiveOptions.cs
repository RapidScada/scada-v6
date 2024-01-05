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
 * Module   : ScadaServerCommon
 * Summary  : Represents archive options
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2022
 * Modified : 2022
 */

using Scada.Config;
using System;

namespace Scada.Server.Archives
{
    /// <summary>
    /// Represents archive options.
    /// <para>Представляет параметры архива.</para>
    /// </summary>
    public abstract class ArchiveOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ArchiveOptions(OptionList options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            ReadOnly = options.GetValueAsBool("ReadOnly");
            LogEnabled = options.GetValueAsBool("LogEnabled");
        }


        /// <summary>
        /// Gets or sets a value indicating whether the archive is read only.
        /// </summary>
        public bool ReadOnly { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to write the archive log.
        /// </summary>
        public bool LogEnabled { get; set; }
        
        
        /// <summary>
        /// Adds the options to the list.
        /// </summary>
        public virtual void AddToOptionList(OptionList options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            options.Clear();
            options["ReadOnly"] = ReadOnly.ToLowerString();
            options["LogEnabled"] = LogEnabled.ToLowerString();
        }
    }
}
