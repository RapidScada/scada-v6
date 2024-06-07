// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using Scada.Web.Api;
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
    public class MimicApiController : ControllerBase //, IMimicApiController
    {
        public Dto<Component> GetComponent()
        {
            Component component = new()
            {
                ID = 1,
                Name = "Test"
            };

            dynamic props = component.Properties;
            props.Prop1 = "aaa";
            props.Prop2 = DateTime.UtcNow;

            dynamic myObj = new ExpandoObject();
            myObj.Field1 = 123.45;
            props.Prop3 = myObj;

            IDictionary<string, object> dict = component.Properties;
            dict.Add("Prop4", "abc");

            return Dto<Component>.Success(component);
        }
    }
}
