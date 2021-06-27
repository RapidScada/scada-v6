using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Scada.Web.Plugins.PlgMain.Areas.Main.Controllers
{
    [ApiController]
    [Route("Main/Api/[action]")]
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
