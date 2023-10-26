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
 * Module   : ScadaWebCommon
 * Summary  : Defines functionality for user login and logout
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2022
 * Modified : 2023
 */

using Scada.Data.TwoFactorAuth;
using System.Threading.Tasks;

namespace Scada.Web.Services
{
    /// <summary>
    /// Defines functionality for user login and logout.
    /// <para>Определяет функциональность для входа и выхода пользователя.</para>
    /// </summary>
    public interface ILoginService
    {
        /// <summary>
        /// Validates the username and password, and logs in.
        /// </summary>
        Task<SimpleResult> LoginAsync(string username, string password, string loginType, string browserIdentity, bool rememberMe);

        /// <summary>
        /// Logs out the current user.
        /// </summary>
        Task LogoutAsync();

        /// <summary>
        /// 获取2FA密钥
        /// </summary>
        Task<TwoFactorAuthInfoResult> GetTwoFactorAuthenticatorKeyAsync(int userId);

        /// <summary>
        /// 验证2FA密钥
        /// </summary>
        Task<TwoFactorAuthValidateResult> VerifyTwoFactorAuthenticatorKeyAsync(int userId, int code, bool trustDevice, string browserIdentity);

        /// <summary>
        /// 重置2FA密钥
        /// </summary>
        Task<SimpleResult> ResetTwoFactorAuthenticatorKeyAsync(int userId);

        /// <summary>
        /// 重置用户
        /// </summary>
        Task<SimpleResult> ResetUserAsync(int userId);
    }
}
