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
 * Summary  : Represents a page containing an error message
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Scada.Lang;
using Scada.Web.Services;
using System;
using System.Diagnostics;

/// <summary>
/// Represents a page containing an error message.
/// <para>Представляет страницу, которая содержит сообщение об ошибке.</para>
/// </summary>
namespace Scada.Web.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        private readonly ILogger<ErrorModel> _logger;
        private readonly IWebContext _webContext;

        public ErrorModel(ILogger<ErrorModel> logger, IWebContext webContext)
        {
            _logger = logger;
            _webContext = webContext;
        }

        private bool GetLastError(out Exception ex)
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            ex = exceptionHandlerPathFeature?.Error;
            return ex != null;
        }

        public void WriteError()
        {
            if (GetLastError(out Exception ex))
            {
                _logger.LogError(ex, CommonPhrases.UnhandledException);
                _webContext.Log.WriteException(ex, CommonPhrases.UnhandledException);
            }

            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        }
    }
}
