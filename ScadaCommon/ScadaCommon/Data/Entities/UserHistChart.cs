using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Data.Entities
{
    /// <summary>
    /// 用户Chart收藏记录表
    /// </summary>
    [Serializable]
    public class UserHistChart
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
        /// 收藏内容，存json
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime UpdateTime { get; set; }

    }
}
