// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scada.Web.Authorization;
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
        [HttpPost]
        public void UpdateMimic(UpdateDTO updateDto)
        {

        }

        [HttpPost]
        public void SaveMimic()
        {

        }

        [HttpPost]
        public void CloseMimic()
        {

        }
    }
}
