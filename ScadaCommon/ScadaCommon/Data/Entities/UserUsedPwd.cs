using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Data.Entities
{
    /// <summary>
    /// 用户旧密码记录表
    /// </summary>
    [Serializable]
    public class UserUsedPwd
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
        /// 密码--加密
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateTime { get; set; }

    }
}
