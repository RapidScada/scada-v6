using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Data.Entities
{
    /// <summary>
    /// 用户机器认证信息
    /// </summary>
    [Serializable]
    public class UserMachineCode
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
        /// 机器码
        /// </summary>
        public string MachineCode { get; set; }
        /// <summary>
        /// 是否过期
        /// </summary>
        public bool IsExpired { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 最近登录日期
        /// </summary>
        public DateTime LastLoginTime { get; set; }
    }
}
