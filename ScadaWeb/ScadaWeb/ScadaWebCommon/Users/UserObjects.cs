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
 * Summary  : Represents a list of objects available to a user
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2022
 * Modified : 2022
 */

using Scada.Data.Entities;
using Scada.Data.Tables;
using Scada.Lang;
using System;
using System.Collections.Generic;

namespace Scada.Web.Users
{
    /// <summary>
    /// Represents a list of objects available to a user.
    /// <para>Представляет список объектов, доступных пользователю.</para>
    /// </summary>
    public class UserObjects : List<ObjectItem>
    {
        /// <summary>
        /// Adds the child objects of the specified object recursively.
        /// </summary>
        private void AddChildObjects(ITableIndex parentObjIndex, UserRights userRights,
            int parentObjNum, int parentLevel)
        {
            foreach (Obj childObj in parentObjIndex.SelectItems(parentObjNum))
            {
                if (userRights.GetRightByObj(childObj.ObjNum).View)
                {
                    Add(new ObjectItem(childObj, parentLevel + 1));
                    AddChildObjects(parentObjIndex, userRights, childObj.ObjNum, parentLevel + 1);
                }
            }
        }

        /// <summary>
        /// Initializes the list of objects according to the user rights.
        /// </summary>
        public void Init(BaseTable<Obj> objTable, UserRights userRights)
        {
            ArgumentNullException.ThrowIfNull(objTable, nameof(objTable));
            ArgumentNullException.ThrowIfNull(userRights, nameof(userRights));

            try
            {
                if (!objTable.TryGetIndex("ParentObjNum", out ITableIndex parentObjIndex))
                    throw new ScadaException(CommonPhrases.IndexNotFound);

                foreach (Obj obj in objTable.Enumerate())
                {
                    if (obj.ParentObjNum == null && userRights.GetRightByObj(obj.ObjNum).View)
                    {
                        Add(new ObjectItem(obj, 0));
                        AddChildObjects(parentObjIndex, userRights, obj.ObjNum, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ScadaException(Locale.IsRussian ?
                    "Ошибка при инициализации списка объектов" :
                    "Error initializing object list", ex);
            }
        }
    }
}
