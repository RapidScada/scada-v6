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
 * Module   : Administrator
 * Summary  : Creates columns for a DataGridView control
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2019
 */

using Scada.Admin.Project;
using Scada.Data.Const;
using Scada.Data.Entities;
using Scada.Data.Tables;
using Scada.Lang;
using System;
using System.Data;
using System.Windows.Forms;

namespace Scada.Admin.App.Code
{
    /// <summary>
    /// Creates columns for a DataGridView control.
    /// <para>Создает столбцы для элемента управления DataGridView.</para>
    /// </summary>
    internal static class ColumnBuilder
    {
        /// <summary>
        /// Creates a new column that hosts text cells.
        /// </summary>
        private static DataGridViewColumn NewTextBoxColumn(string dataPropertyName, ColumnOptions options = null)
        {
            DataGridViewTextBoxColumn column = new()
            {
                Name = dataPropertyName,
                HeaderText = dataPropertyName,
                DataPropertyName = dataPropertyName,
                Tag = options
            };

            if (options != null && options.MaxLength > 0)
                column.MaxInputLength = options.MaxLength;

            return column;
        }

        /// <summary>
        /// Creates a new column that hosts cells with checkboxes.
        /// </summary>
        private static DataGridViewColumn NewCheckBoxColumn(string dataPropertyName, ColumnOptions options = null)
        {
            return new DataGridViewCheckBoxColumn
            {
                Name = dataPropertyName,
                HeaderText = dataPropertyName,
                DataPropertyName = dataPropertyName,
                Tag = options,
                SortMode = DataGridViewColumnSortMode.Automatic
            };
        }

        /// <summary>
        /// Creates a new column that hosts cells with buttons.
        /// </summary>
        private static DataGridViewColumn NewButtonColumn(string dataPropertyName, ColumnOptions options = null)
        {
            return new DataGridViewButtonColumn
            {
                Name = dataPropertyName + (options == null ? ColumnKind.Button : options.Kind),
                HeaderText = dataPropertyName,
                DataPropertyName = dataPropertyName,
                Tag = options,
                Text = dataPropertyName,
                UseColumnTextForButtonValue = true
            };
        }

        /// <summary>
        /// Creates a new column that hosts cells which values are selected from a combo box.
        /// </summary>
        private static DataGridViewColumn NewComboBoxColumn(string dataPropertyName, string valueMember, 
            string displayMember, object dataSource, bool addEmptyRow = false, bool prependID = false,
            ColumnOptions options = null)
        {
            if (dataSource is IBaseTable baseTable)
                dataSource = CreateComboBoxSource(baseTable, valueMember, ref displayMember, addEmptyRow, prependID);

            return new DataGridViewComboBoxColumn
            {
                Name = dataPropertyName,
                HeaderText = dataPropertyName,
                DataPropertyName = dataPropertyName,
                ValueMember = valueMember,
                DisplayMember = displayMember,
                DataSource = dataSource,
                Tag = options,
                SortMode = DataGridViewColumnSortMode.Automatic,
                DisplayStyleForCurrentCellOnly = true
            };
        }

        /// <summary>
        /// Creates a new column that hosts cells which values are selected from a combo box.
        /// </summary>
        private static DataGridViewColumn NewComboBoxColumn(string dataPropertyName, 
            string displayMember, IBaseTable dataSource, bool addEmptyRow = false, bool prependID = false, 
            ColumnOptions options = null)
        {
            return NewComboBoxColumn(dataPropertyName, dataPropertyName, 
                displayMember, dataSource, addEmptyRow, prependID, options);
        }

