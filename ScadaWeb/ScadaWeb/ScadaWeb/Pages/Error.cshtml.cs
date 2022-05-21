// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Scada.Lang;
using Scada.Log;
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
        private readonly ILog _log;

        public ErrorModel(ILogger<ErrorModel> logger, ILog log)
        {
            _logger = logger;
            _log = log;
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
                _log.WriteError(ex, CommonPhrases.UnhandledException);
            }

            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        }
    }
}
