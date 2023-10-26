using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada.Web.Authorization
{
    /// <summary>
    /// Claim 拓展类型
    /// </summary>
    public class ClaimExtTypes
    {
        /// <summary>
        /// 多重认证
        /// </summary>
        public const string TwoFactorEnabled = "TwoFactorEnabled";

        /// <summary>
        /// 机器码认证
        /// </summary>
        public const string BrowserIdentity = "BrowserIdentity";

        /// <summary>
        /// 第一次登录系统
        /// </summary>
        public const string FirstWebLogin = "FirstWebLogin";
    }
}
