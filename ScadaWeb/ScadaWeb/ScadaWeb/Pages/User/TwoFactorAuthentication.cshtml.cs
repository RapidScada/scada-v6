using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Data.TwoFactorAuth;
using Scada.Lang;
using Scada.Protocol;
using Scada.Web.Authorization;
using Scada.Web.Code;
using Scada.Web.Services;
using Scada.Web.Users;
using System;
using System.Collections.Generic;
using System.Security;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Scada.Web.Pages
{
    /// <summary>
    /// 
    /// </summary>
    [BindProperties,AllowAnonymous]
    public class TwoFactorAuthenticationModel : PageModel
    {
        /// <summary>
        /// 密钥二维码地址
        /// </summary>
        public string QrCodeUrl { get; set; }

        /// <summary>
        /// 多重认证密钥
        /// </summary>
        public string TwoFactorAuthKey { get; set; }

        /// <summary>
        /// 是否认证
        /// </summary>
        public bool HasVerified { get; set; }

        /// <summary>
        /// 信任设备
        /// </summary>
        public bool TrustDevice { get; set; }

        public string VerifyCode { get; set; }

        private ILoginService loginService;
        private readonly IWebContext webContext;
        private readonly dynamic dict;
        public TwoFactorAuthenticationModel(ILoginService loginService, IWebContext webContext)
        {
            this.loginService = loginService;
            this.webContext = webContext;
            dict = Locale.GetDictionary("Scada.Web.Pages.TwoFactorAuthentication");
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var authRes= await HttpContext.AuthenticateAsync(IdentityConstants.TwoFactorUserIdScheme);
            if(authRes != null && authRes.Succeeded)
            {
                var userId = authRes.Principal.FindFirst(ClaimTypes.NameIdentifier).Value;
                TwoFactorAuthInfoResult result = await loginService.GetTwoFactorAuthenticatorKeyAsync(int.Parse(userId));
                HasVerified = result.HasVerified;
                if (!result.HasVerified)
                {
                    QrCodeUrl = result.FaQrCodeUrl;
                    TwoFactorAuthKey = result.FaSecret;
                }
                return Page();
            }
            else
            {
                return RedirectToStartPage(WebPath.LoginPage);
            }
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            CheckCode();

            return ModelState.IsValid
                ? await CheckVerifyCodeAsync(returnUrl)
                : Page();
        }

        private async Task<IActionResult> CheckVerifyCodeAsync(string returnUrl="/")
        {
            var authRes = await HttpContext.AuthenticateAsync(IdentityConstants.TwoFactorUserIdScheme);
            var userId = authRes.Principal.FindFirst(ClaimTypes.NameIdentifier).Value;
            var browserIdentity = authRes.Principal.FindFirst(ClaimExtTypes.BrowserIdentity).Value;
            TwoFactorAuthValidateResult result = await loginService.VerifyTwoFactorAuthenticatorKeyAsync(int.Parse(userId), int.Parse(VerifyCode), TrustDevice, browserIdentity);

            if (result.IsValid)
            {
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, authRes.Principal, authRes.Properties);
                return RedirectToStartPage(returnUrl);
            }
            else
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
                return Page();
            }
        }

        private bool CheckCode()
        {
            if (string.IsNullOrEmpty(VerifyCode) || VerifyCode.Length != 6)
            {
                ModelState.AddModelError(string.Empty, "Verify code is empty");
                return false;
            }
            return true;
        }

        private IActionResult RedirectToStartPage(string returnUrl)
        {
            UserConfig userConfig = webContext.PluginHolder.GetUserConfig(User.GetUserID());
            string url = Url.Content(ScadaUtils.FirstNonEmpty(
                returnUrl,
                userConfig?.StartPage,
                webContext.AppConfig.GeneralOptions.DefaultStartPage,
                WebPath.DefaultStartPage.PrependTilde()));
            return LocalRedirect(url);
        }
    }
}
