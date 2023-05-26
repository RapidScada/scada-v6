/*
 * Copyright 2023 Rapid Software LLC
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
        public static string ObjectFilter { get;private set; }
        public static string EmptyDeviceFilter { get; private set; }
        public static string MainObjectFolder { get;private set; }
        public static string InfiniteLoopError { get; private set; }
        public static string InfiniteLoopNoParentError { get; private set; }

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
            LoadAppStateError = dict[nameof(LoadAppStateError)];
            SaveAppStateError = dict[nameof(SaveAppStateError)];

            dict = Locale.GetDictionary("Scada.Admin.App.Code.ExplorerBuilder");
            ConfigDatabaseNode = dict[nameof(ConfigDatabaseNode)];
            PrimaryTablesNode = dict[nameof(PrimaryTablesNode)];
            SecondaryTablesNode = dict[nameof(SecondaryTablesNode)];
            ViewsNode = dict[nameof(ViewsNode)];
            InstancesNode = dict[nameof(InstancesNode)];
            ServerNode = dict[nameof(ServerNode)];
            CommNode = dict[nameof(CommNode)];
            WebNode = dict[nameof(WebNode)];
            AppConfigNode = dict[nameof(AppConfigNode)];
            DeviceFilter = dict[nameof(DeviceFilter)];
            ObjectFilter = dict[nameof(ObjectFilter)];
            EmptyDeviceFilter = dict[nameof(EmptyDeviceFilter)];
            MainObjectFolder = dict[nameof(MainObjectFolder)];
            InfiniteLoopError = dict[nameof(InfiniteLoopError)];    
            InfiniteLoopNoParentError = dict[nameof(InfiniteLoopNoParentError)];

            dict = Locale.GetDictionary("Scada.Admin.App.Controls.Deployment.CtrlProfileSelector");
            ProfileNotSet = dict[nameof(ProfileNotSet)];
            ConfirmDeleteProfile = dict[nameof(ConfirmDeleteProfile)];

            dict = Locale.GetDictionary("Scada.Admin.App.Controls.Deployment.CtrlTransferOptions");
            ConfigNotSelected = dict[nameof(ConfigNotSelected)];
            InvalidObjectFilter = dict[nameof(InvalidObjectFilter)];

            dict = Locale.GetDictionary("Scada.Admin.App.Forms");
            SelectedColumn = dict[nameof(SelectedColumn)];
            NoProfileConnections = dict[nameof(NoProfileConnections)];
            ExtensionNotFound = dict[nameof(ExtensionNotFound)];
            ExtensionCannotDeploy = dict[nameof(ExtensionCannotDeploy)];

            dict = Locale.GetDictionary("Scada.Admin.App.Forms.Deployment.FrmInstanceProfile");
            AgentConnectionOK = dict[nameof(AgentConnectionOK)];
            AgentConnectionError = dict[nameof(AgentConnectionError)];
            DbConnectionOK = dict[nameof(DbConnectionOK)];
            DbConnectionError = dict[nameof(DbConnectionError)];

            dict = Locale.GetDictionary("Scada.Admin.App.Forms.Deployment.FrmInstanceStatus");
            UnableControlService = dict[nameof(UnableControlService)];
            ControlServiceError = dict[nameof(ControlServiceError)];
            AgentDisabled = dict[nameof(AgentDisabled)];

            dict = Locale.GetDictionary("Scada.Admin.App.Forms.Deployment.FrmProfileEdit");
            ProfileNameDuplicated = dict[nameof(ProfileNameDuplicated)];

            dict = Locale.GetDictionary("Scada.Admin.App.Forms.Deployment.FrmTransfer");
            DownloadTitle = dict[nameof(DownloadTitle)];
            DownloadProgress = dict[nameof(DownloadProgress)];
            DownloadError = dict[nameof(DownloadError)];
            UploadTitle = dict[nameof(UploadTitle)];
            UploadProgress = dict[nameof(UploadProgress)];
            UploadError = dict[nameof(UploadError)];
            OperationCompleted = dict[nameof(OperationCompleted)];
            OperationCanceled = dict[nameof(OperationCanceled)];
            OperationError = dict[nameof(OperationError)];

            dict = Locale.GetDictionary("Scada.Admin.App.Forms.Tables.FrmBaseTable");
            GridViewError = dict[nameof(GridViewError)];
            ColumnLabel = dict[nameof(ColumnLabel)];
            DeleteRowConfirm = dict[nameof(DeleteRowConfirm)];
            DeleteRowsConfirm = dict[nameof(DeleteRowsConfirm)];
            ClearTableConfirm = dict[nameof(ClearTableConfirm)];
            RowsNotDeleted = dict[nameof(RowsNotDeleted)];
            ColumnNotNull = dict[nameof(ColumnNotNull)];
            UniqueRequired = dict[nameof(UniqueRequired)];
            KeyReferenced = dict[nameof(KeyReferenced)];
            DataNotExist = dict[nameof(DataNotExist)];
            DataChangeError = dict[nameof(DataChangeError)];

            dict = Locale.GetDictionary("Scada.Admin.App.Forms.Tables.FrmFilter");
            IncorrectTableFilter = dict[nameof(IncorrectTableFilter)];

            dict = Locale.GetDictionary("Scada.Admin.App.Forms.Tables.FrmFind");
            ValueNotFound = dict[nameof(ValueNotFound)];
            SearchCompleted = dict[nameof(SearchCompleted)];
            ReplaceCount = dict[nameof(ReplaceCount)];

            dict = Locale.GetDictionary("Scada.Admin.App.Forms.Tables.FrmLimCreate");
            LimExistsInConfigDatabase = dict[nameof(LimExistsInConfigDatabase)];
            DefaultLimName = dict[nameof(DefaultLimName)];

            dict = Locale.GetDictionary("Scada.Admin.App.Forms.Tables.FrmTextDialog");
            TextLine = dict[nameof(TextLine)];
            TextLength = dict[nameof(TextLength)];

            dict = Locale.GetDictionary("Scada.Admin.App.Forms.Tools.FrmCulture");
            LoadCulturesError = dict[nameof(LoadCulturesError)];
            CultureRequired = dict[nameof(CultureRequired)];
            CultureNotFound = dict[nameof(CultureNotFound)];

            dict = Locale.GetDictionary("Scada.Admin.App.Forms.Tools.FrmConfig");
            ProjectExtRegistered = dict[nameof(ProjectExtRegistered)];

            dict = Locale.GetDictionary("Scada.Admin.App.Forms.Tools.FrmFileAssociation");
            ExecutableFileFilter = dict.GetPhrase("ExecutableFileFilter");

            dict = Locale.GetDictionary("Scada.Admin.App.Forms.FrmFileNew");
            FileNameEmpty = dict[nameof(FileNameEmpty)];
            FileNameInvalid = dict[nameof(FileNameInvalid)];

            dict = Locale.GetDictionary("Scada.Admin.App.Forms.FrmInstanceEdit");
            NewInstanceTitle = dict[nameof(NewInstanceTitle)];
            EditInstanceTitle = dict[nameof(EditInstanceTitle)];
            InstanceNameEmpty = dict[nameof(InstanceNameEmpty)];
            InstanceNameInvalid = dict[nameof(InstanceNameInvalid)];
            InstanceSelectApps = dict[nameof(InstanceSelectApps)];

            dict = Locale.GetDictionary("Scada.Admin.App.Forms.FrmItemName");
            ItemNameEmpty = dict[nameof(ItemNameEmpty)];
            ItemNameInvalid = dict[nameof(ItemNameInvalid)];
            ItemNameDuplicated = dict[nameof(ItemNameDuplicated)];

            dict = Locale.GetDictionary("Scada.Admin.App.Forms.FrmMain");
            EmptyTitle = dict[nameof(EmptyTitle)];
            ProjectTitle = dict[nameof(ProjectTitle)];
            WelcomeMessage = dict[nameof(WelcomeMessage)];
            SelectItemMessage = dict[nameof(SelectItemMessage)];
            ProjectFileFilter = dict[nameof(ProjectFileFilter)];
            ConfirmDeleteDirectory = dict[nameof(ConfirmDeleteDirectory)];
            ConfirmDeleteFile = dict[nameof(ConfirmDeleteFile)];
            ConfirmDeleteInstance = dict[nameof(ConfirmDeleteInstance)];
            FileOperationError = dict[nameof(FileOperationError)];
            DirectoryAlreadyExists = dict[nameof(DirectoryAlreadyExists)];
            FileAlreadyExists = dict[nameof(FileAlreadyExists)];
            InstanceAlreadyExists = dict[nameof(InstanceAlreadyExists)];
            SaveConfigDatabaseConfirm = dict[nameof(SaveConfigDatabaseConfirm)];
            DeviceNotFoundInComm = dict[nameof(DeviceNotFoundInComm)];
            WebUrlNotSet = dict[nameof(WebUrlNotSet)];
            ReopenProject = dict[nameof(ReopenProject)];

            dict = Locale.GetDictionary("Scada.Admin.App.Forms.FrmProjectNew");
            ChooseProjectLocation = dict[nameof(ChooseProjectLocation)];
            ProjectNameEmpty = dict[nameof(ProjectNameEmpty)];
            ProjectNameInvalid = dict[nameof(ProjectNameInvalid)];
            ProjectLocationEmpty = dict[nameof(ProjectLocationEmpty)];
            ProjectLocationInvalid = dict[nameof(ProjectLocationInvalid)];
            ProjectAlreadyExists = dict[nameof(ProjectAlreadyExists)];
            ProjectTemplateEmpty = dict[nameof(ProjectTemplateEmpty)];
            ProjectTemplateNotFound = dict[nameof(ProjectTemplateNotFound)];

            dict = Locale.GetDictionary("Scada.Admin.App.Forms.FrmTextEditor");
            OpenTextFileError = dict[nameof(OpenTextFileError)];
            SaveTextFileError = dict[nameof(SaveTextFileError)];
        }
    }
}
