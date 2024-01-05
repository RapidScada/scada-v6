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
 * Module   : Webstation Application
 * Summary  : Loads views available to the current user
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2023
 */

using Microsoft.Extensions.Caching.Memory;
using Scada.Data.Const;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Lang;
using Scada.Protocol;
using Scada.Storages;
using Scada.Web.Audit;
using Scada.Web.Lang;
using Scada.Web.Plugins;
using Scada.Web.Services;
using System;
using System.IO;

namespace Scada.Web.Code
{
    /// <summary>
    /// Loads views available to the current user.
    /// <para>Загружает представления, доступные текущему пользователю.</para>
    /// </summary>
    internal class ViewLoader : IViewLoader
    {
        private readonly IWebContext webContext;
        private readonly IUserContext userContext;
        private readonly IAuditLog auditLog;
        private readonly IClientAccessor clientAccessor;
        private readonly IMemoryCache memoryCache;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ViewLoader(IWebContext webContext, IUserContext userContext, IAuditLog auditLog,
            IClientAccessor clientAccessor, IMemoryCache memoryCache)
        {
            this.webContext = webContext ?? throw new ArgumentNullException(nameof(webContext));
            this.userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            this.auditLog = auditLog ?? throw new ArgumentNullException(nameof(auditLog));
            this.clientAccessor = clientAccessor ?? throw new ArgumentNullException(nameof(clientAccessor));
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
                errMsg = WebPhrases.ViewNotSpecified;
                return false;
            }

            // find view
            viewEntity = webContext.ConfigDatabase.ViewTable.GetItem(viewID);

            if (viewEntity == null)
            {
                errMsg = WebPhrases.ViewNotExists;
                return false;
            }

            // check access rights
            if (!userContext.Rights.GetRightByView(viewEntity).View)
            {
                errMsg = WebPhrases.InsufficientViewRights;
                return false;
            }

            errMsg = "";
            return true;
        }

        /// <summary>
        /// Gets a specification of the specified view entity.
        /// </summary>
        private bool GetViewSpec(View viewEntity, out ViewSpec viewSpec, out string errMsg)
        {
            viewSpec = memoryCache.GetOrCreate(WebUtils.GetViewSpecCacheKey(viewEntity.ViewID), entry =>
            {
                entry.SetDefaultOptions(webContext);
                return webContext.GetViewSpec(viewEntity);
            });

            if (viewSpec == null)
            {
                errMsg = WebPhrases.UnableResolveViewSpec;
                return false;
            }
            else
            {
                errMsg = "";
                return true;
            }
        }

