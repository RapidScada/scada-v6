/*
 * Copyright 2024 Rapid Software LLC
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
 * Summary  : Provides data for the OnUserLogin and OnUserLogout plugin actions
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

namespace Scada.Web.Plugins
{
    /// <summary>
    /// Provides data for the OnUserLogin and OnUserLogout plugin actions.
    /// <para>Предоставляет данные для действий плагинов OnUserLogin и OnUserLogout.</para>
    /// </summary>
    public class UserLoginArgs
    {
        /// <summary>
        /// Gets the username.
        /// </summary>
        public string Username { get; init; }

        /// <summary>
        /// Gets the user ID.
        /// </summary>
        public int UserID { get; init; }

        /// <summary>
        /// Gets the user role ID.
        /// </summary>
        public int RoleID { get; init; }

        /// <summary>
        /// Gets the browser session ID.
        /// </summary>
        public string SessionID { get; init; }

        /// <summary>
        /// Gets the remote IP address.
        /// </summary>
        public string RemoteIP { get; init; }


        /// <summary>
        /// Gets or sets a value indicating whether the user is valid and able to login.
        /// </summary>
        public bool UserIsValid { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the error message displayed to the user.
        /// </summary>
        public string FriendlyError { get; set; }
    }
}
