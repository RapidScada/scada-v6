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
 * Module   : ScadaServerEngine
 * Summary  : Represents access control for objects
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2021
 */

using Scada.Data.Const;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Data.Tables;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Scada.Server.Engine
{
    /// <summary>
    /// Represents access control for objects.
    /// <para>Представляет управление доступом для объектов.</para>
    /// </summary>
    internal class ObjSecurity
    {
        /// <summary>
        /// Represents access rights by objects.
        /// </summary>
        private class RightByObj : Dictionary<int, Right> { }

        private Dictionary<int, RightByObj> rightMatrix; // the rights, key is a role ID


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ObjSecurity()
        {
            rightMatrix = null;
        }


        /// <summary>
        /// Enumerates parent role IDs recursively.
        /// </summary>
        private IEnumerable<int> EnumerateParentRoleIDs(TableIndex roleRef_childRoleIndex, int childRoleID, 
            HashSet<int> protectionSet = null)
        {
            if (protectionSet == null)
                protectionSet = new HashSet<int> { childRoleID };

            foreach (int parentRoleID in roleRef_childRoleIndex.SelectItems(childRoleID))
            {
                if (protectionSet.Add(parentRoleID))
                {
                    yield return parentRoleID;

                    foreach (int grandparentRoleID in 
                        EnumerateParentRoleIDs(roleRef_childRoleIndex, parentRoleID, protectionSet))
                    {
                        yield return grandparentRoleID;
                    }
                }
            }
        }
        
        /// <summary>
        /// Enumerates child objects recursively.
        /// </summary>
        private IEnumerable<Obj> EnumerateChildObjects(TableIndex obj_parentObjIndex, int parentObjNum, 
            HashSet<int> protectionSet = null)
        {
            if (protectionSet == null)
                protectionSet = new HashSet<int> { parentObjNum };

            foreach (Obj childObj in obj_parentObjIndex.SelectItems(parentObjNum))
            {
                if (protectionSet.Add(childObj.ObjNum))
                {
                    yield return childObj;

                    foreach (Obj grandchildObj in EnumerateChildObjects(obj_parentObjIndex, childObj.ObjNum))
                    {
                        yield return grandchildObj;
                    }
                }
            }
        }

        /// <summary>
        /// Add rights for the specified role.
        /// </summary>
        private void AddRoleRight(TableIndex objRight_roleIndex, TableIndex obj_parentObjIndex, 
            RightByObj rightByObj, int roleID)
        {
            // explicitly defined rights have higher priority
            foreach (ObjRight objRight in objRight_roleIndex.SelectItems(roleID))
            {
                AddObjRight(rightByObj, objRight.ObjNum, new Right(objRight));
            }

            // add rights on child objects
            foreach (ObjRight objRight in objRight_roleIndex.SelectItems(roleID))
            {
                Right right = new Right(objRight);

                foreach (Obj childObj in EnumerateChildObjects(obj_parentObjIndex, objRight.ObjNum))
                {
                    AddObjRight(rightByObj, childObj.ObjNum, right);
                }
            }
        }

        /// <summary>
        /// Add rights for the specified object.
        /// </summary>
        private void AddObjRight(RightByObj rightByObj, int objNum, Right right)
        {
            if (!rightByObj.ContainsKey(objNum))
                rightByObj.Add(objNum, right);
        }


        /// <summary>
        /// Initializes the access control.
        /// </summary>
        public void Init(BaseDataSet baseDataSet)
        {
            if (baseDataSet == null)
                throw new ArgumentNullException(nameof(baseDataSet));

            // initialize rights matrix
            rightMatrix = new Dictionary<int, RightByObj>(baseDataSet.RoleTable.ItemCount);

            // create indexes
            TableIndex roleRef_childRoleIndex = new TableIndex("ChildRoleID", typeof(RoleRef));
            roleRef_childRoleIndex.AddRangeToIndex(baseDataSet.RoleRefTable.Items);

            TableIndex objRight_roleIndex = new TableIndex("RoleID", typeof(ObjRight));
            objRight_roleIndex.AddRangeToIndex(baseDataSet.ObjRightTable.Items);

            TableIndex obj_parentObjIndex = new TableIndex("ParentObjNum", typeof(Obj));
            obj_parentObjIndex.AddRangeToIndex(baseDataSet.ObjTable.Items);

            // fill rights
            foreach (Role role in baseDataSet.RoleTable.EnumerateItems())
            {
                int roleID = role.RoleID;
                RightByObj rightByObj = new RightByObj();
                rightMatrix.Add(roleID, rightByObj);
                AddRoleRight(objRight_roleIndex, obj_parentObjIndex, rightByObj, roleID);

                foreach (int parentRoleID in EnumerateParentRoleIDs(roleRef_childRoleIndex, roleID))
                {
                    AddRoleRight(objRight_roleIndex, obj_parentObjIndex, rightByObj, parentRoleID);
                }
            }
        }

        /// <summary>
        /// Gets the rights of the role on the object.
        /// </summary>
        public Right GetRight(int roleID, int objID)
        {
            switch (roleID)
            {
                case RoleID.Disabled:
                    return Right.Empty;
                case RoleID.Administrator:
                    return Right.Full;
                case RoleID.Dispatcher:
                    return Right.Full;
                case RoleID.Guest:
                    return new Right(true, false);
                case RoleID.Application:
                    return Right.Full;
            }

            return rightMatrix != null &&
                rightMatrix.TryGetValue(roleID, out RightByObj rightsByObj) && 
                rightsByObj.TryGetValue(objID, out Right right) ? right : Right.Empty;
        }
    }
}
