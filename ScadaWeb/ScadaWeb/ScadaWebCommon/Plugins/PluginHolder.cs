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
using Scada.Data.Entities;
using Scada.Log;
using Scada.Web.Config;
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
        private readonly Dictionary<string, PluginLogic> pluginMap;   // the plugins accessed by code
        private readonly Dictionary<string, ViewSpec> viewSpecByCode; // the view specifications accessed by code
        private readonly Dictionary<string, ViewSpec> viewSpecByExt;  // the view specifications accessed by extension
        private readonly object pluginLock;         // synchronizes access to the modules
        private ILog log;                           // the application log


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PluginHolder()
        {
            plugins = new List<PluginLogic>();
            pluginMap = new Dictionary<string, PluginLogic>();
            viewSpecByCode = new Dictionary<string, ViewSpec>();
            viewSpecByExt = new Dictionary<string, ViewSpec>();
            pluginLock = new object();
            log = LogStub.Instance;
            FeaturedPlugins = new FeaturedPlugins();
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
        /// Gets the featured plugins.
        /// </summary>
        public FeaturedPlugins FeaturedPlugins { get; }


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

            if (pluginLogic.ViewSpecs != null)
            {
                foreach (ViewSpec viewSpec in pluginLogic.ViewSpecs)
                {
                    if (!string.IsNullOrEmpty(viewSpec.TypeCode) && !viewSpecByCode.ContainsKey(viewSpec.TypeCode))
                        viewSpecByCode.Add(viewSpec.TypeCode, viewSpec);

                    if (!string.IsNullOrEmpty(viewSpec.FileExtension))
                    {
                        string ext = viewSpec.FileExtension.ToLowerInvariant();
                        if (!viewSpecByExt.ContainsKey(ext))
                            viewSpecByCode.Add(ext, viewSpec);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the plugin by code.
        /// </summary>
        public bool GetPlugin(string pluginCode, out PluginLogic pluginLogic)
        {
            if (string.IsNullOrEmpty(pluginCode))
            {
                pluginLogic = null;
                return false;
            }
            else
            {
                return pluginMap.TryGetValue(pluginCode, out pluginLogic);
            }
        }

        /// <summary>
        /// Gets the plugin by code.
        /// </summary>
        public PluginLogic GetPlugin(string pluginCode)
        {
            GetPlugin(pluginCode, out PluginLogic pluginLogic);
            return pluginLogic;
        }

        /// <summary>
        /// Finds and sets the featured plugins according to the specified configuration.
        /// </summary>
        public void DefineFeaturedPlugins(PluginAssignment pluginAssignment)
        {
            if (pluginAssignment == null)
                throw new ArgumentNullException(nameof(pluginAssignment));

            FeaturedPlugins.ChartPlugin = GetPlugin(pluginAssignment.ChartFeature);
            FeaturedPlugins.CommandPlugin = GetPlugin(pluginAssignment.CommandFeature);
            FeaturedPlugins.EventAckPlugin = GetPlugin(pluginAssignment.EventAckFeature);
            FeaturedPlugins.UserManagementPlugin = GetPlugin(pluginAssignment.UserManagementFeature);
            FeaturedPlugins.NotificationPlugin = GetPlugin(pluginAssignment.NotificationFeature);
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
        /// Gets the view specification by view type code.
        /// </summary>
        public ViewSpec GetViewSpecByCode(string viewTypeCode)
        {
            if (string.IsNullOrEmpty(viewTypeCode))
            {
                return null;
            }
            else
            {
                viewSpecByCode.TryGetValue(viewTypeCode, out ViewSpec viewSpec);
                return viewSpec;
            }
        }

        /// <summary>
        /// Gets the view specification by extenstion.
        /// </summary>
        public ViewSpec GetViewSpecByExt(string extenstion)
        {
            if (string.IsNullOrEmpty(extenstion))
            {
                return null;
            }
            else
            {
                viewSpecByExt.TryGetValue(extenstion.Trim('.').ToLowerInvariant(), out ViewSpec viewSpec);
                return viewSpec;
            }
        }

        /// <summary>
        /// Returns an enumerable collection of all script URLs.
        /// </summary>
        public IEnumerable<string> AllScriptUrls()
        {
            if (!string.IsNullOrEmpty(FeaturedPlugins.ChartPlugin?.Features?.ChartScriptUrl))
                yield return FeaturedPlugins.ChartPlugin.Features.ChartScriptUrl;

            if (!string.IsNullOrEmpty(FeaturedPlugins.CommandPlugin?.Features?.CommandScriptUrl))
                yield return FeaturedPlugins.CommandPlugin.Features.CommandScriptUrl;

            if (!string.IsNullOrEmpty(FeaturedPlugins.EventAckPlugin?.Features?.EventAckScriptUrl))
                yield return FeaturedPlugins.EventAckPlugin.Features.EventAckScriptUrl;

            if (!string.IsNullOrEmpty(FeaturedPlugins.NotificationPlugin?.Features?.NotificationScriptUrl))
                yield return FeaturedPlugins.NotificationPlugin.Features.NotificationScriptUrl;

            foreach (PluginLogic pluginLogic in plugins)
            {
                if (pluginLogic.ScriptUrls != null)
                {
                    foreach (string url in pluginLogic.ScriptUrls)
                    {
                        yield return url;
                    }
                }
            }
        }

        /// <summary>
        /// Returns an enumerable collection of all style URLs.
        /// </summary>
        public IEnumerable<string> AllStyleUrls()
        {
            foreach (PluginLogic pluginLogic in plugins)
            {
                if (pluginLogic.StyleUrls != null)
                {
                    foreach (string url in pluginLogic.StyleUrls)
                    {
                        yield return url;
                    }
                }
            }
        }

        /// <summary>
        /// Calls the AddFilters method of the plugins.
        /// </summary>
        public void AddFilters(FilterCollection filters)
        {
            lock (pluginLock)
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
        }

        /// <summary>
        /// Calls the AddServices method of the plugins.
        /// </summary>
        public void AddServices(IServiceCollection services)
        {
            lock (pluginLock)
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
        }

        /// <summary>
        /// Calls the LoadPluginConfig method of the plugins.
        /// </summary>
        public void LoadPluginConfig()
        {
            lock (pluginLock)
            {
                foreach (PluginLogic pluginLogic in plugins)
                {
                    try
                    {
                        pluginLogic.LoadPluginConfig();
                    }
                    catch (Exception ex)
                    {
                        log.WriteException(ex, WebPhrases.ErrorInPlugin, nameof(LoadPluginConfig), pluginLogic.Code);
                    }
                }
            }
        }

        /// <summary>
        /// Calls the OnUserLogin method of the plugins.
        /// </summary>
        public void OnUserLogin(int userID)
        {
            lock (pluginLock)
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
        }

        /// <summary>
        /// Calls the OnUserLogout method of the plugins.
        /// </summary>
        public void OnUserLogout(int userID)
        {
            lock (pluginLock)
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
        }

        /// <summary>
        /// Calls the GetUserMenuItems method of the specified plugin.
        /// </summary>
        public List<MenuItem> GetUserMenuItems(PluginLogic pluginLogic, User user, UserRights userRights)
        {
            if (pluginLogic == null)
                throw new ArgumentNullException(nameof(pluginLogic));

            lock (pluginLock)
            {
                try
                {
                    return pluginLogic.GetUserMenuItems(user, userRights);
                }
                catch (Exception ex)
                {
                    log.WriteException(ex, WebPhrases.ErrorInPlugin, nameof(GetUserMenuItems), pluginLogic.Code);
                    return null;
                }
            }
        }

        /// <summary>
        /// Calls the FindUser method of the user management plugin.
        /// </summary>
        public User FindUser(int userID)
        {
            if (FeaturedPlugins.UserManagementPlugin == null)
                return null;

            lock (pluginLock)
            {
                try
                {
                    return FeaturedPlugins.UserManagementPlugin.Features?.FindUser(userID);
                }
                catch (Exception ex)
                {
                    log.WriteException(ex, WebPhrases.ErrorInPlugin,
                        nameof(FindUser), FeaturedPlugins.UserManagementPlugin.Code);
                    return null;
                }
            }
        }

        /// <summary>
        /// Calls the GetUserConfig method of the user management plugin.
        /// </summary>
        public UserConfig GetUserConfig(int userID)
        {
            if (FeaturedPlugins.UserManagementPlugin == null)
                return null;

            lock (pluginLock)
            {
                try
                {
                    return FeaturedPlugins.UserManagementPlugin.Features?.GetUserConfig(userID);
                }
                catch (Exception ex)
                {
                    log.WriteException(ex, WebPhrases.ErrorInPlugin,
                        nameof(GetUserConfig), FeaturedPlugins.UserManagementPlugin.Code);
                    return null;
                }
            }
        }
    }
}
