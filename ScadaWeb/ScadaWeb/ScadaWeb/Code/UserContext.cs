/*
 * Copyright 2021 Rapid Software LLC
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
 * Module   : Webstation Application
 * Summary  : Contains user data
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Scada.Data.Entities;

namespace Scada.Web.Code
{
    /// <summary>
    /// Contains user data.
    /// <para>Содержит данные пользователя.</para>
    /// </summary>
    internal class UserContext : IUserContext
    {
        public bool IsLoggedIn { get; set; }

        public User UserModel { get; set;  }

        public object Menu => throw new System.NotImplementedException();

        public object Views => throw new System.NotImplementedException();
    }
}
