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

using Scada.Lang;

namespace Scada.Admin.App.Code
{
    /// <summary>
    /// The phrases used by the application.
    /// <para>Фразы, используемые приложением.</para>
    /// </summary>
    internal static class AppPhrases
    {
        // Scada.Admin.App.Code.AppState
        public static string LoadAppStateError { get; private set; }
        public static string SaveAppStateError { get; private set; }

        // Scada.Admin.App.Code.AppUtils
        public static string EventEnabled { get; private set; }
        public static string EventBeep { get; private set; }
        public static string DataChangeEvent { get; private set; }
        public static string ValueChangeEvent { get; private set; }
        public static string StatusChangeEvent { get; private set; }
        public static string CnlUndefinedEvent { get; private set; }

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

        // Scada.Admin.App.Forms.Tables.FrmTextDialog
        public static string TextLine { get; private set; }
        public static string TextLength { get; private set; }

        // Scada.Admin.App.Forms.Tools.FrmCulture
        public static string LoadCulturesError { get; private set; }
        public static string CultureRequired { get; private set; }
        public static string CultureNotFound { get; private set; }

        // Scada.Admin.App.Forms.FrmMain
        public static string EmptyTitle { get; private set; }
        public static string ProjectTitle { get; private set; }
        public static string WelcomeMessage { get; private set; }
        public static string SelectItemMessage { get; private set; }
        public static string ProjectFileFilter { get; private set; }
        public static string ConfirmDeleteDirectory { get; private set; }
        public static string ConfirmDeleteFile { get; private set; }
        public static string ConfirmDeleteInstance { get; private set; }
        public static string ConfirmDeleteCommLine { get; private set; }
        public static string FileOperationError { get; private set; }
        public static string DirectoryAlreadyExists { get; private set; }
        public static string FileAlreadyExists { get; private set; }
        public static string InstanceAlreadyExists { get; private set; }
        public static string SaveConfigBaseConfirm { get; private set; }
        public static string DeviceNotFoundInComm { get; private set; }
        public static string WebUrlNotSet { get; private set; }
        public static string ReopenProject { get; private set; }

        // Scada.Admin.App.Forms.FrmProjectNew
        public static string ChooseProjectLocation { get; private set; }
        public static string ProjectNameEmpty { get; private set; }
        public static string ProjectNameInvalid { get; private set; }
        public static string ProjectLocationNotExists { get; private set; }
        public static string ProjectAlreadyExists { get; private set; }
        public static string ProjectTemplateEmpty { get; private set; }
        public static string ProjectTemplateNotFound { get; private set; }

        public static void Init()
        {
            LocaleDict dict = Locale.GetDictionary("Scada.Admin.App.Code.AppState");
            LoadAppStateError = dict.GetPhrase("LoadAppStateError");
            SaveAppStateError = dict.GetPhrase("SaveAppStateError");

            dict = Locale.GetDictionary("Scada.Admin.App.Code.AppUtils");
            EventEnabled = dict.GetPhrase("EventEnabled");
            EventBeep = dict.GetPhrase("EventBeep");
            DataChangeEvent = dict.GetPhrase("DataChangeEvent");
            ValueChangeEvent = dict.GetPhrase("ValueChangeEvent");
            StatusChangeEvent = dict.GetPhrase("StatusChangeEvent");
            CnlUndefinedEvent = dict.GetPhrase("CnlUndefinedEvent");

            dict = Locale.GetDictionary("Scada.Admin.App.Code.ExplorerBuilder");
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

            dict = Locale.GetDictionary("Scada.Admin.App.Forms.Tables.FrmTextDialog");
            TextLine = dict.GetPhrase("TextLine");
            TextLength = dict.GetPhrase("TextLength");

            dict = Locale.GetDictionary("Scada.Admin.App.Forms.Tools.FrmCulture");
            LoadCulturesError = dict.GetPhrase("LoadCulturesError");
            CultureRequired = dict.GetPhrase("CultureRequired");
            CultureNotFound = dict.GetPhrase("CultureNotFound");

            dict = Locale.GetDictionary("Scada.Admin.App.Forms.FrmMain");
            EmptyTitle = dict.GetPhrase("EmptyTitle");
            ProjectTitle = dict.GetPhrase("ProjectTitle");
            WelcomeMessage = dict.GetPhrase("WelcomeMessage");
            SelectItemMessage = dict.GetPhrase("SelectItemMessage");
            ProjectFileFilter = dict.GetPhrase("ProjectFileFilter");
            ConfirmDeleteDirectory = dict.GetPhrase("ConfirmDeleteDirectory");
            ConfirmDeleteFile = dict.GetPhrase("ConfirmDeleteFile");
            ConfirmDeleteInstance = dict.GetPhrase("ConfirmDeleteInstance");
            ConfirmDeleteCommLine = dict.GetPhrase("ConfirmDeleteCommLine");
            FileOperationError = dict.GetPhrase("FileOperationError");
            DirectoryAlreadyExists = dict.GetPhrase("DirectoryAlreadyExists");
            FileAlreadyExists = dict.GetPhrase("FileAlreadyExists");
            InstanceAlreadyExists = dict.GetPhrase("InstanceAlreadyExists");
            SaveConfigBaseConfirm = dict.GetPhrase("SaveConfigBaseConfirm");
            DeviceNotFoundInComm = dict.GetPhrase("DeviceNotFoundInComm");
            WebUrlNotSet = dict.GetPhrase("WebUrlNotSet");
            ReopenProject = dict.GetPhrase("ReopenProject");

            dict = Locale.GetDictionary("Scada.Admin.App.Forms.FrmProjectNew");
            ChooseProjectLocation = dict.GetPhrase("ChooseProjectLocation");
            ProjectNameEmpty = dict.GetPhrase("ProjectNameEmpty");
            ProjectNameInvalid = dict.GetPhrase("ProjectNameInvalid");
            ProjectLocationNotExists = dict.GetPhrase("ProjectLocationNotExists");
            ProjectAlreadyExists = dict.GetPhrase("ProjectAlreadyExists");
            ProjectTemplateEmpty = dict.GetPhrase("ProjectTemplateEmpty");
            ProjectTemplateNotFound = dict.GetPhrase("ProjectTemplateNotFound");
        }
    }
}
