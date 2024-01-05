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
 * Summary  : Represents a list of objects available to a user
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2022
 * Modified : 2023
 */

using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Lang;

namespace Scada.Web.Users
{
    /// <summary>
    /// Represents a list of objects available to a user.
    /// <para>Представляет список объектов, доступных пользователю.</para>
    /// </summary>
    /// <remarks>
    /// Includes objects to which a user has rights and their parent objects.
    /// </remarks>
    public class UserObjects : List<ObjectItem>
    {
        /// <summary>
        /// Adds the child objects of the specified object recursively.
        /// </summary>
        private void AddChildObjects(ITableIndex parentObjIndex, int parentObjNum, int parentLevel,
            UserRights userRights)
        {
            foreach (Obj childObj in parentObjIndex.SelectItems(parentObjNum))
            {
                Right right = userRights.GetRightByObj(childObj.ObjNum);

                if (right.List)
                {
                    Add(new ObjectItem(childObj, right, parentLevel + 1));
                    AddChildObjects(parentObjIndex, childObj.ObjNum, parentLevel + 1, userRights);
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

                AddChildObjects(parentObjIndex, 0, -1, userRights);
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
