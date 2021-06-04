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
 * Module   : ScadaWebCommon
 * Summary  : Contains information about user rights
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2021
 */

using Scada.Data.Models;
using Scada.Lang;
using System;

namespace Scada.Web.Users
{
    /// <summary>
    /// Contains information about user rights.
    /// <para>Содержит информацию о правах пользователя.</para>
    /// </summary>
    public class UserRights
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public UserRights()
        {

        }

        /// <summary>
        /// Initializes the user rights.
        /// </summary>
        public void Init(BaseDataSet baseDataSet, int roleID)
        {
            if (baseDataSet == null)
                throw new ArgumentNullException(nameof(baseDataSet));

            try
            {
            }
            catch (Exception ex)
            {
                throw new ScadaException(Locale.IsRussian ?
                    "Ошибка при инициализации прав пользователя" :
                    "Error initializing user rights", ex);
            }
        }
    }
}
