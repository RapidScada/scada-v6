using Scada.Data.Entities;
using Scada.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada.Web.Services
{
    /// <summary>
    /// 用户管理服务
    /// </summary>
    public interface IUserManagerService
    {
        /// <summary>
        /// 获取用户列表
        /// </summary>
        PaginationResult GetListAsync(int offset, int limit, string username);

        /// <summary>
        /// 获取用户
        /// </summary>
        User GetUserById(int userId);

        /// <summary>
        /// 新建/更新用户
        /// </summary>
        SimpleResult AddOrUpdateUser(User user);

        /// <summary>
        /// 启用/禁用用户
        /// </summary>
        SimpleResult EnableUser(int userId,bool enable);

        /// <summary>
        /// 重置用户多重认证
        /// </summary>
        SimpleResult ResetUserTwoFA(int userId);

        /// <summary>
        /// 重置用户密码
        /// </summary>
        SimpleResult ResetUsePwd(int userId,string newPwd);

        /// <summary>
        /// 修改密码
        /// </summary>
        SimpleResult ModifyPassword(int userId, string oldPwd, string newPwd);

        /// <summary>
        /// 校验密码期限
        /// </summary>
        SimpleResult CheckPassword(int userId);

        /// <summary>
        /// 删除用户
        /// </summary>
        SimpleResult DeleteUser(int userID);

        /// <summary>
        /// 获取用户登录日志
        /// </summary>
        PaginationResult GetLoginLogListAsync(int page, int limit, string username);

        /// <summary>
        /// 下载登录日志
        /// </summary>
        SimpleResult DownloadLoginLog(string username);

        /// <summary>
        /// [HistChart]获取列表
        /// </summary>
        SimpleResult ListHistChart(int offset, int limit, int userID);

        /// <summary>
        /// [HistChart]新建/更新
        /// </summary>
        SimpleResult EditHistChart(UserHistChart histChart);

        /// <summary>
        /// [HistChart]删除
        /// </summary>
        SimpleResult DelHistChart(int id,int userID);

        /// <summary>
        /// 更新用户时区
        /// </summary>
        SimpleResult UpdateTimeZone(int userId, string timeZone);
    }
}
