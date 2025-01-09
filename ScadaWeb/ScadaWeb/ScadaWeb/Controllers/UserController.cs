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
    /// 用户管理控制器
    /// Roles = RoleID.Administrator
    /// </summary>
    [Authorize(Roles = "1"), Route("user")]
    public class UserController : Controller
    {
        protected readonly IUserManagerService userManagerService;
        protected readonly IWebContext webContext;
        protected readonly IUserContext userContext;
        public UserController(IUserManagerService userManagerService, IWebContext webContext,IUserContext userContext)
        {
            this.userManagerService = userManagerService;
            this.webContext = webContext;
            this.userContext = userContext;

        }
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<IActionResult> GetListAsync([FromQuery]int page,[FromQuery]int limit,[FromQuery]string username)
        {
            var list = this.userManagerService.GetListAsync(page, limit,username);
            return Json(list);
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("getuserbyid")]
        public async Task<IActionResult> GetUserById([FromQuery]int userId)
        {
            var userInfo = this.userManagerService.GetUserById(userId);
            return Json(userInfo);
        }

        /// <summary>
        /// 获取角色
        /// </summary>
        [HttpGet("role")]
        public async Task<IActionResult> GetRoleAsync()
        {
            var roleTable = this.webContext.ConfigDatabase.RoleTable;
            var roleList = roleTable.EnumerateItems().Cast<Role>().ToList();
            return Json(roleList);
        }


        /// <summary>
        /// 保存用户信息
        /// </summary>
        [HttpPost("save")]
        public async Task<IActionResult> SaveUserAsync([FromBody]User user)
        {
            var res = this.userManagerService.AddOrUpdateUser(user);
            webContext.ReloadConfig();
            return Json(res);
        }

        /// <summary>
        /// 启用、禁用用户
        /// </summary>
        [HttpPost("enableuser")]
        public async Task<IActionResult> EnableUserAsync([FromBody]User user)
        {
            var res = this.userManagerService.EnableUser(user.UserID, user.Enabled);
            return Json(res);
        }

        /// <summary>
        /// 删除用户（软删除）
        /// </summary>
        [HttpPost("deleteUser")]
        public async Task<IActionResult> DeleteUserAsync([FromBody]User user)
        {
            var res = this.userManagerService.DeleteUser(user.UserID);
            return Json(res);
        }

        /// <summary>
        /// 重置用户多重认证
        /// </summary>
        [HttpPost("resetuser2fa")]
        public async Task<IActionResult> ResetUserTwoFAAsync([FromBody] ResetUserDto user)
        {
            var res = this.userManagerService.ResetUserTwoFA(user.UserID);
            return Json(res);
        }

        /// <summary>
        /// 重置用户密码
        /// </summary>
        [HttpPost("resetuserpwd")]
        public async Task<IActionResult> ResetUserPwdAsync([FromBody] ResetUserDto user)
        {
            var res = this.userManagerService.ResetUsePwd(user.UserID, user.Password);
            return Json(res);
        }

        /// <summary>
        /// 获取用户登录列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("loginLogList")]
        public async Task<IActionResult> GetLoginLogListAsync([FromQuery] int page, [FromQuery] int limit, [FromQuery] string username)
        {
            var list = this.userManagerService.GetLoginLogListAsync(page, limit, username);
            return Json(list);
        }

        /// <summary>
        /// 下载用户登录列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("downloadloginLog")]
        public async Task<IActionResult> DownloadLoginLogFileAsync([FromQuery] string username)
        {
            var simpleResult = this.userManagerService.DownloadLoginLog(username);
            if (simpleResult.Ok)
            {
                //返回文件流
                var filePath = Path.Combine(webContext.AppDirs.InstanceDir, "TempFile");
                var fileFullPath = Path.Combine(filePath, simpleResult.Data.ToString());
                return File(new FileStream(fileFullPath, FileMode.Open), "application/octet-stream", Path.GetFileName(fileFullPath)) ;
            }
            return Content(simpleResult.Msg);
        }
    }

    /// <summary>
    /// 重置用户对象
    /// </summary>
    public class ResetUserDto
    {
        public int UserID { get; set; }
        public string Password { get; set; }
    }

    /// <summary>
    /// 更新时区
    /// </summary>
    public class UpdateTimeZoneDto
    {
        public int UserID { get; set; }
        public string TimeZone { get; set; }
    }
}
