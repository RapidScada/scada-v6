using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scada.Protocol;
using Scada.Web.Services;
using System;
using System.Threading.Tasks;

namespace Scada.Web.Controllers
{
    [Route("google"),AllowAnonymous]
    public class GoogleController : Controller
    {
        private readonly ILoginService loginService;
        private readonly IWebContext webContext;
        public GoogleController(ILoginService loginService, IWebContext webContext)
        {
            this.loginService = loginService;
            this.webContext = webContext;

        }

        /// <summary>
        /// Google oath登录
        /// </summary>
        [HttpPost("signin")]
        public async Task<IActionResult> SigninGoogle([FromForm] GoogleAuth googleAuth)
        {
            this.webContext.Log.WriteAction("google sso signin");
            var csrf_token_cookie = Request.Cookies["g_csrf_token"];
            //校验scrf cookie
            if (string.IsNullOrEmpty(csrf_token_cookie) || csrf_token_cookie != googleAuth.G_csrf_token)
            {
                HttpContext.Response.Cookies.Append("loginErr", "Login failed with an unknown expection");
                return Redirect(WebPath.LoginPage);
            }
            if (googleAuth.Credential != "")
            {
                try
                {
                    //后台校验收到的token
                    var validPayload = await GoogleJsonWebSignature.ValidateAsync(googleAuth.Credential);
                    if (validPayload == null)
                    {
                        HttpContext.Response.Cookies.Append("loginErr", "Google Credential is not valid");
                        return Redirect(WebPath.LoginPage);
                    }

                    var result = await loginService.LoginAsync(validPayload.Email, "", WebLoginType.GoogleOAuth, "", false);
                    if (result.Ok)
                    {
                        HttpContext.Response.Cookies.Append("google_name", validPayload.Name);
                        HttpContext.Response.Cookies.Append("google_avator", validPayload.Picture);
                        return Redirect(WebPath.Root);
                    }
                    else
                    {
                        HttpContext.Response.Cookies.Append("loginErr", $"{result.Msg}");
                    }
                }
                catch (Exception ex)
                {
                    HttpContext.Response.Cookies.Append("loginErr", $"{ex.Message}");
                }
            }
            else
            {
                HttpContext.Response.Cookies.Append("loginErr", "Google Credential is empty");
            }
            
            return Redirect(WebPath.LoginPage);
        }
    }

    public class GoogleAuth
    {
        public string Credential { get; set; }
        public string G_csrf_token { get; set; }
    }
}
