﻿/*
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
 * Module   : ScadaWebCommon
 * Summary  : Represents an authorization handler that evaluates an object right requirement
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2023
 */

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Scada.Data.Entities;
using Scada.Web.Services;

namespace Scada.Web.Authorization
{
    /// <summary>
    /// Represents an authorization handler that evaluates an object right requirement.
    /// <para>Представляет обработчик авторизации, который оценивает выполнение требования прав на объект.</para>
    /// </summary>
    public class ObjRightHandler : AuthorizationHandler<ObjRightRequirement>
    {
        private readonly IWebContext webContext;
        private readonly IUserContext userContext;
        private readonly IHttpContextAccessor httpContextAccessor;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ObjRightHandler(IWebContext webContext, IUserContext userContext, 
            IHttpContextAccessor httpContextAccessor)
        {
            this.webContext = webContext ?? throw new ArgumentNullException(nameof(webContext));
            this.userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }


        /// <summary>
        /// Checks channel access rights.
        /// </summary>
        private bool CheckChannelAccess(string cnlNumsStr)
        {
            if (!string.IsNullOrEmpty(cnlNumsStr))
            {
                IList<int> cnlNums = ScadaUtils.ParseRange(cnlNumsStr, true, true);

                foreach (int cnlNum in cnlNums)
                {
                    Cnl cnl = webContext.ConfigDatabase.CnlTable.GetItem(cnlNum);

                    if (cnl == null)
                        return false; // no rights on undefined channel
                    else if (!userContext.Rights.GetRightByObj(cnl.ObjNum).View)
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks view access rights.
        /// </summary>
        private bool CheckViewAccess(string viewIdStr)
        {
            return int.TryParse(viewIdStr, out int viewID) &&
                webContext.ConfigDatabase.ViewTable.GetItem(viewID) is View view &&
                userContext.Rights.GetRightByObj(view.ObjNum).View;
        }

        /// <summary>
        /// Checks object access rights.
        /// </summary>
        private bool CheckObjectAccess(string objNumStr)
        {
            return int.TryParse(objNumStr, out int objNum) &&
                userContext.Rights.GetRightByObj(objNum).View;
        }

        /// <summary>
        /// Makes a decision if authorization is allowed based on a specific requirement.
        /// </summary>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ObjRightRequirement requirement)
        {
            bool accessAllowed = false;

            if (userContext.Rights.ViewAll)
            {
                accessAllowed = true;
            }
            else if (httpContextAccessor.HttpContext != null)
            {
                IQueryCollection query = httpContextAccessor.HttpContext.Request.Query;
                bool cnlNumsOK = !query.ContainsKey("cnlNums") || CheckChannelAccess(query["cnlNums"]);
                bool viewIdOK = !query.ContainsKey("viewID") || CheckViewAccess(query["viewID"]);
                bool objNumOK = !query.ContainsKey("objNum") || CheckObjectAccess(query["objNum"]);

                if (cnlNumsOK && viewIdOK && objNumOK)
                    accessAllowed = true;
            }

            if (accessAllowed)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
