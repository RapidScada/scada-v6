using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Data.Entities
{
    /// <summary>
    /// 用户旧密码记录表
    /// </summary>
    [Serializable]
    public class UserLoginLog
    {
        /// <summary>
        /// 主键标识
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 登录IP
        /// </summary>
        public string LoginIP { get; set; }
        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime LoginTime { get; set; }
        /// <summary>
        /// 登录状态
        /// </summary>
        public int LoginStatus { get; set; }
        /// <summary>
        /// 登录描述
        /// </summary>
        public string LoginDesc { get; set; }
    }
}
