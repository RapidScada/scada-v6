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
 * Summary  : Represents an authorization handler that evaluates an object right requirement
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Scada.Data.Entities;
using Scada.Web.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        /// Makes a decision if authorization is allowed based on a specific requirement.
        /// </summary>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ObjRightRequirement requirement)
        {
            bool accessAllowed = false;

            if (userContext.Rights.ViewAll)
            {
                accessAllowed = true;
            }
            else if (httpContextAccessor.HttpContext is HttpContext httpContext)
            {
                accessAllowed = true;
                string cnlNumsStr = httpContext.Request.Query["cnlNums"];

                if (!string.IsNullOrEmpty(cnlNumsStr))
                {
                    IList<int> cnlNums = ScadaUtils.ParseRange(cnlNumsStr, true, true);

                    foreach (int cnlNum in cnlNums)
                    {
                        Cnl cnl = webContext.ConfigDatabase.CnlTable.GetItem(cnlNum);

                        if (cnl == null)
                            accessAllowed = false; // no rights on undefined channel
                        else if (!userContext.Rights.GetRightByObj(cnl.ObjNum).View)
                            accessAllowed = false;

                        if (!accessAllowed)
                            break;
                    }
                }
            }

            if (accessAllowed)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
