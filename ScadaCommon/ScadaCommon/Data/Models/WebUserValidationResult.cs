/*
 * Copyright 2023 Rapid Software LLC
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * 
 * Product  : Rapid SCADA
 * Module   : ScadaCommon
 * Summary  : Represents a result of web user validation
 * 
 * Author   : Zeus
 * Created  : 2023
 * Modified : 2023
 */

namespace Scada.Data.Models
{
    /// <summary>
    /// 网页用户登录结果.
    /// </summary>
    public class WebUserValidationResult
    {
        /// <summary>
        /// Represents an empty result.
        /// </summary>
        public static readonly WebUserValidationResult Empty = new WebUserValidationResult(false, false);


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public WebUserValidationResult()
            : this(true, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public WebUserValidationResult(bool handled, bool isValid)
        {
            Handled = handled;
            IsValid = isValid;
            UserID = 0;
            RoleID = 0;
            ErrorMessage = "";
            NeedFactorAuth = false;
            NeedModifyPwd = false;
            UserName = "";
            ClientIpAddress = "";
        }


        /// <summary>
        /// Gets or sets a value indicating whether user validation is handled by a module.
        /// </summary>
        public bool Handled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is successfully validated.
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// Gets or sets the role ID.
        /// </summary>
        public int RoleID { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 需要2fa认证
        /// </summary>
        public bool NeedFactorAuth { get; set; }

        /// <summary>
        /// 需要修改密码
        /// </summary>
        public bool NeedModifyPwd { get; set; }

        /// <summary>
        /// 登录用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 客户端IP地址
        /// </summary>
        public string ClientIpAddress { get; set; }

        /// <summary>
        /// Creates a failed result with the specified message.
        /// </summary>
        public static WebUserValidationResult Fail(string message)
        {
            return new WebUserValidationResult
            {
                ErrorMessage = message
            };
        }
    }
}
