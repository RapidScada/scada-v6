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
 * Module   : ScadaAdminCommon
 * Summary  : Represents a configuration database of a project
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2021
 */

using Scada.Admin.Lang;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Lang;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Scada.Admin.Project
{
    /// <summary>
    /// Represents a configuration database of a project.
    /// <para>Представляет базу конфигурации проекта.</para>
    /// </summary>
    public class ConfigBase : BaseDataSet
    {
        /// <summary>
        /// The minimum ID value.
        /// </summary>
        public const int MinID = 1;
        /// <summary>
        /// The maximum ID value.
        /// </summary>
        public const int MaxID = int.MaxValue;
        /// <summary>
        /// The minimum bit number in a mask.
        /// </summary>
        public const int MinBit = 0;
        /// <summary>
        /// The maximum bit number in a mask.
        /// </summary>
        public const int MaxBit = 30;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ConfigBase()
            : base()
        {
            BaseDir = "";
            Loaded = false;
            AddIndexes();
        }


        /// <summary>
        /// Gets or sets the directory of the configuration database.
        /// </summary>
        public string BaseDir { get; set; }

        /// <summary>
        /// Gets a value indicating whether the tables are loaded.
        /// </summary>
        public bool Loaded { get; private set; }

        /// <summary>
        /// Gets a value indicating whether at least one table was modified.
        /// </summary>
        public bool Modified
        {
            get
            {
                return AllTables.Any(t => t.Modified);
            }
        }


        /// <summary>
        /// Adds the necessary indexes to the tables.
        /// </summary>
        private void AddIndexes()
        {
            FormatTable.AddIndex("Code");
            QuantityTable.AddIndex("Code");
            UnitTable.AddIndex("Code");
        }

        /// <summary>
        /// Gets the item with the specified code from the table.
        /// </summary>
        private static T GetItemByCode<T>(IBaseTable baseTable, string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return default;
            }
            else if (baseTable.TryGetIndex("Code", out ITableIndex index))
            {
                foreach (object item in index.SelectItems(code))
                {
                    return (T)item;
                }

                return default;
            }
            else
            {
                throw new ScadaException(CommonPhrases.IndexNotFound);
            }
        }


        /// <summary>
        /// Gets the format with the specified code, or null if it not found.
        /// </summary>
        public Format GetFormatByCode(string code)
        {
            return GetItemByCode<Format>(FormatTable, code);
        }

        /// <summary>
        /// Gets the quantity with the specified code, or null if it not found.
        /// </summary>
        public Quantity GetQuantityByCode(string code)
        {
            return GetItemByCode<Quantity>(QuantityTable, code);
        }

        /// <summary>
        /// Gets the quantity with the specified code, or null if it not found.
        /// </summary>
        public Unit GetUnitByCode(string code)
        {
            return GetItemByCode<Unit>(UnitTable, code);
        }

        /// <summary>
        /// Gets the table of the configuration database by the specified item type.
        /// </summary>
        public IBaseTable GetTable(Type itemType)
        {
            foreach (IBaseTable baseTable in AllTables)
            {
                if (baseTable.ItemType == itemType)
                    return baseTable;
            }

            return null;
        }


        /// <summary>
        /// Loads the configuration database if needed.
        /// </summary>
        public bool Load(out string errMsg)
        {
            try
            {
                if (!Loaded)
                {
                    foreach (IBaseTable baseTable in AllTables)
                    {
                        string fileName = Path.Combine(BaseDir, baseTable.FileName);

                        if (File.Exists(fileName))
                        {
                            try
                            {
                                baseTable.Load(fileName);
                            }
                            catch (Exception ex)
                            {
                                throw new ScadaException(string.Format(
                                    AdminPhrases.LoadBaseTableError, baseTable.Title), ex);
                            }
                        }
                    }

                    Loaded = true;
                }

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.BuildErrorMessage(AdminPhrases.LoadConfigBaseError);
                return false;
            }
        }

        /// <summary>
        /// Saves all modified tables of the configuration database.
        /// </summary>
        public bool Save(out string errMsg)
        {
            try
            {
                Directory.CreateDirectory(BaseDir);

                foreach (IBaseTable baseTable in AllTables)
                {
                    if (baseTable.Modified)
                    {
                        try
                        {
                            string fileName = Path.Combine(BaseDir, baseTable.FileName);
                            baseTable.Save(fileName);
                        }
                        catch (Exception ex)
                        {
                            throw new ScadaException(string.Format(
                                AdminPhrases.SaveBaseTableError, baseTable.Title), ex);
                        }
                    }
                }

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.BuildErrorMessage(AdminPhrases.SaveConfigBaseError);
                return false;
            }
        }

        /// <summary>
        /// Saves the specified table of the configuration database.
        /// </summary>
        public bool SaveTable(IBaseTable baseTable, out string errMsg)
        {
            try
            {
                Directory.CreateDirectory(BaseDir);
                string fileName = Path.Combine(BaseDir, baseTable.FileName);
                baseTable.Save(fileName);
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.BuildErrorMessage(AdminPhrases.SaveBaseTableError, baseTable.Title);
                return false;
            }
        }
    }
}
