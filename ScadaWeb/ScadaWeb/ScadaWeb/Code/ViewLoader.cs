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
 * Summary  : Loads views by a current user
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Microsoft.Extensions.Caching.Memory;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Web.Plugins;
using Scada.Web.Services;
using System;
using System.IO;

namespace Scada.Web.Code
{
    /// <summary>
    /// Loads views by a current user.
    /// <para>Загружает представления текущим пользователем.</para>
    /// </summary>
    internal class ViewLoader
    {
        private readonly IWebContext webContext;
        private readonly IUserContext userContext;
        private readonly IMemoryCache memoryCache;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ViewLoader(IWebContext webContext, IUserContext userContext, IMemoryCache memoryCache)
        {
            this.webContext = webContext ?? throw new ArgumentNullException(nameof(webContext));
            this.userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            this.memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }


        /// <summary>
        /// Checks if the specified view exists and that the current user is permitted to access it.
        /// </summary>
        private bool ValidateView(int viewID, out View viewEntity, out string errMsg)
        {
            if (viewID <= 0)
            {
                viewEntity = null;
                errMsg = "dict.ViewNotSpecified";
                return false;
            }

            // find view
            viewEntity = webContext.BaseDataSet.ViewTable.GetItem(viewID);

            if (viewEntity == null)
            {
                errMsg = "dict.ViewNotExists";
                return false;
            }

            // check access rights
            if (!userContext.Rights.GetRightByObj(viewEntity.ObjNum ?? 0).View)
            {
                errMsg = "dict.InsufficientViewRights";
                return false;
            }

            errMsg = "";
            return true;
        }

        /// <summary>
        /// Gets the view specification according to the entity properties.
        /// </summary>
        private ViewSpec GetViewSpec(View viewEntity)
        {
            if (viewEntity.ViewTypeID == null)
            {
                return webContext.PluginHolder.GetViewSpecByExt(Path.GetExtension(viewEntity.Path));
            }
            else
            {
                ViewType viewType = webContext.BaseDataSet.ViewTypeTable.GetItem(viewEntity.ViewTypeID.Value);
                return viewType == null ? null : webContext.PluginHolder.GetViewSpecByCode(viewType.Code);
            }
        }


        /// <summary>
        /// Gets the view specification.
        /// </summary>
        public bool GetViewSpec(int viewID, out ViewSpec viewSpec, out string errMsg)
        {
            if (!ValidateView(viewID, out View viewEntity, out errMsg))
            {
                viewSpec = null;
                return false;
            }

            viewSpec = memoryCache.GetOrCreate(WebUtils.GetViewSpecCacheKey(viewID), entry =>
            {
                entry.SetDefaultOptions(webContext);
                return GetViewSpec(viewEntity);
            });

            if (viewSpec == null)
            {
                errMsg = "dict.UnableResolveSpec";
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Gets a view from the server or cache.
        /// </summary>
        public bool GetView<T>(int viewID, out T view, out string errMsg) where T : BaseView
        {
            view = null;
            errMsg = "";
            return false;
        }

        /// <summary>
        /// Gets a view from the cache.
        /// </summary>
        public bool GetViewFromCache(int viewID, out BaseView view, out string errMsg)
        {
            view = null;
            errMsg = "";
            return false;
        }
    }
}
