// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using Scada.Web.Api;
using Scada.Web.Lang;
using Scada.Web.Plugins.PlgMimic.Code;
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a range of images.
        /// </summary>
        public Dto<ImagePacket> GetImages(long key, int index, int count, int size)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the faceplate.
        /// </summary>
        public Dto<FaceplatePacket> GetFaceplate(long key, string typeName)
        {
            throw new NotImplementedException();
        }
    }
}
