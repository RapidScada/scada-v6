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
 * Summary  : Holds active plugins and helps to call their methods
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Scada.Log;
using Scada.Web.Lang;
using Scada.Web.TreeView;
using Scada.Web.Users;
using System;
using System.Collections.Generic;

namespace Scada.Web.Plugins
{
    /// <summary>
    /// Holds active plugins and helps to call their methods.
    /// <para>Содержит активные плагины и помогает вызывать их методы.</para>
    /// </summary>
    public class PluginHolder
    {
        private readonly List<PluginLogic> plugins; // the plugins used
        private readonly Dictionary<string, PluginLogic> pluginMap; // the plugins accessed by code
        private readonly object pluginLock;         // synchronizes access to the modules
        private ILog log;                           // the application log


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PluginHolder()
        {
            plugins = new List<PluginLogic>();
            pluginMap = new Dictionary<string, PluginLogic>();
            pluginLock = new object();
            log = LogStub.Instance;
        }


        /// <summary>
        /// Gets or sets the application log.
        /// </summary>
        public ILog Log
        {
            get
            {
                return log;
            }
            set
            {
                log = value ?? throw new ArgumentNullException(nameof(value));
            }
        }


        /// <summary>
        /// Adds the specified plugin to the lists.
        /// </summary>
        public void AddPlugin(PluginLogic pluginLogic)
        {
            if (pluginLogic == null)
                throw new ArgumentNullException(nameof(pluginLogic));

            if (pluginMap.ContainsKey(pluginLogic.Code))
                throw new ScadaException("Plugin already exists.");

            plugins.Add(pluginLogic);
            pluginMap.Add(pluginLogic.Code, pluginLogic);
        }

        /// <summary>
        /// Returns an enumerable collection of the active plugins.
        /// </summary>
        public IEnumerable<PluginLogic> EnumeratePlugins()
        {
            foreach (PluginLogic pluginLogic in plugins)
            {
                yield return pluginLogic;
            }
        }

        /// <summary>
        /// Calls the OnServiceStart method of the plugins.
        /// </summary>
        public void OnServiceStart()
        {
            foreach (PluginLogic pluginLogic in plugins)
            {
                try
                {
                    pluginLogic.OnServiceStart();
                }
                catch (Exception ex)
                {
                    log.WriteException(ex, WebPhrases.ErrorInPlugin, nameof(OnServiceStart), pluginLogic.Code);
                }
            }
        }

        /// <summary>
        /// Calls the OnServiceStop method of the plugins.
        /// </summary>
        public void OnServiceStop()
        {
            foreach (PluginLogic pluginLogic in plugins)
            {
                try
                {
                    pluginLogic.OnServiceStop();
                }
                catch (Exception ex)
                {
                    log.WriteException(ex, WebPhrases.ErrorInPlugin, nameof(OnServiceStop), pluginLogic.Code);
                }
            }
        }

        /// <summary>
        /// Calls the AddFilters method of the plugins.
        /// </summary>
        public void AddFilters(FilterCollection filters)
        {
            foreach (PluginLogic pluginLogic in plugins)
            {
                try
                {
                    pluginLogic.AddFilters(filters);
                }
                catch (Exception ex)
                {
                    log.WriteException(ex, WebPhrases.ErrorInPlugin, nameof(AddFilters), pluginLogic.Code);
                }
            }
        }

        /// <summary>
        /// Calls the AddServices method of the plugins.
        /// </summary>
        public void AddServices(IServiceCollection services)
        {
            foreach (PluginLogic pluginLogic in plugins)
            {
                try
                {
                    pluginLogic.AddServices(services);
                }
                catch (Exception ex)
                {
                    log.WriteException(ex, WebPhrases.ErrorInPlugin, nameof(AddServices), pluginLogic.Code);
                }
            }
        }

        /// <summary>
        /// Calls the OnUserLogin method of the plugins.
        /// </summary>
        public void OnUserLogin(int userID)
        {
            foreach (PluginLogic pluginLogic in plugins)
            {
                try
                {
                    pluginLogic.OnUserLogin(userID);
                }
                catch (Exception ex)
                {
                    log.WriteException(ex, WebPhrases.ErrorInPlugin, nameof(OnUserLogin), pluginLogic.Code);
                }
            }
        }

        /// <summary>
        /// Calls the OnUserLogout method of the plugins.
        /// </summary>
        public void OnUserLogout(int userID)
        {
            foreach (PluginLogic pluginLogic in plugins)
            {
                try
                {
                    pluginLogic.OnUserLogout(userID);
                }
                catch (Exception ex)
                {
                    log.WriteException(ex, WebPhrases.ErrorInPlugin, nameof(OnUserLogout), pluginLogic.Code);
                }
            }
        }

        /// <summary>
        /// Calls the GetUserMenuItems method of the specified plugin.
        /// </summary>
        public List<MenuItem> GetUserMenuItems(PluginLogic pluginLogic, int userID, UserRights userRights)
        {
            if (pluginLogic == null)
                throw new ArgumentNullException(nameof(pluginLogic));

            try
            {
                return pluginLogic.GetUserMenuItems(userID, userRights);
            }
            catch (Exception ex)
            {
                log.WriteException(ex, WebPhrases.ErrorInPlugin, nameof(GetUserMenuItems), pluginLogic.Code);
                return null;
            }
        }
    }
}