        /// <summary>
        /// Creates a data table for using as a data source of a combo box.
        /// </summary>
        private static DataTable CreateComboBoxSource(
            IBaseTable baseTable, string valueMember, ref string displayMember, bool addEmptyRow, bool prependID)
        {
            DataTable dataTable = baseTable.ToDataTable(true);

            if (prependID)
            {
                // display ID and name
                string columnName = valueMember + "_" + displayMember;
                dataTable.Columns.Add(columnName, typeof(string), 
                    string.Format("'[' + {0} + '] ' + {1}", valueMember, displayMember));
                displayMember = columnName;
                dataTable.DefaultView.Sort = valueMember;
            }
            else
            {
                dataTable.DefaultView.Sort = displayMember;
            }

            if (addEmptyRow)
            {
                DataRow emptyRow = dataTable.NewRow();
                emptyRow[valueMember] = DBNull.Value;
                emptyRow[displayMember] = " ";
                dataTable.Rows.Add(emptyRow);
            }

            return dataTable;
        }

        /// <summary>
        /// Translates the column headers.
        /// </summary>
        private static DataGridViewColumn[] TranslateHeaders(string tableName, DataGridViewColumn[] columns)
        {
            if (Locale.Dictionaries.TryGetValue(typeof(ColumnBuilder).FullName + "." + tableName, 
                out LocaleDict localeDict))
            {
                foreach (DataGridViewColumn col in columns)
                {
                    if (localeDict.Phrases.TryGetValue(col.Name, out string header))
                    {
                        if (col is DataGridViewButtonColumn buttonColumn)
                            buttonColumn.Text = header;
                        col.HeaderText = header;
                    }
                }
            }

            return columns;
        }


        /// <summary>
        /// Creates columns for the archive table.
        /// </summary>
        private static DataGridViewColumn[] CreateArchiveTableColumns()
        {
            return TranslateHeaders("ArchiveTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("ArchiveID", new ColumnOptions(ColumnKind.PrimaryKey)),
                NewTextBoxColumn("Name", new ColumnOptions(ColumnLength.Name)),
                NewTextBoxColumn("Code", new ColumnOptions(ColumnLength.Code)),
                NewCheckBoxColumn("IsDefault"),
                NewTextBoxColumn("Bit", new ColumnOptions(ConfigDatabase.MinBit, ConfigDatabase.MaxBit)),
                NewTextBoxColumn("Descr", new ColumnOptions(ColumnLength.Description))
            });
        }

