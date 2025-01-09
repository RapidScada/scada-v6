using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scada.Data.Entities;
using Scada.Web.Code;
using Scada.Web.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Scada.Web.Controllers
{
    /// <summary>
    /// ChartPro收藏控制器
    /// </summary>
    [Authorize, Route("chartfavor")]
    public class ChartFavoriteController : Controller
    {
        protected readonly IUserManagerService userManagerService;
        protected readonly IWebContext webContext;
        public ChartFavoriteController(IUserManagerService userManagerService, IWebContext webContext)
        {
            this.userManagerService = userManagerService;
            this.webContext = webContext;

        }

        /// <summary>
        /// [HistChart]获取列表
        /// </summary>
        [HttpGet("list")]
        public IActionResult ListHistChart()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userInfo = this.userManagerService.ListHistChart(0, 10, int.Parse(userId));
            return Json(userInfo);
        }

        /// <summary>
        /// [HistChart]新建/更新
        /// </summary>
        [HttpPost("edit")]
        public IActionResult EditHistChart([FromBody] UserHistChart histChart)
        {
            if(histChart.Id == 0)
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                histChart.UserID = int.Parse(userId);
                histChart.CreateTime = DateTime.Now;
            }
            histChart.UpdateTime = DateTime.Now;
            var res = this.userManagerService.EditHistChart(histChart);
            return Json(res);
        }

        /// <summary>
        /// [HistChart]删除
        /// </summary>
        [HttpPost("delete")]
        public IActionResult DelHistChart([FromQuery] int id)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var res = this.userManagerService.DelHistChart(id, int.Parse(userId));
            return Json(res);
        }
    }
}
