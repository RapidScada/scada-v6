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
 * Summary  : Defines functionality to access web application level data
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2022
 */

using Scada.Client;
using Scada.Config;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Log;
using Scada.Storages;
using Scada.Web.Config;
using Scada.Web.Plugins;
using System.Threading;

namespace Scada.Web.Services
{
    /// <summary>
    /// Defines functionality to access web application level data.
    /// <para>Определяет функциональность для доступа данным уровня веб-приложения.</para>
    /// </summary>
    public interface IWebContext
    {
        /// <summary>
        /// Gets a value indicating whether the application is ready for operating.
        /// </summary>
        bool IsReady { get; }

        /// <summary>
        /// Gets a value indicating whether a user can login.
        /// </summary>
        bool IsReadyToLogin { get; }

        /// <summary>
        /// Gets the instance configuration.
        /// </summary>
        InstanceConfig InstanceConfig { get; }

        /// <summary>
        /// Gets the application configuration.
        /// </summary>
        WebConfig AppConfig { get; }

        /// <summary>
        /// Gets the application directories.
        /// </summary>
        WebDirs AppDirs { get; }

        /// <summary>
        /// Gets the application storage.
        /// </summary>
        IStorage Storage { get; }

        /// <summary>
        /// Gets the application log.
        /// </summary>
        ILog Log { get; }

        /// <summary>
        /// Gets the cached configuration database.
        /// </summary>
        ConfigDatabase ConfigDatabase { get; }

        /// <summary>
        /// Gets the client pool.
        /// </summary>
        ScadaClientPool ClientPool { get; }

        /// <summary>
        /// Gets the object containing plugins.
        /// </summary>
        PluginHolder PluginHolder { get; }

        /// <summary>
        /// Gets the source object that can send expiration notification to the memory cache.
        /// </summary>
        CancellationTokenSource CacheExpirationTokenSource { get; }

        /// <summary>
        /// Gets the statistics ID.
        /// </summary>
        string StatsID { get; }


        /// <summary>
        /// Gets a specification of the specified view entity.
        /// </summary>
        ViewSpec GetViewSpec(View viewEntity);

        /// <summary>
        /// Reloads the application configuration and resets the memory cache.
        /// </summary>
        bool ReloadConfig();
        
        /// <summary>
        /// Resets the memory cache.
        /// </summary>
        void ResetCache();
    }
}
