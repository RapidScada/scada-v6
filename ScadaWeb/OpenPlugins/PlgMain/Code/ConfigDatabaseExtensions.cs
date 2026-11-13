// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Entities;
using Scada.Data.Tables;
using Scada.Lang;
using Scada.Web.Users;

namespace Scada.Web.Plugins.PlgMain.Code
{
    /// <summary>
    /// The class provides extension methods for the configuration database.
    /// <para>Класс, предоставляющий методы расширения для базы конфигурации.</para>
    /// </summary>
    internal static class ConfigDatabaseExtensions
    {
        /// <summary>
        /// Gets the numbers of available objects starting from the specified object.
        /// </summary>
        public static List<int> GetObjNumHierarchy(this ConfigDatabase configDatabase, int startObjNum,
            UserRights userRights)
        {
            ArgumentNullException.ThrowIfNull(configDatabase);

            if (!configDatabase.ObjTable.TryGetIndex("ParentObjNum", out ITableIndex parentObjIndex))
            {
                throw new ScadaException(CommonPhrases.IndexNotFound);
            }

            List<int> objNums = [];

            void AddObjectRecursively(int objNum)
            {
                if (userRights.GetRightByObj(objNum).View)
                {
                    objNums.Add(objNum);

                    foreach (Obj childObj in parentObjIndex.SelectItems(objNum))
                    {
                        AddObjectRecursively(childObj.ObjNum);
                    }
                }
            }

            AddObjectRecursively(startObjNum);
            return objNums;
        }
    }
}
