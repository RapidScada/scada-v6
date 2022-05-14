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
 * Summary  : The class provides helper methods for the Webstation application and its plugins
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2021
 */

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scada.Lang;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Web;

namespace Scada.Web
{
    /// <summary>
    /// The class provides helper methods for the Webstation application and its plugins.
    /// <para>Класс, предоставляющий вспомогательные методы для приложения Вебстанция и его плагинов.</para>
    /// </summary>
    public static partial class WebUtils
    {
        /// <summary>
        /// The application version.
        /// </summary>
        public const string AppVersion = "6.0.0.0";
        /// <summary>
        /// The application log file name.
        /// </summary>
        public const string LogFileName = "ScadaWeb.log";
        /// <summary>
        /// Formats value for a date input field.
        /// </summary>
        public const string InputDateFormat = "yyyy-MM-dd";
        /// <summary>
        /// Formats date and time to use in JavaScript.
        /// </summary>
        public const string JsDateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffK";
        /// <summary>
        /// Specifies how objects are converted to JSON.
        /// </summary>
        private static readonly JsonSerializerOptions JsonOptions = 
            new () { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };


        /// <summary>
        /// Checks if the user has been authenticated.
        /// </summary>
        public static bool IsAuthenticated(this ClaimsPrincipal user)
        {
            return user.Identity != null && user.Identity.IsAuthenticated;
        }

        /// <summary>
        /// Gets the username from the specified principal.
        /// </summary>
        public static string GetUsername(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.Name) ?? "";
        }

        /// <summary>
        /// Gets the user ID from the specified principal.
        /// </summary>
        public static bool GetUserID(this ClaimsPrincipal user, out int userID)
        {
            if (user.FindFirstValue(ClaimTypes.NameIdentifier) is string userIdStr &&
                int.TryParse(userIdStr, out userID))
            {
                return true;
            }
            else
            {
                userID = 0;
                return false;
            }
        }

        /// <summary>
        /// Gets the user ID from the specified principal.
        /// </summary>
        public static int GetUserID(this ClaimsPrincipal user)
        {
            GetUserID(user, out int userID);
            return userID;
        }

        /// <summary>
        /// Gets the role ID from the specified principal.
        /// </summary>
        public static int GetRoleID(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.Role) is string roleIdStr && int.TryParse(roleIdStr, out int roleID)
                ? roleID
                : 0;
        }

        /// <summary>
        /// Converts the phrases dictionary to a JavaScript object.
        /// </summary>
        public static HtmlString DictionaryToJs(LocaleDict dict)
        {
            StringBuilder sbJs = new();
            sbJs.AppendLine("{");

            if (dict != null)
            {
                foreach (KeyValuePair<string, string> pair in dict.Phrases)
                {
                    sbJs.Append(pair.Key)
                        .Append(": '")
                        .Append(HttpUtility.JavaScriptStringEncode(pair.Value))
                        .AppendLine("',");
                }
            }

            sbJs.Append('}');
            return new HtmlString(sbJs.ToString());
        }

        /// <summary>
        /// Converts the phrases dictionary to a JavaScript object.
        /// </summary>
        public static HtmlString DictionaryToJs(string dictKey)
        {
            return DictionaryToJs(Locale.GetDictionary(dictKey));
        }

        /// <summary>
        /// Gets a JavaScript object that contains information about the environment.
        /// </summary>
        public static HtmlString GetEnvironmentJs(IUrlHelper urlHelper)
        {
            return new HtmlString(new StringBuilder()
                .AppendLine("{")
                .AppendLine("isStub: false,")
                .AppendLine($"rootPath: '{urlHelper.Content("~/")}',")
                .AppendLine($"locale: '{HttpUtility.JavaScriptStringEncode(Locale.Culture.Name)}',")
                .AppendLine($"productName: '{HttpUtility.JavaScriptStringEncode(CommonPhrases.ProductName)}'")
                .Append('}')
                .ToString());
        }

        /// <summary>
        /// Converts the specified object to a JavaScript object.
        /// </summary>
        public static HtmlString ToJs(this object obj)
        {
            return obj == null
                ? new HtmlString("null")
                : new HtmlString(string.Format("JSON.parse('{0}')", 
                    HttpUtility.JavaScriptStringEncode(JsonSerializer.Serialize(obj, JsonOptions))));
        }

        /// <summary>
        /// Determines whether the specified connection is local.
        /// </summary>
        public static bool IsLocal(this ConnectionInfo connection)
        {
            if (connection.RemoteIpAddress != null)
            {
                return connection.LocalIpAddress == null
                    ? IPAddress.IsLoopback(connection.RemoteIpAddress)
                    : connection.RemoteIpAddress.Equals(connection.LocalIpAddress);
            }

            return false;
        }
    }
}
