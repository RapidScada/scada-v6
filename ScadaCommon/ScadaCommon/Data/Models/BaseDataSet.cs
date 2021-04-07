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
 * Module   : ScadaCommon
 * Summary  : Represents an in-memory cache of the configuration database
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Data.Entities;
using Scada.Data.Tables;
using Scada.Lang;

namespace Scada.Data.Models
{
    /// <summary>
    /// Represents an in-memory cache of the configuration database.
    /// <para>Представляет кэш базы конфигурации.</para>
    /// </summary>
    public class BaseDataSet
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public BaseDataSet()
        {
            CreateAllTables();
            AddRelations();
        }


        /// <summary>
        /// Gets the archive table.
        /// </summary>
        public BaseTable<Archive> ArchiveTable { get; protected set; }

        /// <summary>
        /// Gets the command type table.
        /// </summary>
        public BaseTable<CmdType> CmdTypeTable { get; protected set; }

        /// <summary>
        /// Gets the input channel status table.
        /// </summary>
        public BaseTable<CnlStatus> CnlStatusTable { get; protected set; }

        /// <summary>
        /// Gets the input channel type table.
        /// </summary>
        public BaseTable<CnlType> CnlTypeTable { get; protected set; }

        /// <summary>
        /// Gets the communication line table.
        /// </summary>
        public BaseTable<CommLine> CommLineTable { get; protected set; }

        /// <summary>
        /// Gets the data type table.
        /// </summary>
        public BaseTable<DataType> DataTypeTable { get; protected set; }

        /// <summary>
        /// Gets the device table.
        /// </summary>
        public BaseTable<Device> DeviceTable { get; protected set; }

        /// <summary>
        /// Gets the device type table.
        /// </summary>
        public BaseTable<DevType> DevTypeTable { get; protected set; }

        /// <summary>
        /// Gets the format table.
        /// </summary>
        public BaseTable<Format> FormatTable { get; protected set; }

        /// <summary>
        /// Gets the input channel table.
        /// </summary>
        public BaseTable<InCnl> InCnlTable { get; protected set; }

        /// <summary>
        /// Gets the limit table.
        /// </summary>
        public BaseTable<Lim> LimTable { get; protected set; }

        /// <summary>
        /// Gets the object (location) table.
        /// </summary>
        public BaseTable<Obj> ObjTable { get; protected set; }

        /// <summary>
        /// Gets the object right table.
        /// </summary>
        public BaseTable<ObjRight> ObjRightTable { get; protected set; }

        /// <summary>
        /// Gets the output channel table.
        /// </summary>
        public BaseTable<OutCnl> OutCnlTable { get; protected set; }

        /// <summary>
        /// Gets the quantity table.
        /// </summary>
        public BaseTable<Quantity> QuantityTable { get; protected set; }

        /// <summary>
        /// Gets the role table.
        /// </summary>
        public BaseTable<Role> RoleTable { get; protected set; }

        /// <summary>
        /// Gets the role inheritance table.
        /// </summary>
        public BaseTable<RoleRef> RoleRefTable { get; protected set; }

        /// <summary>
        /// Gets the script table.
        /// </summary>
        public BaseTable<Script> ScriptTable { get; protected set; }

        /// <summary>
        /// Gets the unit table.
        /// </summary>
        public BaseTable<Unit> UnitTable { get; protected set; }

        /// <summary>
        /// Gets the user table.
        /// </summary>
        public BaseTable<User> UserTable { get; protected set; }

        /// <summary>
        /// Gets the view table.
        /// </summary>
        public BaseTable<View> ViewTable { get; protected set; }

        /// <summary>
        /// Gets the view type table.
        /// </summary>
        public BaseTable<ViewType> ViewTypeTable { get; protected set; }

        /// <summary>
        /// Gets all the tables of the configuration database.
        /// </summary>
        public IBaseTable[] AllTables { get; protected set; }


