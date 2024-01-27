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
 * Summary  : Specifies the authorization policy names
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2022
 */

namespace Scada.Web.Authorization
{
    /// <summary>
    /// Specifies the authorization policy names.
    /// <para>Задает имена политик авторизации.</para>
    /// </summary>
    public static class PolicyName
    {
        /// <summary>
        /// Only administrators are allowed.
        /// </summary>
        public const string Administrators = nameof(Administrators);

        /// <summary>
        /// Users must have rights to view data of any object.
        /// </summary>
        public const string RequireViewAll = nameof(RequireViewAll);

        /// <summary>
        /// User access is restricted according to user rights specified in the configuration database.
        /// </summary>
        public const string Restricted = nameof(Restricted);
    }
}
