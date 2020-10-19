/*
 * Copyright 2020 Mikhail Shiryaev
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
 * Modified : 2020
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
        private class RightsByObj : Dictionary<int, EntityRights> { }

        private Dictionary<int, RightsByObj> rightsMatrix; // the rights, key is a role ID


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ObjSecurity()
        {
            rightsMatrix = null;
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
        private void AddRoleRights(TableIndex objRight_roleIndex, TableIndex obj_parentObjIndex, 
            RightsByObj rightsByObj, int roleID)
        {
            // explicitly defined rights have higher priority
            foreach (ObjRight objRight in objRight_roleIndex.SelectItems(roleID))
            {
                AddObjRights(rightsByObj, objRight.ObjNum, new EntityRights(objRight));
            }

            // add rights on child objects
            foreach (ObjRight objRight in objRight_roleIndex.SelectItems(roleID))
            {
                EntityRights entityRights = new EntityRights(objRight);

                foreach (Obj childObj in EnumerateChildObjects(obj_parentObjIndex, objRight.ObjNum))
                {
                    AddObjRights(rightsByObj, childObj.ObjNum, entityRights);
                }
            }
        }

        /// <summary>
        /// Add rights for the specified object.
        /// </summary>
        private void AddObjRights(RightsByObj rightsByObj, int objNum, EntityRights entityRights)
        {
            if (!rightsByObj.ContainsKey(objNum))
                rightsByObj.Add(objNum, entityRights);
        }


        /// <summary>
        /// Initializes the access control.
        /// </summary>
        public void Init(BaseDataSet baseDataSet)
        {
            if (baseDataSet == null)
                throw new ArgumentNullException(nameof(baseDataSet));

            // initialize rights matrix
            rightsMatrix = new Dictionary<int, RightsByObj>(baseDataSet.RoleTable.ItemCount);

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
                RightsByObj rightsByObj = new RightsByObj();
                rightsMatrix.Add(roleID, rightsByObj);
                AddRoleRights(objRight_roleIndex, obj_parentObjIndex, rightsByObj, roleID);

                foreach (int parentRoleID in EnumerateParentRoleIDs(roleRef_childRoleIndex, roleID))
                {
                    AddRoleRights(objRight_roleIndex, obj_parentObjIndex, rightsByObj, parentRoleID);
                }
            }
        }

        /// <summary>
        /// Gets the rights of the role on the object.
        /// </summary>
        public EntityRights GetRights(int roleID, int objID)
        {
            switch (roleID)
            {
                case RoleID.Disabled:
                    return EntityRights.NoRights;
                case RoleID.Administrator:
                    return EntityRights.FullRights;
                case RoleID.Dispatcher:
                    return EntityRights.FullRights;
                case RoleID.Guest:
                    return new EntityRights(true, false);
                case RoleID.Application:
                    return EntityRights.FullRights;
            }

            return rightsMatrix != null &&
                rightsMatrix.TryGetValue(roleID, out RightsByObj rightsByObj) && 
                rightsByObj.TryGetValue(objID, out EntityRights rights) ? rights : EntityRights.NoRights;
        }
    }
}
