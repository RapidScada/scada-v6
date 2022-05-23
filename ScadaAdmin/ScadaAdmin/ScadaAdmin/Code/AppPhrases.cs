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
 * Summary  : The phrases used by the application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2022
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
        public static string CommandEvent { get; private set; }

        // Scada.Admin.App.Code.ExplorerBuilder
        public static string ConfigDatabaseNode { get; private set; }
        public static string PrimaryTablesNode { get; private set; }
        public static string SecondaryTablesNode { get; private set; }
        public static string ViewsNode { get; private set; }
        public static string InstancesNode { get; private set; }
        public static string ServerNode { get; private set; }
        public static string CommNode { get; private set; }
        public static string WebNode { get; private set; }
        public static string AppConfigNode { get; private set; }
        public static string DeviceFilter { get; private set; }
        public static string EmptyDeviceFilter { get; private set; }

        // Scada.Admin.App.Controls.Deployment.CtrlProfileSelector
        public static string ProfileNotSet { get; private set; }
        public static string ConfirmDeleteProfile { get; private set; }

        // Scada.Admin.App.Controls.Deployment.CtrlTransferOptions
        public static string ConfigNotSelected { get; private set; }
        public static string InvalidObjectFilter { get; private set; }

        // Scada.Admin.App.Forms
        public static string SelectedColumn { get; private set; }
        public static string NoProfileConnections { get; private set; }
        public static string ExtensionNotFound { get; private set; }
        public static string ExtensionCannotDeploy { get; private set; }

        // Scada.Admin.App.Forms.Deployment.FrmInstanceProfile
        public static string AgentConnectionOK { get; private set; }
        public static string AgentConnectionError { get; private set; }
        public static string DbConnectionOK { get; private set; }
        public static string DbConnectionError { get; private set; }

        // Scada.Admin.App.Forms.Deployment.FrmInstanceStatus
        public static string ControlServiceSuccessful { get; private set; }
        public static string UnableControlService { get; private set; }
        public static string ControlServiceError { get; private set; }
        public static string AgentDisabled { get; private set; }

        // Scada.Admin.App.Forms.Deployment.FrmProfileEdit
        public static string ProfileNameDuplicated { get; private set; }

        // Scada.Admin.App.Forms.Deployment.FrmTransfer
        public static string DownloadTitle { get; private set; }
        public static string DownloadProgress { get; private set; }
        public static string DownloadError { get; private set; }
        public static string UploadTitle { get; private set; }
        public static string UploadProgress { get; private set; }
        public static string UploadError { get; private set; }
        public static string OperationCompleted { get; private set; }
        public static string OperationCanceled { get; private set; }
        public static string OperationError { get; private set; }

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

        // Scada.Admin.App.Forms.Tables.FrmFilter
        public static string IncorrectTableFilter { get; private set; }

        // Scada.Admin.App.Forms.Tables.FrmFind
        public static string ValueNotFound { get; private set; }
        public static string SearchCompleted { get; private set; }
        public static string ReplaceCount { get; private set; }

        // Scada.Admin.App.Forms.Tables.FrmLimCreate
        public static string LimExistsInConfigDatabase { get; private set; }
        public static string DefaultLimName { get; private set; }

        // Scada.Admin.App.Forms.Tables.FrmTextDialog
        public static string TextLine { get; private set; }
        public static string TextLength { get; private set; }

        // Scada.Admin.App.Forms.Tools.FrmConfig
        public static string ProjectExtRegistered { get; private set; }

        // Scada.Admin.App.Forms.Tools.FrmCulture
        public static string LoadCulturesError { get; private set; }
        public static string CultureRequired { get; private set; }
        public static string CultureNotFound { get; private set; }

        // Scada.Admin.App.Forms.Tools.FrmFileAssociation
        public static string ExecutableFileFilter { get; private set; }

        // Scada.Admin.App.Forms.FrmFileNew
        public static string FileNameEmpty { get; private set; }
        public static string FileNameInvalid { get; private set; }

        // Scada.Admin.App.Forms.FrmInstanceEdit
        public static string NewInstanceTitle { get; private set; }
        public static string EditInstanceTitle { get; private set; }
        public static string InstanceNameEmpty { get; private set; }
        public static string InstanceNameInvalid { get; private set; }
        public static string InstanceSelectApps { get; private set; }

        // Scada.Admin.App.Forms.FrmItemName
        public static string ItemNameEmpty { get; private set; }
        public static string ItemNameInvalid { get; private set; }
        public static string ItemNameDuplicated { get; private set; }

        // Scada.Admin.App.Forms.FrmMain
        public static string EmptyTitle { get; private set; }
        public static string ProjectTitle { get; private set; }
        public static string WelcomeMessage { get; private set; }
        public static string SelectItemMessage { get; private set; }
        public static string ProjectFileFilter { get; private set; }
        public static string ConfirmDeleteDirectory { get; private set; }
        public static string ConfirmDeleteFile { get; private set; }
        public static string ConfirmDeleteInstance { get; private set; }
        public static string FileOperationError { get; private set; }
        public static string DirectoryAlreadyExists { get; private set; }
        public static string FileAlreadyExists { get; private set; }
        public static string InstanceAlreadyExists { get; private set; }
        public static string SaveConfigDatabaseConfirm { get; private set; }
        public static string DeviceNotFoundInComm { get; private set; }
        public static string WebUrlNotSet { get; private set; }
        public static string ReopenProject { get; private set; }

        // Scada.Admin.App.Forms.FrmProjectNew
        public static string ChooseProjectLocation { get; private set; }
        public static string ProjectNameEmpty { get; private set; }
        public static string ProjectNameInvalid { get; private set; }
        public static string ProjectLocationEmpty { get; private set; }
        public static string ProjectLocationInvalid { get; private set; }
        public static string ProjectAlreadyExists { get; private set; }
        public static string ProjectTemplateEmpty { get; private set; }
        public static string ProjectTemplateNotFound { get; private set; }

        // Scada.Admin.App.Forms.FrmTextEditor
        public static string OpenTextFileError { get; private set; }
        public static string SaveTextFileError { get; private set; }

        public static void Init()
        {
            LocaleDict dict = Locale.GetDictionary("Scada.Admin.App.Code.AppState");
            LoadAppStateError = dict["LoadAppStateError"];
            SaveAppStateError = dict["SaveAppStateError"];

            dict = Locale.GetDictionary("Scada.Admin.App.Code.AppUtils");
            EventEnabled = dict["EventEnabled"];
            EventBeep = dict["EventBeep"];
            DataChangeEvent = dict["DataChangeEvent"];
            ValueChangeEvent = dict["ValueChangeEvent"];
            StatusChangeEvent = dict["StatusChangeEvent"];
            CnlUndefinedEvent = dict["CnlUndefinedEvent"];
            CommandEvent = dict["CommandEvent"];

            dict = Locale.GetDictionary("Scada.Admin.App.Code.ExplorerBuilder");
            ConfigDatabaseNode = dict["ConfigDatabaseNode"];
            PrimaryTablesNode = dict["PrimaryTablesNode"];
            SecondaryTablesNode = dict["SecondaryTablesNode"];
            ViewsNode = dict["ViewsNode"];
            InstancesNode = dict["InstancesNode"];
            ServerNode = dict["ServerNode"];
            CommNode = dict["CommNode"];
            WebNode = dict["WebNode"];
            AppConfigNode = dict["AppConfigNode"];
            DeviceFilter = dict["DeviceFilter"];
            EmptyDeviceFilter = dict["EmptyDeviceFilter"];

            dict = Locale.GetDictionary("Scada.Admin.App.Controls.Deployment.CtrlProfileSelector");
            ProfileNotSet = dict["ProfileNotSet"];
            ConfirmDeleteProfile = dict["ConfirmDeleteProfile"];

            dict = Locale.GetDictionary("Scada.Admin.App.Controls.Deployment.CtrlTransferOptions");
            ConfigNotSelected = dict["ConfigNotSelected"];
            InvalidObjectFilter = dict["InvalidObjectFilter"];

            dict = Locale.GetDictionary("Scada.Admin.App.Forms");
            SelectedColumn = dict["SelectedColumn"];
            NoProfileConnections = dict["NoProfileConnections"];
            ExtensionNotFound = dict["ExtensionNotFound"];
            ExtensionCannotDeploy = dict["ExtensionCannotDeploy"];

            dict = Locale.GetDictionary("Scada.Admin.App.Forms.Deployment.FrmInstanceProfile");
            AgentConnectionOK = dict["AgentConnectionOK"];
            AgentConnectionError = dict["AgentConnectionError"];
            DbConnectionOK = dict["DbConnectionOK"];
            DbConnectionError = dict["DbConnectionError"];

            dict = Locale.GetDictionary("Scada.Admin.App.Forms.Deployment.FrmInstanceStatus");
            ControlServiceSuccessful = dict["ControlServiceSuccessful"];
            UnableControlService = dict["UnableControlService"];
            ControlServiceError = dict["ControlServiceError"];
            AgentDisabled = dict["AgentDisabled"];

            dict = Locale.GetDictionary("Scada.Admin.App.Forms.Deployment.FrmProfileEdit");
            ProfileNameDuplicated = dict["ProfileNameDuplicated"];

            dict = Locale.GetDictionary("Scada.Admin.App.Forms.Deployment.FrmTransfer");
            DownloadTitle = dict["DownloadTitle"];
            DownloadProgress = dict["DownloadProgress"];
            DownloadError = dict["DownloadError"];
            UploadTitle = dict["UploadTitle"];
            UploadProgress = dict["UploadProgress"];
            UploadError = dict["UploadError"];
            OperationCompleted = dict["OperationCompleted"];
            OperationCanceled = dict["OperationCanceled"];
            OperationError = dict["OperationError"];

            dict = Locale.GetDictionary("Scada.Admin.App.Forms.Tables.FrmBaseTable");
            GridViewError = dict["GridViewError"];
            ColumnLabel = dict["ColumnLabel"];
            DeleteRowConfirm = dict["DeleteRowConfirm"];
            DeleteRowsConfirm = dict["DeleteRowsConfirm"];
            ClearTableConfirm = dict["ClearTableConfirm"];
            RowsNotDeleted = dict["RowsNotDeleted"];
            ColumnNotNull = dict["ColumnNotNull"];
            UniqueRequired = dict["UniqueRequired"];
            KeyReferenced = dict["KeyReferenced"];
            DataNotExist = dict["DataNotExist"];
            DataChangeError = dict["DataChangeError"];

            dict = Locale.GetDictionary("Scada.Admin.App.Forms.Tables.FrmFilter");
            IncorrectTableFilter = dict["IncorrectTableFilter"];

            dict = Locale.GetDictionary("Scada.Admin.App.Forms.Tables.FrmFind");
            ValueNotFound = dict["ValueNotFound"];
            SearchCompleted = dict["SearchCompleted"];
            ReplaceCount = dict["ReplaceCount"];

            dict = Locale.GetDictionary("Scada.Admin.App.Forms.Tables.FrmLimCreate");
            LimExistsInConfigDatabase = dict["LimExistsInConfigDatabase"];
            DefaultLimName = dict["DefaultLimName"];

            dict = Locale.GetDictionary("Scada.Admin.App.Forms.Tables.FrmTextDialog");
            TextLine = dict["TextLine"];
            TextLength = dict["TextLength"];

            dict = Locale.GetDictionary("Scada.Admin.App.Forms.Tools.FrmCulture");
            LoadCulturesError = dict["LoadCulturesError"];
            CultureRequired = dict["CultureRequired"];
            CultureNotFound = dict["CultureNotFound"];

            dict = Locale.GetDictionary("Scada.Admin.App.Forms.Tools.FrmConfig");
            ProjectExtRegistered = dict["ProjectExtRegistered"];

            dict = Locale.GetDictionary("Scada.Admin.App.Forms.Tools.FrmFileAssociation");
            ExecutableFileFilter = dict.GetPhrase("ExecutableFileFilter");

            dict = Locale.GetDictionary("Scada.Admin.App.Forms.FrmFileNew");
            FileNameEmpty = dict["FileNameEmpty"];
            FileNameInvalid = dict["FileNameInvalid"];

            dict = Locale.GetDictionary("Scada.Admin.App.Forms.FrmInstanceEdit");
            NewInstanceTitle = dict["NewInstanceTitle"];
            EditInstanceTitle = dict["EditInstanceTitle"];
            InstanceNameEmpty = dict["InstanceNameEmpty"];
            InstanceNameInvalid = dict["InstanceNameInvalid"];
            InstanceSelectApps = dict["InstanceSelectApps"];

            dict = Locale.GetDictionary("Scada.Admin.App.Forms.FrmItemName");
            ItemNameEmpty = dict["ItemNameEmpty"];
            ItemNameInvalid = dict["ItemNameInvalid"];
            ItemNameDuplicated = dict["ItemNameDuplicated"];

            dict = Locale.GetDictionary("Scada.Admin.App.Forms.FrmMain");
            EmptyTitle = dict["EmptyTitle"];
            ProjectTitle = dict["ProjectTitle"];
            WelcomeMessage = dict["WelcomeMessage"];
            SelectItemMessage = dict["SelectItemMessage"];
            ProjectFileFilter = dict["ProjectFileFilter"];
            ConfirmDeleteDirectory = dict["ConfirmDeleteDirectory"];
            ConfirmDeleteFile = dict["ConfirmDeleteFile"];
            ConfirmDeleteInstance = dict["ConfirmDeleteInstance"];
            FileOperationError = dict["FileOperationError"];
            DirectoryAlreadyExists = dict["DirectoryAlreadyExists"];
            FileAlreadyExists = dict["FileAlreadyExists"];
            InstanceAlreadyExists = dict["InstanceAlreadyExists"];
            SaveConfigDatabaseConfirm = dict["SaveConfigDatabaseConfirm"];
            DeviceNotFoundInComm = dict["DeviceNotFoundInComm"];
            WebUrlNotSet = dict["WebUrlNotSet"];
            ReopenProject = dict["ReopenProject"];

            dict = Locale.GetDictionary("Scada.Admin.App.Forms.FrmProjectNew");
            ChooseProjectLocation = dict["ChooseProjectLocation"];
            ProjectNameEmpty = dict["ProjectNameEmpty"];
            ProjectNameInvalid = dict["ProjectNameInvalid"];
            ProjectLocationEmpty = dict["ProjectLocationEmpty"];
            ProjectLocationInvalid = dict["ProjectLocationInvalid"];
            ProjectAlreadyExists = dict["ProjectAlreadyExists"];
            ProjectTemplateEmpty = dict["ProjectTemplateEmpty"];
            ProjectTemplateNotFound = dict["ProjectTemplateNotFound"];

            dict = Locale.GetDictionary("Scada.Admin.App.Forms.FrmTextEditor");
            OpenTextFileError = dict["OpenTextFileError"];
            SaveTextFileError = dict["SaveTextFileError"];
        }
    }
}
