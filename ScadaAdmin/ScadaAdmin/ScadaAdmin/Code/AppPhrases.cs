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
 * Module   : Administrator
 * Summary  : The phrases used by the application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2021
 */

namespace Scada.Admin.App.Code
{
    /// <summary>
    /// The phrases used by the application.
    /// <para>Фразы, используемые приложением.</para>
    /// </summary>
    internal static class AppPhrases
    {
        // Scada.Admin.App.Code.ExplorerBuilder
        public static string BaseNode { get; private set; }
        public static string PrimaryTablesNode { get; private set; }
        public static string SecondaryTablesNode { get; private set; }
        public static string TableByDeviceNode { get; private set; }
        public static string EmptyDeviceNode { get; private set; }
        public static string ViewsNode { get; private set; }
        public static string InstancesNode { get; private set; }
        public static string ServerNode { get; private set; }
        public static string CommNode { get; private set; }
        public static string WebNode { get; private set; }
        public static string EmptyNode { get; private set; }
        public static string DeviceFilter { get; private set; }
        public static string EmptyDeviceFilter { get; private set; }

        // Scada.Admin.App.Forms.Tables.FrmBaseTable
        public static string GridViewError { get; private set; }
        public static string ColumnLabel { get; private set; }
        public static string DeleteRowConfirm { get; private set; }
        public static string DeleteRowsConfirm { get; private set; }
        public static string ClearTableConfirm { get; private set; }
        public static string RowsNotDeleted { get; private set; }
        public static string ColumnNotNull { get; private set; }
        public static string UniqueRequired { get; private set; }
        public static string KeyReferenced { get; private set; }
        public static string DataNotExist { get; private set; }
        public static string DataChangeError { get; private set; }

        public static void Init()
        {
            LocaleDict dict = Locale.GetDictionary("Scada.Admin.App.Code.ExplorerBuilder");
            BaseNode = dict.GetPhrase("BaseNode");
            PrimaryTablesNode = dict.GetPhrase("PrimaryTablesNode");
            SecondaryTablesNode = dict.GetPhrase("SecondaryTablesNode");
            TableByDeviceNode = dict.GetPhrase("TableByDeviceNode");
            EmptyDeviceNode = dict.GetPhrase("EmptyDeviceNode");
            ViewsNode = dict.GetPhrase("ViewsNode");
            InstancesNode = dict.GetPhrase("InstancesNode");
            ServerNode = dict.GetPhrase("ServerNode");
            CommNode = dict.GetPhrase("CommNode");
            WebNode = dict.GetPhrase("WebNode");
            EmptyNode = dict.GetPhrase("EmptyNode");
            DeviceFilter = dict.GetPhrase("DeviceFilter");
            EmptyDeviceFilter = dict.GetPhrase("EmptyDeviceFilter");

            dict = Locale.GetDictionary("Scada.Admin.App.Forms.Tables.FrmBaseTable");
            GridViewError = dict.GetPhrase("GridViewError");
            ColumnLabel = dict.GetPhrase("ColumnLabel");
            DeleteRowConfirm = dict.GetPhrase("DeleteRowConfirm");
            DeleteRowsConfirm = dict.GetPhrase("DeleteRowsConfirm");
            ClearTableConfirm = dict.GetPhrase("ClearTableConfirm");
            RowsNotDeleted = dict.GetPhrase("RowsNotDeleted");
            ColumnNotNull = dict.GetPhrase("ColumnNotNull");
            UniqueRequired = dict.GetPhrase("UniqueRequired");
            KeyReferenced = dict.GetPhrase("KeyReferenced");
            DataNotExist = dict.GetPhrase("DataNotExist");
            DataChangeError = dict.GetPhrase("DataChangeError");
        }
    }
}
