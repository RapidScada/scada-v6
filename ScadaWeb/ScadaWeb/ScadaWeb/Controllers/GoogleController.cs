using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using Scada.Web.Services;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Scada.Protocol;
using System.Web;

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
                var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                var readJwtToken = jwtSecurityTokenHandler.ReadJwtToken(googleAuth.Credential);
                
                var jwtClaims = readJwtToken.Claims;
                var username = jwtClaims.FirstOrDefault(c => c.Type == "email").Value;
                var result = await loginService.LoginAsync(username, "", WebLoginType.GoogleOAuth, "", false);
                if (result.Ok)
                {
                    return Redirect(WebPath.Root);
                }
                else
                {
                    HttpContext.Response.Cookies.Append("loginErr", $"{result.Msg}");
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
