// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scada.Web.Api;
using Scada.Web.Authorization;
using Scada.Web.Lang;
using Scada.Web.Plugins.PlgMimicEditor.Code;
using Scada.Web.Plugins.PlgMimicEditor.Models;
using Scada.Web.Services;

namespace Scada.Web.Plugins.PlgMimicEditor.Controllers
{
    /// <summary>
    /// Represents a web API for updating mimic diagrams on the server side.
    /// <para>Представляет веб-API для обновления мнемосхем на стороне сервера.</para>
    /// </summary>
    [ApiController]
    [Route("Api/MimicEditor/Updater/[action]")]
    [Authorize(Policy = PolicyName.Administrators)]
    public class UpdaterApiController(
        IWebContext webContext,
        EditorManager editorManager) : ControllerBase
    {
        /// <summary>
        /// Updates the mimic.
        /// </summary>
        [HttpPost]
        public Dto UpdateMimic([FromBody] UpdateDto updateDto)
        {
            try
            {
                if (editorManager.FindMimic(updateDto.MimicKey, out MimicInstance mimicInstance, out string errMsg))
                {
                    lock (mimicInstance.Mimic.SyncRoot)
                    {
                        mimicInstance.RegisterClientActivity();
                        mimicInstance.Mimic.ApplyChanges(updateDto.Changes);
                        return Dto.Success();
                    }
                }
                else
                {
                    return Dto.Fail(errMsg);
                }
            }
            catch (Exception ex)
            {
                webContext.Log.WriteError(ex.BuildErrorMessage(WebPhrases.ErrorInWebApi, nameof(UpdateMimic)));
                return Dto.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Saves the mimic.
        /// </summary>
        [HttpPost]
        public Dto SaveMimic([FromQuery] long key)
        {
            try
            {
                return editorManager.SaveMimic(key, out string errMsg)
                    ? Dto.Success()
                    : Dto.Fail(errMsg);
            }
            catch (Exception ex)
            {
                webContext.Log.WriteError(ex.BuildErrorMessage(WebPhrases.ErrorInWebApi, nameof(SaveMimic)));
                return Dto.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Closes and optionally saves the mimic.
        /// </summary>
        [HttpPost]
        public Dto CloseMimic([FromQuery] long key, [FromQuery] bool save)
        {
            try
            {
                if (save && !editorManager.SaveMimic(key, out string errMsg))
                    return Dto.Fail(errMsg);
                
                editorManager.CloseMimic(key);
                return Dto.Success();
            }
            catch (Exception ex)
            {
                webContext.Log.WriteError(ex.BuildErrorMessage(WebPhrases.ErrorInWebApi, nameof(CloseMimic)));
                return Dto.Fail(ex.Message);
            }
        }
    }
}
