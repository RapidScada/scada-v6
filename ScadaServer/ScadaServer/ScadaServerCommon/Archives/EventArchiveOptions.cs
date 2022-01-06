/*
 * Copyright 2022 Rapid Software LLC
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
 * Summary  : Represents options of an event archive
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2022
 */

using Scada.Config;
using System;

namespace Scada.Server.Archives
{
    /// <summary>
    /// Represents options of an event archive.
    /// <para>Представляет параметры архива событий.</para>
    /// </summary>
    public abstract class EventArchiveOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EventArchiveOptions(OptionList options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            Retention = options.GetValueAsInt("Retention", 365);
            LogEnabled = options.GetValueAsBool("LogEnabled");
        }


        /// <summary>
        /// Gets or sets the duration of time that the archive retains data, days.
        /// </summary>
        public int Retention { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to write the archive log.
        /// </summary>
        public bool LogEnabled { get; set; }


        /// <summary>
        /// Adds the options to the list.
        /// </summary>
        public virtual void AddToOptionList(OptionList options)
        {
            options.Clear();
            options["Retention"] = Retention.ToString();
            options["LogEnabled"] = LogEnabled.ToLowerString();
        }
    }
}
