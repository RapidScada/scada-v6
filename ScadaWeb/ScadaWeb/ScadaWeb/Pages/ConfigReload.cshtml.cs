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
 * Module   : Webstation Application
 * Summary  : Represents a page for reloading application configuration
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Microsoft.AspNetCore.Mvc.RazorPages;
using Scada.Data.Const;
using Scada.Lang;
using Scada.Web.Services;
using System.IO;

namespace Scada.Web.Pages
{
    /// <summary>
    /// Represents a page for reloading application configuration.
    /// <para>Представляет страницу для перезагрузки конфигурации приложения.</para>
    /// </summary>
    public class ConfigReloadModel : PageModel
    {
        private const string ConfirmationFileName = "webreload";

        private readonly IWebContext webContext;
        private readonly IUserContext userContext;

        public ConfigReloadModel(IWebContext webContext, IUserContext userContext)
        {
            this.webContext = webContext;
            this.userContext = userContext;
        }


        public string Message { get; set; }


        private void ReloadConfig(string target)
        {
            target ??= "all";

            if (target == "all")
            {
                Message = webContext.ReloadConfig()
                    ? "Configuration reload has started."
                    : "Unable to reload configuration.";
            }
            else if (target == "cache")
            {
                webContext.ResetCache();
                Message = "Cache has been reset.";
            }
        }

        public void OnGet(string target = null)
        {
            if (userContext.UserEntity.RoleID == RoleID.Administrator)
            {
                webContext.Log.WriteAction(Locale.IsRussian ?
                    "Перезагрузка конфигурации запрошена {0}, IP {1}" :
                    "Configuration reload is requested by {0}, IP {1}",
                    User.GetUsername(), HttpContext.Connection.RemoteIpAddress);
                ReloadConfig(target);
            }
            else if (HttpContext.Connection.IsLocal())
            {
                if (System.IO.File.Exists(Path.Combine(webContext.AppDirs.CmdDir, ConfirmationFileName)))
                {
                    webContext.Log.WriteAction(Locale.IsRussian ?
                        "Перезагрузка конфигурации запрошена локально" :
                        "Configuration reload is requested locally");
                    ReloadConfig(target);
                }
                else
                {
                    webContext.Log.WriteError(Locale.IsRussian ?
                        "Перезагрузка конфигурации отклонена" :
                        "Configuration reload rejected");
                    Message = "Confirmation file does not exist.";
                }
            }
            else
            {
                Message = "Access denied.";
            }
        }
    }
}
