using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scada.Web.Code;
using Scada.Web.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Scada.Web.Controllers
{
    [Authorize, Route("account")]
    public class AccountController : Controller
    {

        protected readonly IUserManagerService userManagerService;
        protected readonly IWebContext webContext;
        protected readonly IUserContext userContext;
        public AccountController(IUserManagerService userManagerService, IWebContext webContext, IUserContext userContext)
        {
            this.userManagerService = userManagerService;
            this.webContext = webContext;
            this.userContext = userContext;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        [HttpPost("modifypassword")]
        public async Task<IActionResult> ModifyPasswordAsync([FromBody] ModifyPasswordDto modifyPwd)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var res = this.userManagerService.ModifyPassword(int.Parse(userId),modifyPwd.OldPwd, modifyPwd.NewPwd);
            return Json(res);
        }

        /// <summary>
        /// 检验密码是否快到期
        /// </summary>
        [HttpGet("checkpassword")]
        public async Task<IActionResult> CheckPasswordAsync()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var res = this.userManagerService.CheckPassword(int.Parse(userId));
            return Json(res);
        }

        /// <summary>
        /// 用户修改时区
        /// </summary>
        [HttpPost("updatetimezone")]
        public IActionResult UpdateTimezone([FromBody] UpdateTimeZoneDto user)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var res = this.userManagerService.UpdateTimeZone(int.Parse(userId), user.TimeZone);
            if (res.Ok) ((UserContext)this.userContext).SetTimeZone(user.TimeZone);
            return Json(res);
        }
    }


    public class ModifyPasswordDto
    {
        public string OldPwd { get; set; }
        public string NewPwd { get; set; }
    }
}
