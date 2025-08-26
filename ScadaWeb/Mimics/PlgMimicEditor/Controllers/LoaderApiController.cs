// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scada.Lang;
using Scada.Web.Api;
using Scada.Web.Authorization;
using Scada.Web.Lang;
using Scada.Web.Plugins.PlgMimic.Controllers;
using Scada.Web.Plugins.PlgMimic.MimicModel;
using Scada.Web.Plugins.PlgMimic.Models;
using Scada.Web.Plugins.PlgMimicEditor.Code;
using Scada.Web.Services;

namespace Scada.Web.Plugins.PlgMimicEditor.Controllers
{
    /// <summary>
    /// Represents a web API for loading mimic diagrams.
    /// <para>Представляет веб-API для загрузки мнемосхем.</para>
    /// </summary>
    [ApiController]
    [Route("Api/MimicEditor/Loader/[action]")]
    [Authorize(Policy = PolicyName.Administrators)]
    [TypeFilter(typeof(MimicLockFilter))]
    [CamelCaseJsonFormatter]
    public class LoaderApiController(
        IWebContext webContext,
        EditorManager editorManager) : ControllerBase, IMimicApiController
    {
        /// <summary>
        /// Gets the mimic properties.
        /// </summary>
        public Dto<MimicPacket> GetMimicProperties(long key)
        {
            try
            {
                return editorManager.FindMimic(key, out MimicInstance mimicInstance, out string errMsg)
                    ? Dto<MimicPacket>.Success(new MimicPacket(key, mimicInstance.Mimic))
                    : Dto<MimicPacket>.Fail(errMsg);
            }
            catch (Exception ex)
            {
                webContext.Log.WriteError(ex.BuildErrorMessage(WebPhrases.ErrorInWebApi, nameof(GetMimicProperties)));
                return Dto<MimicPacket>.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Gets the faceplate.
        /// </summary>
        public Dto<FaceplatePacket> GetFaceplate(long key, string typeName)
        {
            try
            {
                if (editorManager.FindMimic(key, out MimicInstance mimicInstance, out string errMsg))
                {
                    if (mimicInstance.Mimic.FaceplateMap.TryGetValue(typeName, out Faceplate faceplate))
                    {
                        return Dto<FaceplatePacket>.Success(new FaceplatePacket(key, faceplate));
                    }
                    else
                    {
                        return Dto<FaceplatePacket>.Fail(Locale.IsRussian ?
                            "Фейсплейт не найден." :
                            "Faceplate not found.");
                    }
                }
                else
                {
                    return Dto<FaceplatePacket>.Fail(errMsg);
                }
            }
            catch (Exception ex)
            {
                webContext.Log.WriteError(ex.BuildErrorMessage(WebPhrases.ErrorInWebApi, nameof(GetFaceplate)));
                return Dto<FaceplatePacket>.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Gets a range of components.
        /// </summary>
        public Dto<ComponentPacket> GetComponents(long key, int index, int count)
        {
            try
            {
                return editorManager.FindMimic(key, out MimicInstance mimicInstance, out string errMsg)
                    ? Dto<ComponentPacket>.Success(new ComponentPacket(key, mimicInstance.Mimic, index, count))
                    : Dto<ComponentPacket>.Fail(errMsg);
            }
            catch (Exception ex)
            {
                webContext.Log.WriteError(ex.BuildErrorMessage(WebPhrases.ErrorInWebApi, nameof(GetComponents)));
                return Dto<ComponentPacket>.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Gets a range of images.
        /// </summary>
        public Dto<ImagePacket> GetImages(long key, int index, int count, int size)
        {
            try
            {
                return editorManager.FindMimic(key, out MimicInstance mimicInstance, out string errMsg)
                    ? Dto<ImagePacket>.Success(new ImagePacket(key, mimicInstance.Mimic, index, count, size))
                    : Dto<ImagePacket>.Fail(errMsg);
            }
            catch (Exception ex)
            {
                webContext.Log.WriteError(ex.BuildErrorMessage(WebPhrases.ErrorInWebApi, nameof(GetImages)));
                return Dto<ImagePacket>.Fail(ex.Message);
            }
        }
    }
}
