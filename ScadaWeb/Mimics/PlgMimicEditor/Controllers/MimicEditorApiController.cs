// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using Scada.Web.Api;
using Scada.Web.Plugins.PlgMimic.Controllers;
using Scada.Web.Plugins.PlgMimic.Models;

namespace Scada.Web.Plugins.PlgMimicEditor.Controllers
{
    /// <summary>
    /// Represents the mimic editor web API.
    /// <para>Представляет веб-API редактора мнемосхем.</para>
    /// </summary>
    [ApiController]
    [Route("Api/MimicEditor/[action]")]
    public class MimicEditorApiController : ControllerBase, IMimicApiController
    {
        /// <summary>
        /// Gets the mimic properties.
        /// </summary>
        public Dto<MimicPacket> GetMimicProperties(long editorKey)
        {
            return null;
        }

        /// <summary>
        /// Gets a range of components.
        /// </summary>
        public Dto<ComponentPacket> GetComponents(long editorKey, int index, int count)
        {
            return null;
        }

        /// <summary>
        /// Gets a range of images.
        /// </summary>
        public Dto<ComponentPacket> GetImages(long editorKey, int index, int count)
        {
            return null;
        }

        /// <summary>
        /// Gets the faceplate.
        /// </summary>
        public Dto<FaceplatePacket> GetFaceplate(long editorKey, string typeName)
        {
            return null;
        }
    }
}
