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
 * Summary  : Represents a filter that checks if the application is ready for operating
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Scada.Web.Code
{
    /// <summary>
    /// Represents a filter that checks if the application is ready for operating.
    /// <para>Представляет фильтр, проверяющий, готово ли приложение к работе.</para>
    /// </summary>
    public class CheckReadyPageFilter : IPageFilter
    {
        private readonly IWebContext webContext;

        public CheckReadyPageFilter(IWebContext webContext)
        {
            this.webContext = webContext;
        }

        public void OnPageHandlerSelected(PageHandlerSelectedContext context)
        {
            // do nothing
        }

        public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            if (!webContext.IsReady)
            {
                string path = context.HttpContext.Request.Path;
                if (path != WebUrl.Root &&
                    path != WebUrl.IndexPage &&
                    path != WebUrl.LoginPage &&
                    path != WebUrl.LogoutPage)
                {
                    context.Result = new StatusCodeResult(StatusCodes.Status503ServiceUnavailable);
                }
            }
        }

        public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {
            // do nothing
        }
    }
}
