using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Scada.Web.Code
{
    public class CookieBaseAuthEvents : CookieAuthenticationEvents
    {
        public override Task RedirectToLogin(RedirectContext<CookieAuthenticationOptions> context)
        {
            if (context.Request.Path.StartsWithSegments("/api") &&
                context.Response.StatusCode == StatusCodes.Status200OK)
            {
                // do not redirect to the login page
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            }
            else
            {
                return base.RedirectToLogin(context);
            }
        }

        public override Task RedirectToAccessDenied(RedirectContext<CookieAuthenticationOptions> context)
        {
            if (context.Request.Path.StartsWithSegments("/api") &&
                context.Response.StatusCode == StatusCodes.Status200OK)
            {
                // do not redirect to the access denied page
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return Task.CompletedTask;
            }
            else
            {
                return base.RedirectToAccessDenied(context);
            }
        }
    }
}
