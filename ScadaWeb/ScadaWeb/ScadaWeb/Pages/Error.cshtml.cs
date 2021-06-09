using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Scada.Lang;
using Scada.Web.Services;
using System;
using System.Diagnostics;

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

        public void OnGet()
        {
            if (GetLastError(out Exception ex))
            {
                _logger.LogError(ex, CommonPhrases.UnhandledException);
                _webContext.Log.WriteException(ex, CommonPhrases.UnhandledException);
            }
            else
            {
                _logger.LogError(CommonPhrases.UnhandledException);
                _webContext.Log.WriteError(CommonPhrases.UnhandledException);
            }

            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        }
    }
}
