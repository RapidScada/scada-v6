/*
 * Copyright 2022 Rapid Software LLC
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
 * Modified : 2022
 */

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
        Task<SimpleResult> LoginAsync(string username, string password, bool rememberMe);

        /// <summary>
        /// Logs out the current user.
        /// </summary>
        Task LogoutAsync();
    }
}
