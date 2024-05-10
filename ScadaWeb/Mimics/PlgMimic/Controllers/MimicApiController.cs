// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using Scada.Web.Plugins.PlgMimic.MimicModel;
using System.Dynamic;

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
            dynamic dto = new ExpandoObject();
            dto.Prop1 = "aaa";

            dynamic obj2 = new ExpandoObject();
            obj2.Field1 = 123.45;
            dto.Prop2 = obj2;

            Component component = new()
            {
                ID = 1,
                Name = "Test",
                Properties = dto
            };

            dynamic dtoLink = component.Properties;
            dtoLink.Prop3 = DateTime.UtcNow;

            return component;
        }
    }
}
