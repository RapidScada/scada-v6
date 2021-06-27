// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Scada.Web.Plugins.PlgMain.Controllers
{
    [ApiController]
    [Route("Api/Main/[action]")]
    //[Route("Main/Api/[action]")]
    public class MainApiController : ControllerBase
    {
        public IEnumerable<int> Get()
        {
            return new int[] { 1, 2, 3, 4, 5 };
        }

        public IEnumerable<int> Get2()
        {
            return new int[] { 6, 7, 8, 9, 10 };
        }
    }
}
