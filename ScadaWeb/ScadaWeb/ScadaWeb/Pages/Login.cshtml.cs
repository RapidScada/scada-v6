// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Scada.Lang;
using Scada.Web.Code;
using Scada.Web.Config;
using Scada.Web.Services;
using Scada.Web.Users;
using System;
using System.Threading.Tasks;

namespace Scada.Web.Pages
{
    /// <summary>
    /// Represents a login page.
    /// <para>Представляет страницу входа в систему.</para>
    /// </summary>
    [BindProperties]
    public class LoginModel : PageModel
    {
        private const string CaptchaSessionKey = "ScadaWeb.CaptchaCode";

        private readonly IWebContext webContext;
        private readonly ILoginService loginService;
        private readonly dynamic dict;


        public LoginModel(IWebContext webContext, ILoginService loginService)
        {
            this.webContext = webContext;
            this.loginService = loginService;
            dict = Locale.GetDictionary("Scada.Web.Pages.Login");
        }


        public string Username { get; set; }

        public string Password { get; set; }

        public string CaptchaCode { get; set; }

        public bool RememberMe { get; set; }

        [TempData]
        public bool JustLogout { get; set; }


        private bool CheckReady()
        {
            if (webContext.IsReadyToLogin)
            {
                return true;
            }
            else
            {
                ModelState.AddModelError(string.Empty, dict.NotReady);
                return false;
            }
        }

        private void ValidateCaptcha()
        {
            if (webContext.AppConfig.LoginOptions.RequireCaptcha)
            {
                string requiredCode = HttpContext.Session.GetString(CaptchaSessionKey);

                if (string.IsNullOrEmpty(requiredCode) || string.IsNullOrEmpty(CaptchaCode) ||
                    !string.Equals(requiredCode, CaptchaCode.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    webContext.Log.WriteError(Locale.IsRussian ?
                        "Указан неверный защитный код, IP {0}" :
                        "Invalid captcha specified, IP {0}", HttpContext.Connection.RemoteIpAddress);
                    ModelState.AddModelError(string.Empty, dict.InvalidCaptcha);
                    ModelState.Remove(nameof(CaptchaCode));
                    CaptchaCode = "";
                }
            }
        }

        private async Task<IActionResult> LoginAsync(string returnUrl)
        {
            SimpleResult result = await loginService.LoginAsync(Username, Password, RememberMe);

            if (result.Ok)
            {
                return RedirectToStartPage(returnUrl);
            }
            else
            {
                ModelState.AddModelError(string.Empty, result.Msg);
                return Page();
            }
        }

        private IActionResult RedirectToStartPage(string returnUrl)
        {
            UserConfig userConfig = webContext.PluginHolder.GetUserConfig(User.GetUserID());
            string url = Url.Content(ScadaUtils.FirstNonEmpty(
                returnUrl,
                userConfig?.StartPage,
                webContext.AppConfig.GeneralOptions.DefaultStartPage,
                WebPath.DefaultStartPage.PrependTilda()));
            return LocalRedirect(url);
        }

        public HtmlString GetCaptchaImage()
        {
            Captcha captcha = new();
            string captchaCode = captcha.GenerateCode();
            HttpContext.Session.SetString(CaptchaSessionKey, captchaCode);
            return new HtmlString(captcha.GenerateImageSrc(captchaCode));
        }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            bool isReady = CheckReady();
            LoginOptions loginOptions = webContext.AppConfig.LoginOptions;

            // already logged in
            if (isReady && User.IsAuthenticated())
                return RedirectToStartPage(returnUrl);

            // auto login
            if (!string.IsNullOrEmpty(loginOptions.AutoLoginUsername))
            {
                Username = loginOptions.AutoLoginUsername;
                Password = loginOptions.AutoLoginPassword;
                RememberMe = loginOptions.AllowRememberMe;

                if (isReady && !JustLogout)
                    return await LoginAsync(returnUrl);
            }

            // normal login
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            CheckReady();
            ValidateCaptcha();

            return ModelState.IsValid
                ? await LoginAsync(returnUrl)
                : Page();
        }
    }
}
