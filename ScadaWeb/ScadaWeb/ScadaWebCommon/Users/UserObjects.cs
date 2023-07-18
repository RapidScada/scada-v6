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
 * Summary  : Represents a list of objects available to a user
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2022
 * Modified : 2023
 */

using Scada.Data.Entities;
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
        /// Recursively adds the child objects of the specified object without checking rights.
        /// </summary>
        private void AddChildObjects(ITableIndex parentObjIndex, int parentObjNum, int parentLevel)
        {
            foreach (Obj childObj in parentObjIndex.SelectItems(parentObjNum))
            {
                Add(new ObjectItem(childObj, parentLevel + 1));
                AddChildObjects(parentObjIndex, childObj.ObjNum, parentLevel + 1);
            }
        }

        /// <summary>
        /// Recursively adds the child objects of the specified object with rights check.
        /// </summary>
        private void AddChildObjects(ITableIndex parentObjIndex, int parentObjNum, int parentLevel, 
            HashSet<int> availableObjNums)
        {
            foreach (Obj childObj in parentObjIndex.SelectItems(parentObjNum))
            {
                if (availableObjNums.Contains(childObj.ObjNum))
                {
                    Add(new ObjectItem(childObj, parentLevel + 1));
                    AddChildObjects(parentObjIndex, childObj.ObjNum, parentLevel + 1, availableObjNums);
                }
            }
        }

        /// <summary>
        /// Gets a set of object numbers available to the user, including parent object numbers.
        /// </summary>
        private static HashSet<int> GetAvailableObjNums(BaseTable<Obj> objTable, UserRights userRights)
        {
            HashSet<int> availableObjNums = new();

            foreach (int objNum in userRights.GetAvailableObjs())
            {
                int childObjNum = objNum;
                availableObjNums.Add(childObjNum);

                while (objTable.GetItem(childObjNum) is Obj childObj && childObj.ParentObjNum != null)
                {
                    childObjNum = childObj.ParentObjNum.Value;
                    availableObjNums.Add(childObjNum);
                }
            }

            return availableObjNums;
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
                IEnumerable<Obj> topLevelObjects = objTable.Where(o => o.ParentObjNum == null);

                if (!objTable.TryGetIndex("ParentObjNum", out ITableIndex parentObjIndex))
                    throw new ScadaException(CommonPhrases.IndexNotFound);

                if (userRights.ViewAll)
                {
                    // all objects
                    foreach (Obj obj in topLevelObjects)
                    {
                        Add(new ObjectItem(obj, 0));
                        AddChildObjects(parentObjIndex, obj.ObjNum, 0);
                    }
                }
                else
                {
                    // available objects and their parents
                    HashSet<int> availableObjNums = GetAvailableObjNums(objTable, userRights);

                    foreach (Obj obj in topLevelObjects)
                    {
                        if (availableObjNums.Contains(obj.ObjNum))
                        {
                            Add(new ObjectItem(obj, 0));
                            AddChildObjects(parentObjIndex, obj.ObjNum, 0, availableObjNums);
                        }
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