        /// <summary>
        /// Creates and loads the specified view.
        /// </summary>
        private bool GetView(View viewEntity, Type viewType, out ViewBase view, out string errMsg)
        {
            try
            {
                // get exact view type if needed
                if (viewType == typeof(ViewBase))
                {
                    if (GetViewSpec(viewEntity, out ViewSpec viewSpec, out errMsg))
                    {
                        viewType = viewSpec.ViewType;
                    }
                    else
                    {
                        view = null;
                        return false;
                    }
                }

                // create view
                view = (ViewBase)Activator.CreateInstance(viewType, viewEntity);

                if (webContext.PluginHolder.GetPluginByViewType(viewType, out PluginLogic pluginLogic))
                    pluginLogic.PrepareView(view);

                // load view
                if (view.StoredOnServer)
                {
                    if (webContext.Storage.ViewAvailable)
                        LoadViewFromStorage(view, viewEntity.Path);
                    else
                        LoadViewFromServer(view, viewEntity.Path);
                }

                view.Build();
                view.Bind(webContext.ConfigDatabase);
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                webContext.Log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при загрузке представления с ид. {0} по пути {1}" :
                    "Error loading view with ID {0} by the path {1}", 
                    viewEntity.ViewID, viewEntity.Path);

                view = null;
                errMsg = WebPhrases.UnableLoadView;
                return false;
            }
        }

        /// <summary>
        /// Loads the specified view from the application storage.
        /// </summary>
        private void LoadViewFromStorage(ViewBase view, string path)
        {
            // load view
            using (BinaryReader reader = webContext.Storage.OpenBinary(DataCategory.View, path))
            {
                view.LoadView(reader.BaseStream);
            }

            // load resources
            if (view.Resources != null)
            {
                foreach (ViewResource resource in view.Resources)
                {
                    if (resource != null)
                    {
                        using BinaryReader reader = webContext.Storage.OpenBinary(DataCategory.View, resource.Path);
                        view.LoadResource(resource, reader.BaseStream);
                    }
                }
            }
        }

        /// <summary>
        /// Loads the specified view from the server.
        /// </summary>
        private void LoadViewFromServer(ViewBase view, string path)
        {
            // load view
            using (MemoryStream memoryStream = new())
            {
                RelativePath relativePath = new(TopFolder.View, AppFolder.Root, path);
                clientAccessor.ScadaClient.DownloadFile(relativePath, memoryStream, true);
                memoryStream.Position = 0;
                view.LoadView(memoryStream);
            }

            // load resources
            if (view.Resources != null)
            {
                foreach (ViewResource resource in view.Resources)
                {
                    if (resource != null)
                    {
                        using MemoryStream memoryStream = new();
                        RelativePath relativePath = new(TopFolder.View, AppFolder.Root, resource.Path);
                        clientAccessor.ScadaClient.DownloadFile(relativePath, memoryStream, true);
                        memoryStream.Position = 0;
                        view.LoadResource(resource, memoryStream);
                    }
                }
            }
        }

        /// <summary>
        /// Writes the entry to the audit log.
        /// </summary>
        private void WriteToAuditLog(int viewID, bool success, string message)
        {
            auditLog.Write(new AuditLogEntry(userContext.UserEntity)
            {
                ActionType = AuditActionType.OpenView,
                ActionArgs = AuditActionArgs.FromObject(new { ViewID = viewID }),
                ActionResult = AuditActionResult.FromBool(success),
                Severity = success ? Severity.Info : Severity.Minor,
                Message = message
            });
        }


        /// <summary>
        /// Gets a specification of the specified view.
        /// </summary>
        public bool GetViewSpec(int viewID, out ViewSpec viewSpec, out string errMsg)
        {
            if (!ValidateView(viewID, out View viewEntity, out errMsg))
            {
                viewSpec = null;
                return false;
            }

            return GetViewSpec(viewEntity, out viewSpec, out errMsg);
        }

        /// <summary>
        /// Gets a view from the server or cache.
        /// </summary>
        public bool GetView<T>(int viewID, bool enableAudit, out T view, out string errMsg) where T : ViewBase
        {
            bool success = false;
            string message = null;

            if (ValidateView(viewID, out View viewEntity, out string msg1))
            {
                view = (T)memoryCache.GetOrCreate(WebUtils.GetViewCacheKey(viewID), entry =>
                {
                    entry.AddExpirationToken(webContext);

                    if (GetView(viewEntity, typeof(T), out ViewBase createdView, out string msg2))
                    {
                        entry.SetSlidingExpiration(WebUtils.ViewCacheExpiration);
                        return createdView;
                    }
                    else
                    {
                        entry.SetSlidingExpiration(WebUtils.ErrorCacheExpiration);
                        message = msg2;
                        return null;
                    }
                });

                if (view == null)
                    message ??= WebPhrases.UnableLoadView;
                else
                    success = true;
            }
            else
            {
                view = null;
                message = msg1;
            }

            if (enableAudit)
                WriteToAuditLog(viewID, success, message);

            errMsg = message;
            return success;
        }

        /// <summary>
        /// Gets a view from the cache.
        /// </summary>
        public bool GetViewFromCache(int viewID, out ViewBase view, out string errMsg)
        {
            if (!ValidateView(viewID, out _, out errMsg))
            {
                view = null;
                return false;
            }

            if (!memoryCache.TryGetValue(WebUtils.GetViewCacheKey(viewID), out view))
            {
                errMsg = WebPhrases.ViewMissingFromCache;
                return false;
            }
            else if (view == null)
            {
                errMsg = WebPhrases.ViewUndefined;
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
