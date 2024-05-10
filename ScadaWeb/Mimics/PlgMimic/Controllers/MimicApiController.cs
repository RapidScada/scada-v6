// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using Scada.Web.Plugins.PlgMimic.MimicModel;

namespace Scada.Web.Plugins.PlgMimic.Controllers
{
    /// <summary>
    /// Represents the mimic web API.
    /// <para>Представляет веб-API мнемосхем.</para>
    /// </summary>
    [ApiController]
    [Route("Api/Mimic/[action]")]
    public class MimicApiController : ControllerBase
    {
        public Component GetComponent()
        {
            Component component = new()
            {
                ID = 1,
                Name = "Test"
            };

            return component;
        }
    }
}