        /// <summary>
        /// Creates all tables of the configuration database.
        /// </summary>
        protected void CreateAllTables()
        {
            AllTables = new IBaseTable[]
            {
                ArchiveTable = new BaseTable<Archive>("Archive", "ArchiveID", CommonPhrases.ArchiveTable),
                CmdTypeTable = new BaseTable<CmdType>("CmdType", "CmdTypeID", CommonPhrases.CmdTypeTable),
                CnlStatusTable = new BaseTable<CnlStatus>("CnlStatus", "CnlStatusID", CommonPhrases.CnlStatusTable),
                CnlTypeTable = new BaseTable<CnlType>("CnlType", "CnlTypeID", CommonPhrases.CnlTypeTable),
                CommLineTable = new BaseTable<CommLine>("CommLine", "CommLineNum", CommonPhrases.CommLineTable),
                DataTypeTable = new BaseTable<DataType>("DataType", "DataTypeID", CommonPhrases.DataTypeTable),
                DeviceTable = new BaseTable<Device>("Device", "DeviceNum", CommonPhrases.DeviceTable),
                DevTypeTable = new BaseTable<DevType>("DevType", "DevTypeID", CommonPhrases.DevTypeTable),
                FormatTable = new BaseTable<Format>("Format", "FormatID", CommonPhrases.FormatTable),
                InCnlTable = new BaseTable<InCnl>("InCnl", "CnlNum", CommonPhrases.InCnlTable),
                LimTable = new BaseTable<Lim>("Lim", "LimID", CommonPhrases.LimTable),
                ObjTable = new BaseTable<Obj>("Obj", "ObjNum", CommonPhrases.ObjTable),
                ObjRightTable = new BaseTable<ObjRight>("ObjRight", "ObjRightID", CommonPhrases.ObjRightTable),
                OutCnlTable = new BaseTable<OutCnl>("OutCnl", "OutCnlNum", CommonPhrases.OutCnlTable),
                QuantityTable = new BaseTable<Quantity>("Quantity", "QuantityID", CommonPhrases.QuantityTable),
                RoleTable = new BaseTable<Role>("Role", "RoleID", CommonPhrases.RoleTable),
                RoleRefTable = new BaseTable<RoleRef>("RoleRef", "RoleRefID", CommonPhrases.RoleRefTable),
                ScriptTable = new BaseTable<Script>("Script", "ScriptID", CommonPhrases.ScriptTable),
                UnitTable = new BaseTable<Unit>("Unit", "UnitID", CommonPhrases.UnitTable),
                UserTable = new BaseTable<User>("User", "UserID", CommonPhrases.UserTable),
                ViewTable = new BaseTable<View>("View", "ViewID", CommonPhrases.ViewTable),
                ViewTypeTable = new BaseTable<ViewType>("ViewType", "ViewTypeID", CommonPhrases.ViewTypeTable)
            };
        }

        /// <summary>
        /// Adds relations between the tables.
        /// </summary>
        protected void AddRelations()
        {
            // object table
            AddRelation(ObjTable, ObjTable, "ParentObjNum");

            // device table
            AddRelation(DevTypeTable, DeviceTable, "DevTypeID");
            AddRelation(CommLineTable, DeviceTable, "CommLineNum");

            // input channel table
            AddRelation(CnlTypeTable, InCnlTable, "CnlTypeID");
            AddRelation(ObjTable, InCnlTable, "ObjNum");
            AddRelation(DeviceTable, InCnlTable, "DeviceNum");
            AddRelation(DataTypeTable, InCnlTable, "DataTypeID");
            AddRelation(FormatTable, InCnlTable, "FormatID");
            AddRelation(QuantityTable, InCnlTable, "QuantityID");
            AddRelation(UnitTable, InCnlTable, "UnitID");
            AddRelation(LimTable, InCnlTable, "LimID");

            // output channel table
            AddRelation(CmdTypeTable, OutCnlTable, "CmdTypeID");
            AddRelation(ObjTable, OutCnlTable, "ObjNum");
            AddRelation(DeviceTable, OutCnlTable, "DeviceNum");
            AddRelation(FormatTable, OutCnlTable, "FormatID");

            // view table
            AddRelation(ViewTypeTable, ViewTable, "ViewTypeID");
            AddRelation(ObjTable, ViewTable, "ObjNum");

            // user table
            AddRelation(RoleTable, UserTable, "RoleID");

            // object right table
            AddRelation(ObjTable, ObjRightTable, "ObjNum");
            AddRelation(RoleTable, ObjRightTable, "RoleID");

            // role inheritance table
            AddRelation(RoleTable, RoleRefTable, "ParentRoleID");
            AddRelation(RoleTable, RoleRefTable, "ChildRoleID");
        }

        /// <summary>
        /// Adds a relation to the configuration database.
        /// </summary>
        protected void AddRelation(IBaseTable parentTable, IBaseTable childTable, string childColumn)
        {
            TableRelation relation = new TableRelation(parentTable, childTable, childColumn);
            childTable.AddIndex(childColumn);
            childTable.DependsOn.Add(relation);
            parentTable.Dependent.Add(relation);
        }
    }
}
