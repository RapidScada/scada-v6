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
 * Module   : ScadaCommon
 * Summary  : Represents access rights structured by roles and objects
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2022
 */

using Scada.Data.Entities;
using Scada.Data.Tables;
using Scada.Lang;
using System;
using System.Collections.Generic;

namespace Scada.Data.Models
{
    /// <summary>
    /// Represents access rights structured by roles and objects.
    /// <para>Представляет права доступа, структурированные по ролям и объектам.</para>
    /// </summary>
    public class RightMatrix
    {
        /// <summary>
        /// Represents rights accessed by object.
        /// </summary>
        public class RightByObj : Dictionary<int, Right> { }


        /// <summary>
        /// Gets or sets the rights. Dictionary key is a role ID.
        /// </summary>
        protected Dictionary<int, RightByObj> Matrix { get; set; }


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public RightMatrix()
        {
            Matrix = null;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public RightMatrix(ConfigDataset configDataset)
        {
            Init(configDataset);
        }


        /// <summary>
        /// Enumerates parent role IDs recursively.
        /// </summary>
        protected IEnumerable<int> EnumerateParentRoleIDs(ITableIndex roleRef_childRoleIndex, int childRoleID,
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
        protected IEnumerable<Obj> EnumerateChildObjects(ITableIndex obj_parentObjIndex, int parentObjNum,
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
        protected void AddRoleRight(ITableIndex objRight_roleIndex, ITableIndex obj_parentObjIndex,
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
        protected void AddObjRight(RightByObj rightByObj, int objNum, Right right)
        {
            if (!rightByObj.ContainsKey(objNum))
                rightByObj.Add(objNum, right);
        }


        /// <summary>
        /// Initializes the access rights.
        /// </summary>
        public void Init(ConfigDataset configDataset)
        {
            if (configDataset == null)
                throw new ArgumentNullException(nameof(configDataset));

            // initialize rights matrix
            Matrix = new Dictionary<int, RightByObj>(configDataset.RoleTable.ItemCount);

            // get indexes
            if (!configDataset.RoleRefTable.TryGetIndex("ChildRoleID", out ITableIndex roleRef_childRoleIndex))
                throw new ScadaException(CommonPhrases.IndexNotFound);

            if (!configDataset.ObjRightTable.TryGetIndex("RoleID", out ITableIndex objRight_roleIndex))
                throw new ScadaException(CommonPhrases.IndexNotFound);

            if (!configDataset.ObjTable.TryGetIndex("ParentObjNum", out ITableIndex obj_parentObjIndex))
                throw new ScadaException(CommonPhrases.IndexNotFound);

            // fill rights
            foreach (Role role in configDataset.RoleTable.EnumerateItems())
            {
                int roleID = role.RoleID;
                RightByObj rightByObj = new RightByObj();
                Matrix.Add(roleID, rightByObj);
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
        public virtual Right GetRight(int roleID, int objID)
        {
            return Matrix != null && 
                Matrix.TryGetValue(roleID, out RightByObj rightByObj) && 
                rightByObj.TryGetValue(objID, out Right right)
                ? right 
                : Right.Empty;
        }

        /// <summary>
        /// Gets a dictionary of object rights for the specified role.
        /// </summary>
        public RightByObj GetRightByObj(int roleID)
        {
            return Matrix != null && Matrix.TryGetValue(roleID, out RightByObj rightByObj)
                ? rightByObj
                : null;
        }
    }
}
