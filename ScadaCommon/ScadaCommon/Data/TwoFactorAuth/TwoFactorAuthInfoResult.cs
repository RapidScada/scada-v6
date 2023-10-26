using Scada.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Data.TwoFactorAuth
{
    /// <summary>
    /// 多重认证信息
    /// </summary>
    public class TwoFactorAuthInfoResult
    {
        /// <summary>
        /// ctor
        /// </summary>
        public TwoFactorAuthInfoResult()
        {
            HasVerified = false;
            FaSecret = string.Empty;
            ErrorMessage = string.Empty;
        }

        /// <summary>
        /// 是否验证过
        /// </summary>
        public bool HasVerified { get; set; }

        /// <summary>
        /// fa密钥
        /// </summary>
        public string FaSecret { get; set; }

        /// <summary>
        /// fa二维码地址
        /// </summary>
        public string FaQrCodeUrl { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string ErrorMessage { get; set; }


        /// <summary>
        /// Creates a failed result with the specified message.
        /// </summary>
        public static TwoFactorAuthInfoResult Fail(string message)
        {
            return new TwoFactorAuthInfoResult
            {
                ErrorMessage = message
            };
        }
    }
}
