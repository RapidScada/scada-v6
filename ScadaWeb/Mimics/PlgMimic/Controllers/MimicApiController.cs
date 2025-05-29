// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using Scada.Lang;
using Scada.Web.Api;
using Scada.Web.Lang;
using Scada.Web.Plugins.PlgMimic.Code;
using Scada.Web.Plugins.PlgMimic.MimicModel;
using Scada.Web.Plugins.PlgMimic.Models;
using Scada.Web.Services;

namespace Scada.Web.Plugins.PlgMimic.Controllers
{
    /// <summary>
    /// Represents the mimic web API.
    /// <para>Представляет веб-API мнемосхем.</para>
    /// </summary>
    [ApiController]
    [Route("Api/Mimic/[action]")]
    [CamelCaseJsonFormatter]
    public class MimicApiController(
        IWebContext webContext,
        IViewLoader viewLoader) : ControllerBase, IMimicApiController
    {
        /// <summary>
        /// Gets the mimic properties.
        /// </summary>
        public Dto<MimicPacket> GetMimicProperties(long key)
        {
            try
            {
                return viewLoader.GetView((int)key, true, out MimicView mimicView, out string errMsg)
                    ? Dto<MimicPacket>.Success(new MimicPacket(key, mimicView.Mimic))
                    : Dto<MimicPacket>.Fail(errMsg);
            }
            catch (Exception ex)
            {
                webContext.Log.WriteError(ex.BuildErrorMessage(WebPhrases.ErrorInWebApi, nameof(GetMimicProperties)));
                return Dto<MimicPacket>.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Gets a range of components.
        /// </summary>
        public Dto<ComponentPacket> GetComponents(long key, int index, int count)
        {
            try
            {
                return viewLoader.GetView((int)key, false, out MimicView mimicView, out string errMsg)
                    ? Dto<ComponentPacket>.Success(new ComponentPacket(key, mimicView.Mimic, index, count))
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
            return viewLoader.GetView((int)key, false, out MimicView mimicView, out string errMsg)
                ? Dto<ImagePacket>.Success(new ImagePacket(key, mimicView.Mimic, index, count, size))
                : Dto<ImagePacket>.Fail(errMsg);
        }

        /// <summary>
        /// Gets the faceplate.
        /// </summary>
        public Dto<FaceplatePacket> GetFaceplate(long key, string typeName)
        {
            try
            {
                if (viewLoader.GetView((int)key, false, out MimicView mimicView, out string errMsg))
                {
                    if (mimicView.Mimic.FaceplateMap.TryGetValue(typeName, out Faceplate faceplate))
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
    }
}
