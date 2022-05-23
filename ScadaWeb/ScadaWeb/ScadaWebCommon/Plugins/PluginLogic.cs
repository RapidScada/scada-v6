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
 * Module   : ScadaWebCommon
 * Summary  : Represents the base class for plugin logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Log;
using Scada.Web.Services;
using Scada.Web.TreeView;
using Scada.Web.Users;
using System;
using System.Collections.Generic;

namespace Scada.Web.Plugins
{
    /// <summary>
    /// Represents the base class for plugin logic.
    /// <para>Представляет базовый класс логики плагина.</para>
    /// </summary>
    public abstract class PluginLogic
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PluginLogic(IWebContext webContext)
        {
            WebContext = webContext ?? throw new ArgumentNullException(nameof(webContext));
            AppDirs = webContext.AppDirs;
            Log = webContext.Log;
        }


        /// <summary>
        /// Gets the web application context.
        /// </summary>
        protected IWebContext WebContext { get; }

        /// <summary>
        /// Gets the application directories.
        /// </summary>
        protected WebDirs AppDirs { get; }

        /// <summary>
        /// Gets or sets the plugin log.
        /// </summary>
        protected ILog Log { get; set; }

        /// <summary>
        /// Gets the plugin code.
        /// </summary>
        public abstract string Code { get; }

        /// <summary>
        /// Gets the plugin features.
        /// </summary>
        public virtual PluginFeatures Features => null;

        /// <summary>
        /// Gets the view specifications.
        /// </summary>
        public virtual ICollection<ViewSpec> ViewSpecs => null;

        /// <summary>
        /// Gets the data window specifications.
        /// </summary>
        public virtual ICollection<DataWindowSpec> DataWindowSpecs => null;

        /// <summary>
        /// Gets the script URLs to add to the main page.
        /// </summary>
        public virtual ICollection<string> ScriptUrls => null;

        /// <summary>
        /// Gets the URLs of the CSS stylesheets to add to the main page.
        /// </summary>
        public virtual ICollection<string> StyleUrls => null;


        /// <summary>
        /// Loads language dictionaries.
        /// </summary>
        public virtual void LoadDictionaries()
        {
        }

        /// <summary>
        /// Loads configuration.
        /// </summary>
        public virtual void LoadConfig()
        {
        }
        
        /// <summary>
        /// Resets the plugin cache.
        /// </summary>
        public virtual void ResetCache()
        {
        }

        /// <summary>
        /// Adds request processing filters.
        /// </summary>
        public virtual void AddFilters(FilterCollection filters)
        {
        }

        /// <summary>
        /// Adds services to the DI container.
        /// </summary>
        public virtual void AddServices(IServiceCollection services)
        {
        }

        /// <summary>
        /// Performs actions when the application is ready for operating.
        /// </summary>
        public virtual void OnAppReady()
        {
        }

        /// <summary>
        /// Performs actions after a user logs in, whether successful or not.
        /// </summary>
        public virtual void OnUserLogin(UserLoginArgs userLoginArgs)
        {
        }

        /// <summary>
        /// Performs actions after a user logs out.
        /// </summary>
        public virtual void OnUserLogout(UserLoginArgs userLoginArgs)
        {
        }

        /// <summary>
        /// Gets menu items available for the specified user.
        /// </summary>
        public virtual List<MenuItem> GetUserMenuItems(User user, UserRights userRights)
        {
            return null;
        }

        /// <summary>
        /// Prepares the specified view provided by the plugin.
        /// </summary>
        public virtual void PrepareView(ViewBase view)
        {
        }
    }
}
