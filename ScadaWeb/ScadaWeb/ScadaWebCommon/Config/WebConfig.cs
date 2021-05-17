/*
 * Copyright 2021 Rapid Software LLC
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
 * Module   : ScadaWebCommon
 * Summary  : The phrases used by the web application and its plugins
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Scada.Client;
using Scada.Config;
using System.Collections.Generic;

namespace Scada.Web.Config
{
    /// <summary>
    /// Represents a web application configuration.
    /// <para>Представляет конфигурацию веб-приложения.</para>
    /// </summary>
    public class WebConfig
    {
        /// <summary>
        /// The default configuration file name.
        /// </summary>
        public const string DefaultFileName = "ScadaWebConfig.xml";


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public WebConfig()
        {
            SetToDefault();
        }


        /// <summary>
        /// Gets the general options.
        /// </summary>
        public GeneralOptions GeneralOptions { get; private set; }

        /// <summary>
        /// Gets the connection options.
        /// </summary>
        public ConnectionOptions ConnectionOptions { get; private set; }

        /// <summary>
        /// Gets the codes of the enabled plugins.
        /// </summary>
        public List<string> PluginCodes { get; private set; }

        /// <summary>
        /// Gets the groups of custom options.
        /// </summary>
        public SortedList<string, OptionList> CustomOptions { get; private set; }



        /// <summary>
        /// Sets the default values.
        /// </summary>
        private void SetToDefault()
        {
            GeneralOptions = new GeneralOptions();
            ConnectionOptions = new ConnectionOptions();
            PluginCodes = new List<string>();
            CustomOptions = new SortedList<string, OptionList>();
        }
    }
}
