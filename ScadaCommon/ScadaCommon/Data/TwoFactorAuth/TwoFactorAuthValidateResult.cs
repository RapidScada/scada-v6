using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Data.TwoFactorAuth
{
    /// <summary>
    /// 校验结果
    /// </summary>
    public class TwoFactorAuthValidateResult
    {
        /// <summary>
        /// ctor
        /// </summary>
        public TwoFactorAuthValidateResult()
        {
            IsValid = false;
            ErrorMessage = string.Empty;
        }

        /// <summary>
        /// 是否校验通过
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string ErrorMessage { get; set; }


        /// <summary>
        /// Creates a failed result with the specified message.
        /// </summary>
        public static TwoFactorAuthValidateResult Fail(string message)
        {
            return new TwoFactorAuthValidateResult
            {
                ErrorMessage = message
            };
        }
    }
}
