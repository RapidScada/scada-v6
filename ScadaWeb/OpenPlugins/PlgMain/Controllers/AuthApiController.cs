// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scada.Web.Api;
using Scada.Web.Lang;
using Scada.Web.Plugins.PlgMain.Code;
using Scada.Web.Plugins.PlgMain.Models;
using Scada.Web.Services;
using System;
using System.Threading.Tasks;

namespace Scada.Web.Plugins.PlgMain.Controllers
{
    /// <summary>
    /// Represents the authentication and authorization web API.
    /// <para>Представляет веб-API проверки подлинности и авторизации.</para>
    /// </summary>
    [ApiController]
    [AllowAnonymous]
    [Route("Api/Auth/[action]")]
    public class AuthApiController : ControllerBase
    {
        private readonly IWebContext webContext;
        private readonly ILoginService loginService;
        private readonly PluginContext pluginContext;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public AuthApiController(IWebContext webContext, ILoginService loginService, PluginContext pluginContext)
        {
            this.webContext = webContext;
            this.loginService = loginService;
            this.pluginContext = pluginContext;
        }


        /// <summary>
        /// Validates the username and password, and logs in.
        /// </summary>
        [HttpPost]
        public async Task<Dto> Login([FromBody] CredentialsDTO credentialsDTO)
        {
            try
            {
                if (pluginContext.Options.AllowAuthApi)
                {
                    SimpleResult result = await loginService.LoginAsync(
                        credentialsDTO.Username, credentialsDTO.Password, false);
                    return new Dto(result);
                }
                else
                {
                    return Dto.Fail(WebPhrases.ActionNotAllowed);
                }
            }
            catch (Exception ex)
            {
                webContext.Log.WriteError(ex.BuildErrorMessage(WebPhrases.ErrorInWebApi, nameof(Login)));
                return Dto.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Logs out the current user.
        /// </summary>
        [HttpPost]
        public async Task<Dto> Logout()
        {
            try
            {
                if (pluginContext.Options.AllowAuthApi)
                {
                    await loginService.LogoutAsync();
                    return Dto.Success();
                }
                else
                {
                    return Dto.Fail(WebPhrases.ActionNotAllowed);
                }
            }
            catch (Exception ex)
            {
                webContext.Log.WriteError(ex.BuildErrorMessage(WebPhrases.ErrorInWebApi, nameof(Logout)));
                return Dto.Fail(ex.Message);
            }
        }
    }
}
