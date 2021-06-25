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
 * Created  : 2016
 * Modified : 2021
 */

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

using Scada.Lang;

namespace Scada.Web.Lang
{
    /// <summary>
    /// The phrases used by the web application and its plugins.
    /// <para>Фразы, используемые веб-приложением и его плагинами.</para>
    /// </summary>
    public static class WebPhrases
    {
        // Webstation Application
        public static string ErrorInPlugin { get; private set; }

        // Scada.Web
        public static string CorrectErrors { get; private set; }
        public static string ClientError { get; private set; }
        public static string UnknownUsername { get; private set; }

        // Scada.Web.Code.ViewLoader
        public static string ViewNotSpecified { get; private set; }
        public static string ViewNotExists { get; private set; }
        public static string InsufficientViewRights { get; private set; }
        public static string UnableResolveViewSpec { get; private set; }
        public static string UnableLoadView { get; private set; }
        public static string ViewMissingFromCache { get; private set; }

        // Scada.Web.TreeView.MenuItem
        public static string ReportsMenuItem { get; private set; }
        public static string AdministrationMenuItem { get; private set; }
        public static string ConfigurationMenuItem { get; private set; }
        public static string RegistrationMenuItem { get; private set; }
        public static string PluginsMenuItem { get; private set; }
        public static string AboutMenuItem { get; private set; }

        public static void Init()
        {
            // set phrases that are used in the bilingual service logic, depending on the locale
            if (Locale.IsRussian)
            {
                ErrorInPlugin = "Ошибка при вызове метода {0} плагина {1}";
            }
            else
            {
                ErrorInPlugin = "Error calling the {0} method of the {1} plugin";
            }

            // load phrases that are used in the multilingual user interface from dictionaries
            LocaleDict dict = Locale.GetDictionary("Scada.Web");
            CorrectErrors = dict["CorrectErrors"];
            ClientError = dict["ClientError"];
            UnknownUsername = dict["UnknownUsername"];

            dict = Locale.GetDictionary("Scada.Web.Code.ViewLoader");
            ViewNotSpecified = dict["ViewNotSpecified"];
            ViewNotExists = dict["ViewNotExists"];
            InsufficientViewRights = dict["InsufficientViewRights"];
            UnableResolveViewSpec = dict["UnableResolveViewSpec"];
            UnableLoadView = dict["UnableLoadView"];
            ViewMissingFromCache = dict["ViewMissingFromCache"];

            dict = Locale.GetDictionary("Scada.Web.TreeView.MenuItem");
            ReportsMenuItem = dict["ReportsMenuItem"];
            AdministrationMenuItem = dict["AdministrationMenuItem"];
            ConfigurationMenuItem = dict["ConfigurationMenuItem"];
            RegistrationMenuItem = dict["RegistrationMenuItem"];
            PluginsMenuItem = dict["PluginsMenuItem"];
            AboutMenuItem = dict["AboutMenuItem"];
        }
    }
}
