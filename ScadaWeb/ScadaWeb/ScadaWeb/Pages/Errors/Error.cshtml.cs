// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Scada.Lang;
using Scada.Log;

/// <summary>
/// Represents a page containing an error message.
/// <para>Представляет страницу, которая содержит сообщение об ошибке.</para>
/// </summary>
namespace Scada.Web.Pages.Errors
{
    [AllowAnonymous]
    [IgnoreAntiforgeryToken]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorModel(
        ILogger<ErrorModel> logger,
        ILog log) : PageModel
    {
        public string ErrorMessage { get; private set; }

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
                logger.LogError(ex, CommonPhrases.UnhandledException);
                log.WriteError(ex, CommonPhrases.UnhandledException);
            }

            ErrorMessage = ex is ScadaException scadaEx && scadaEx.MessageIsPublic ? ex.Message : null;
        }
    }
}