        /// <summary>
        /// Creates columns for the channel table.
        /// </summary>
        private static DataGridViewColumn[] CreateCnlTableColumns(ConfigDatabase configDatabase)
        {
            return TranslateHeaders("CnlTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("CnlNum", new ColumnOptions(ColumnKind.PrimaryKey)),
                NewCheckBoxColumn("Active", new ColumnOptions { DefaultValue = true }),
                NewTextBoxColumn("Name", new ColumnOptions(ColumnLength.Name)),
                NewComboBoxColumn("DataTypeID", "Name", configDatabase.DataTypeTable, true),
                NewTextBoxColumn("DataLen"),
                NewComboBoxColumn("CnlTypeID", "Name", configDatabase.CnlTypeTable, false, false,
                    new ColumnOptions { DefaultValue = CnlTypeID.Input }),
                NewComboBoxColumn("ObjNum","Name", configDatabase.ObjTable, true),
                NewComboBoxColumn("DeviceNum", "Name", configDatabase.DeviceTable, true),
                NewTextBoxColumn("TagNum"),
                NewTextBoxColumn("TagCode", new ColumnOptions(ColumnLength.Code)),
                NewCheckBoxColumn("FormulaEnabled"),
                NewTextBoxColumn("InFormula", new ColumnOptions(ColumnLength.Default)),
                NewTextBoxColumn("OutFormula", new ColumnOptions(ColumnLength.Default)),
                NewComboBoxColumn("FormatID", "Name", configDatabase.FormatTable, true),
                NewComboBoxColumn("QuantityID", "Name", configDatabase.QuantityTable, true),
                NewComboBoxColumn("UnitID", "Name", configDatabase.UnitTable, true),
                NewComboBoxColumn("LimID", "Name", configDatabase.LimTable, true),
                NewTextBoxColumn("ArchiveMask", new ColumnOptions(ColumnKind.BitMask)
                    { DataSource = AppUtils.GetArchiveBits(configDatabase.ArchiveTable) }),
                NewButtonColumn("ArchiveMask"),
                NewTextBoxColumn("EventMask", new ColumnOptions(ColumnKind.BitMask)
                    { DataSource = AppUtils.GetEventBits() }),
                NewButtonColumn("EventMask")
            });
        }

        /// <summary>
        /// Creates columns for the channel status table.
        /// </summary>
        private static DataGridViewColumn[] CreateCnlStatusTableColumns()
        {
            return TranslateHeaders("CnlStatusTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("CnlStatusID", new ColumnOptions(ColumnKind.PrimaryKey, 0, ConfigDatabase.MaxID)),
                NewTextBoxColumn("Name", new ColumnOptions(ColumnLength.Name)),
                NewTextBoxColumn("MainColor", new ColumnOptions(ColumnKind.Color, ColumnLength.Default)),
                NewButtonColumn("MainColor"),
                NewTextBoxColumn("SecondColor", new ColumnOptions(ColumnKind.Color, ColumnLength.Default)),
                NewButtonColumn("SecondColor"),
                NewTextBoxColumn("BackColor", new ColumnOptions(ColumnKind.Color, ColumnLength.Default)),
                NewButtonColumn("BackColor"),
                NewTextBoxColumn("Severity", new ColumnOptions(Severity.Min, Severity.Max)),
                NewCheckBoxColumn("AckRequired"),
                NewTextBoxColumn("Descr", new ColumnOptions(ColumnLength.Description))
            });
        }

        /// <summary>
        /// Creates columns for the channel type table.
        /// </summary>
        private static DataGridViewColumn[] CreateCnlTypeTableColumns()
        {
            return TranslateHeaders("CnlTypeTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("CnlTypeID", new ColumnOptions(ColumnKind.PrimaryKey)),
                NewTextBoxColumn("Name", new ColumnOptions(ColumnLength.Name)),
                NewTextBoxColumn("Descr", new ColumnOptions(ColumnLength.Description))
            });
        }

        /// <summary>
        /// Creates columns for the communication line table.
        /// </summary>
        private static DataGridViewColumn[] CreateCommLineTableColumns()
        {
            return TranslateHeaders("CommLineTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("CommLineNum", new ColumnOptions(ColumnKind.PrimaryKey)),
                NewTextBoxColumn("Name", new ColumnOptions(ColumnLength.Name)),
                NewTextBoxColumn("Descr", new ColumnOptions(ColumnLength.Description))
            });
        }

        /// <summary>
        /// Creates columns for the data type table.
        /// </summary>
        private static DataGridViewColumn[] CreateDataTypeTableColumns()
        {
            return TranslateHeaders("DataTypeTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("DataTypeID", new ColumnOptions(ColumnKind.PrimaryKey, 0, ConfigDatabase.MaxID)),
                NewTextBoxColumn("Name", new ColumnOptions(ColumnLength.Name)),
                NewTextBoxColumn("Descr", new ColumnOptions(ColumnLength.Description))
            });
        }

        /// <summary>
        /// Creates columns for the devices table.
        /// </summary>
        private static DataGridViewColumn[] CreateDeviceTableColumns(ConfigDatabase configDatabase)
        {
            return TranslateHeaders("DeviceTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("DeviceNum", new ColumnOptions(ColumnKind.PrimaryKey)),
                NewTextBoxColumn("Name", new ColumnOptions(ColumnLength.Name)),
                NewTextBoxColumn("Code", new ColumnOptions(ColumnLength.Code)),
                NewComboBoxColumn("DevTypeID", "Name", configDatabase.DevTypeTable, true),
                NewTextBoxColumn("NumAddress"),
                NewTextBoxColumn("StrAddress", new ColumnOptions(ColumnLength.Default)),
                NewComboBoxColumn("CommLineNum", "Name", configDatabase.CommLineTable, true),
                NewTextBoxColumn("Descr", new ColumnOptions(ColumnLength.Description))
            });
        }

        /// <summary>
        /// Creates columns for the device type table.
        /// </summary>
        private static DataGridViewColumn[] CreateDevTypeTableColumns()
        {
            return TranslateHeaders("DevTypeTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("DevTypeID", new ColumnOptions(ColumnKind.PrimaryKey)),
                NewTextBoxColumn("Name", new ColumnOptions(ColumnLength.Name)),
                NewTextBoxColumn("Driver", new ColumnOptions(ColumnLength.Default)),
                NewTextBoxColumn("Descr", new ColumnOptions(ColumnLength.Description))
            });
        }

        /// <summary>
        /// Creates columns for the format table.
        /// </summary>
        private static DataGridViewColumn[] CreateFormatTableColumns()
        {
            return TranslateHeaders("FormatTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("FormatID", new ColumnOptions(ColumnKind.PrimaryKey)),
                NewTextBoxColumn("Name", new ColumnOptions(ColumnLength.Name)),
                NewTextBoxColumn("Code", new ColumnOptions(ColumnLength.Code)),
                NewCheckBoxColumn("IsNumber"),
                NewCheckBoxColumn("IsEnum"),
                NewCheckBoxColumn("IsDate"),
                NewCheckBoxColumn("IsString"),
                NewTextBoxColumn("Frmt", new ColumnOptions(ColumnKind.MultilineText, ColumnLength.Enumeration)),
                NewButtonColumn("Frmt"),
                NewTextBoxColumn("Descr", new ColumnOptions(ColumnLength.Description))
            });
        }

        /// <summary>
        /// Creates columns for the limit table.
        /// </summary>
        private static DataGridViewColumn[] CreateLimTableColumns()
        {
            return TranslateHeaders("LimTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("LimID", new ColumnOptions(ColumnKind.PrimaryKey)),
                NewTextBoxColumn("Name", new ColumnOptions(ColumnLength.Name)),
                NewCheckBoxColumn("IsBoundToCnl"),
                NewCheckBoxColumn("IsShared"),
                NewTextBoxColumn("LoLo"),
                NewTextBoxColumn("Low"),
                NewTextBoxColumn("High"),
                NewTextBoxColumn("HiHi"),
                NewTextBoxColumn("Deadband")
            });
        }

        /// <summary>
        /// Creates columns for the object table.
        /// </summary>
        private static DataGridViewColumn[] CreateObjTableColumns(ConfigDatabase configDatabase)
        {
            return TranslateHeaders("ObjTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("ObjNum", new ColumnOptions(ColumnKind.PrimaryKey)),
                NewTextBoxColumn("Code", new ColumnOptions(ColumnLength.Code)),
                NewTextBoxColumn("Name", new ColumnOptions(ColumnLength.Name)),
                NewComboBoxColumn("ParentObjNum", "ObjNum", "Name", configDatabase.ObjTable, true),
                NewTextBoxColumn("Descr", new ColumnOptions(ColumnLength.Description))
            });
        }

        /// <summary>
        /// Creates columns for the object right table.
        /// </summary>
        private static DataGridViewColumn[] CreateObjRightTableColumns(ConfigDatabase configDatabase)
        {
            return TranslateHeaders("ObjRightTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("ObjRightID", new ColumnOptions(ColumnKind.PrimaryKey)),
                NewComboBoxColumn("ObjNum","Name", configDatabase.ObjTable),
                NewComboBoxColumn("RoleID", "Name", configDatabase.RoleTable),
                NewCheckBoxColumn("View"),
                NewCheckBoxColumn("Control")
            });
        }

        /// <summary>
        /// Creates columns for the quantity table.
        /// </summary>
        private static DataGridViewColumn[] CreateQuantityTableColumns()
        {
            return TranslateHeaders("QuantityTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("QuantityID", new ColumnOptions(ColumnKind.PrimaryKey)),
                NewTextBoxColumn("Name", new ColumnOptions(ColumnLength.Name)),
                NewTextBoxColumn("Code", new ColumnOptions(ColumnLength.Code)),
                NewTextBoxColumn("Icon", new ColumnOptions(ColumnLength.Default)),
                NewTextBoxColumn("Descr", new ColumnOptions(ColumnLength.Description))
            });
        }

        /// <summary>
        /// Creates columns for the role table.
        /// </summary>
        private static DataGridViewColumn[] CreateRoleTableColumns()
        {
            return TranslateHeaders("RoleTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("RoleID", new ColumnOptions(ColumnKind.PrimaryKey, 0, ConfigDatabase.MaxID)),
                NewTextBoxColumn("Name", new ColumnOptions(ColumnLength.Name)),
                NewTextBoxColumn("Code", new ColumnOptions(ColumnLength.Code)),
                NewTextBoxColumn("Descr", new ColumnOptions(ColumnLength.Description))
            });
        }

        /// <summary>
        /// Creates columns for the role inheritance table.
        /// </summary>
        private static DataGridViewColumn[] CreateRoleRefTableColumns(ConfigDatabase configDatabase)
        {
            return TranslateHeaders("RoleRefTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("RoleRefID", new ColumnOptions(ColumnKind.PrimaryKey)),
                NewComboBoxColumn("ParentRoleID", "RoleID", "Name", configDatabase.RoleTable),
                NewComboBoxColumn("ChildRoleID", "RoleID", "Name", configDatabase.RoleTable)
            });
        }

        /// <summary>
        /// Creates columns for the script table.
        /// </summary>
        private static DataGridViewColumn[] CreateScriptTableColumns()
        {
            return TranslateHeaders("ScriptTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("ScriptID", new ColumnOptions(ColumnKind.PrimaryKey)),
                NewTextBoxColumn("Name", new ColumnOptions(ColumnLength.Name)),
                NewTextBoxColumn("Source", new ColumnOptions(ColumnKind.MultilineText, ColumnLength.SourceCode)),
                NewButtonColumn("Source"),
                NewTextBoxColumn("Descr", new ColumnOptions(ColumnLength.Description))
            });
        }

        /// <summary>
        /// Creates columns for the unit table.
        /// </summary>
        private static DataGridViewColumn[] CreateUnitTableColumns()
        {
            return TranslateHeaders("UnitTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("UnitID", new ColumnOptions(ColumnKind.PrimaryKey)),
                NewTextBoxColumn("Name", new ColumnOptions(ColumnLength.Name)),
                NewTextBoxColumn("Code", new ColumnOptions(ColumnLength.Code)),
                NewTextBoxColumn("Descr", new ColumnOptions(ColumnLength.Description))
            });
        }

        /// <summary>
        /// Creates columns for the user table.
        /// </summary>
        private static DataGridViewColumn[] CreateUserTableColumns(ConfigDatabase configDatabase)
        {
            return TranslateHeaders("UserTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("UserID", new ColumnOptions(ColumnKind.PrimaryKey)),
                NewCheckBoxColumn("Enabled", new ColumnOptions { DefaultValue = true }),
                NewTextBoxColumn("Name", new ColumnOptions(ColumnLength.Name)),
                NewTextBoxColumn("Password", new ColumnOptions(ColumnKind.Password, ColumnLength.Password)),
                NewButtonColumn("Password"),
                NewComboBoxColumn("RoleID", "Name", configDatabase.RoleTable, false, false, 
                    new ColumnOptions { DefaultValue = RoleID.Disabled }),
                NewTextBoxColumn("Descr", new ColumnOptions(ColumnLength.Description))
            });
        }

        /// <summary>
        /// Creates columns for the view table.
        /// </summary>
        private static DataGridViewColumn[] CreateViewTableColumns(ConfigDatabase configDatabase)
        {
            return TranslateHeaders("ViewTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("ViewID", new ColumnOptions(ColumnKind.PrimaryKey)),
                NewTextBoxColumn("Path", new ColumnOptions(ColumnKind.Path, ColumnLength.Long)),
                NewButtonColumn("Path", new ColumnOptions(ColumnKind.SelectFolderButton)),
                NewButtonColumn("Path", new ColumnOptions(ColumnKind.SelectFileButton)),
                NewComboBoxColumn("ViewTypeID", "Name", configDatabase.ViewTypeTable, true),
                NewComboBoxColumn("ObjNum","Name", configDatabase.ObjTable, true),
                NewTextBoxColumn("Args", new ColumnOptions(ColumnLength.Default)),
                NewTextBoxColumn("Title", new ColumnOptions(ColumnLength.Long)),
                NewTextBoxColumn("Ord"),
                NewCheckBoxColumn("Hidden"),
            });
        }

        /// <summary>
        /// Creates columns for the view type table.
        /// </summary>
        private static DataGridViewColumn[] CreateViewTypeTableColumns()
        {
            return TranslateHeaders("ViewTypeTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("ViewTypeID", new ColumnOptions(ColumnKind.PrimaryKey)),
                NewTextBoxColumn("Name", new ColumnOptions(ColumnLength.Name)),
                NewTextBoxColumn("Code", new ColumnOptions(ColumnLength.Code)),
                NewTextBoxColumn("FileExt", new ColumnOptions(ColumnLength.Default)),
                NewTextBoxColumn("Descr", new ColumnOptions(ColumnLength.Description))
            });
        }


        /// <summary>
        /// Creates columns for the specified table
        /// </summary>
        public static DataGridViewColumn[] CreateColumns(ConfigDatabase configDatabase, Type itemType)
        {
            if (configDatabase == null)
                throw new ArgumentNullException(nameof(configDatabase));

            if (itemType == typeof(Archive))
                return CreateArchiveTableColumns();
            else if (itemType == typeof(Cnl))
                return CreateCnlTableColumns(configDatabase);
            else if (itemType == typeof(CnlStatus))
                return CreateCnlStatusTableColumns();
            else if (itemType == typeof(CnlType))
                return CreateCnlTypeTableColumns();
            else if (itemType == typeof(CommLine))
                return CreateCommLineTableColumns();
            else if (itemType == typeof(DataType))
                return CreateDataTypeTableColumns();
            else if (itemType == typeof(Device))
                return CreateDeviceTableColumns(configDatabase);
            else if (itemType == typeof(DevType))
                return CreateDevTypeTableColumns();
            else if (itemType == typeof(Format))
                return CreateFormatTableColumns();
            else if (itemType == typeof(Lim))
                return CreateLimTableColumns();
            else if (itemType == typeof(Obj))
                return CreateObjTableColumns(configDatabase);
            else if (itemType == typeof(ObjRight))
                return CreateObjRightTableColumns(configDatabase);
            else if (itemType == typeof(Quantity))
                return CreateQuantityTableColumns();
            else if (itemType == typeof(Role))
                return CreateRoleTableColumns();
            else if (itemType == typeof(RoleRef))
                return CreateRoleRefTableColumns(configDatabase);
            else if (itemType == typeof(Script))
                return CreateScriptTableColumns();
            else if (itemType == typeof(Unit))
                return CreateUnitTableColumns();
            else if (itemType == typeof(User))
                return CreateUserTableColumns(configDatabase);
            else if (itemType == typeof(Data.Entities.View))
                return CreateViewTableColumns(configDatabase);
            else if (itemType == typeof(ViewType))
                return CreateViewTypeTableColumns();
            else
                return Array.Empty<DataGridViewColumn>();
        }
    }
}
