// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Scada.Lang;
using Scada.Log;

namespace Scada.Web.Pages.Errors
{
    /// <summary>
    /// Represents a page containing an error message.
    /// <para>Представляет страницу, которая содержит сообщение об ошибке.</para>
    /// </summary>
    [AllowAnonymous]
    [IgnoreAntiforgeryToken]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorModel(
        ILogger<ErrorModel> logger,
        ILog log) : PageModel
    {
        public dynamic Dict { get; } = Locale.GetDictionary("Scada.Web.Pages.Errors.Error");

        public string ErrorMessage { get; private set; }


        private void WriteError()
        {
            Exception ex = HttpContext.Features.Get<IExceptionHandlerPathFeature>()?.Error;

            if (ex != null)
            {
                logger.LogError(ex, CommonPhrases.UnhandledException);
                log.WriteError(ex, CommonPhrases.UnhandledException);
            }

            ErrorMessage = (ex is ScadaException { MessageIsPublic: true } scadaEx ? scadaEx.Message : null)
                ?? (string)Dict.ErrorMessage;
        }

        private IActionResult HandleRequest()
        {
            WriteError();

            // AJAX request called by partial
            if (Request.Headers.XRequestedWith == "XMLHttpRequest")
            {
                return new ContentResult
                {
                    StatusCode = Response.StatusCode,
                    ContentType = "text/plain; charset=utf-8",
                    Content = ErrorMessage
                };
            }

            return Page();
        }

        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            // replaces any handler (OnGet, OnPost, OnPostXxx, ...)
            context.Result = HandleRequest();
        }
    }
}
