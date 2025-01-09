using Microsoft.AspNetCore.Http;
using Scada.Data.Entities;
using Scada.Response;
using Scada.Web.Services;
using System;
using System.Collections.Generic;

namespace Scada.Web.Code
{
    /// <summary>
    /// 
    /// </summary>
    public class UserManagerService : IUserManagerService
    {
        private readonly IWebContext webContext;
        private readonly IClientAccessor clientAccessor;

        public UserManagerService(IWebContext webContext, IClientAccessor clientAccessor)
        {
            this.webContext = webContext ?? throw new ArgumentNullException(nameof(webContext));
            this.clientAccessor = clientAccessor;
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        public PaginationResult GetListAsync(int page, int limit, string username)
        {
            try
            {
                var offset = (page - 1) * limit;
                var result = clientAccessor.ScadaClient.GetUserList(offset, limit, username);
                return result.Data as PaginationResult;
            }
            catch (Exception ex)
            {
                return PaginationResult.Empty(0,1);
            }
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        public User GetUserById(int userId)
        {
            if(userId == 0)
            {
                //用户默认信息
                return new User()
                {
                    Enabled = true,
                    Gender = "1",
                    RoleID = 3,
                    UserPwdEnabled = true,
                    PwdPeriodModify = true,
                    PwdPeriodLimit = 30,
                    PwdLenLimit = 8,
                    PwdComplicatedRequire = true,
                    PwdComplicatedFormat = "num|letter",
                    PwdUsedDifferent = true,
                    PwdUsedTimes = 3
                };
            }
            var user = clientAccessor.ScadaClient.GetUserByID(userId);
            if(user != null)
            {
                if (string.IsNullOrEmpty(user.Gender)) user.Gender = "1";
                if (user.RoleID == 0) user.RoleID = 3;//Guest
            }
            return user;
        }

        public SimpleResult AddOrUpdateUser(User user)
        {
            return clientAccessor.ScadaClient.AddOrUpdateUser(user);
        }

        public SimpleResult EnableUser(int userId, bool enable)
        {
            return clientAccessor.ScadaClient.EnableUser(userId,enable);
        }

        public SimpleResult ResetUserTwoFA(int userId)
        {
            return clientAccessor.ScadaClient.ResetUserTwoFA(userId);
        }
        
        public SimpleResult ResetUsePwd(int userId, string newPwd)
        {
            return clientAccessor.ScadaClient.ResetUserPwd(userId, newPwd);
        }

        public SimpleResult ModifyPassword(int userId, string oldPwd, string newPwd)
        {
            return clientAccessor.ScadaClient.ModifyPassword(userId, oldPwd, newPwd);
        }

        public SimpleResult CheckPassword(int userId)
        {
            return clientAccessor.ScadaClient.CheckPassword(userId);
        }


        /// <summary>
        /// 删除用户
        /// </summary>
        public SimpleResult DeleteUser(int userId)
        {
            return clientAccessor.ScadaClient.DeleteUser(userId);
        }


        /// <summary>
        /// 获取用户登录日志列表
        /// </summary>
        public PaginationResult GetLoginLogListAsync(int page, int limit, string username)
        {
            try
            {
                var offset = (page - 1) * limit;
                var result = clientAccessor.ScadaClient.GetLoginLogList(offset, limit, username);
                return result.Data as PaginationResult;
            }
            catch (Exception ex)
            {
                return PaginationResult.Empty(0, 1);
            }
        }

        /// <summary>
        /// 下载登录日志
        /// </summary>
        public SimpleResult DownloadLoginLog(string username)
        {
            try
            {
                return clientAccessor.ScadaClient.DownloadLoginLog(username);
            }
            catch (Exception ex)
            {
                return SimpleResult.Fail(ex.Message);
            }
        }

        /// <summary>
        /// [HistChart]获取列表
        /// </summary>
        public SimpleResult ListHistChart(int offset, int limit, int userID)
        {
            try
            {
                return clientAccessor.ScadaClient.ListUserHisChart(offset, limit, userID);
            }
            catch (Exception ex)
            {
                return SimpleResult.Fail(ex.Message);
            }
        }

        /// <summary>
        /// [HistChart]新建/更新
        /// </summary>
        public SimpleResult EditHistChart(UserHistChart histChart)
        {
            try
            {
                return clientAccessor.ScadaClient.SaveUserHisChart(histChart);
            }
            catch (Exception ex)
            {
                return SimpleResult.Fail(ex.Message);
            }
        }

        /// <summary>
        /// [HistChart]删除
        /// </summary>
        public SimpleResult DelHistChart(int id, int userID)
        {
            try
            {
                return clientAccessor.ScadaClient.DelUserHistChart(id, userID);
            }
            catch (Exception ex)
            {
                return SimpleResult.Fail(ex.Message);
            }
        }

        /// <summary>
        /// 更新用户时区
        /// </summary>
        public SimpleResult UpdateTimeZone(int userId, string timeZone)
        {
            return clientAccessor.ScadaClient.UpdateUserTimeZone(userId, timeZone);
        }
    }
}
