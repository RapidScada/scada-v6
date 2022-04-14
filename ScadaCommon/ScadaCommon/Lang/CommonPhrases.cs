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
 * Summary  : The common phrases for the entire software package
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2022
 */

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

namespace Scada.Lang
{
    /// <summary>
    /// The common phrases for the entire software package.
    /// <para>Общие фразы для всего программного комплекса.</para>
    /// </summary>
    public static class CommonPhrases
    {
        // Scada.Application
        public static string ProductName { get; private set; }
        public static string ServerAppName { get; private set; }
        public static string CommAppName { get; private set; }
        public static string WebAppName { get; private set; }
        public static string WebsiteUrl { get; private set; }
        public static string UnhandledException { get; private set; }
        public static string ExecutionImpossible { get; private set; }
        public static string StartLogic { get; private set; }
        public static string LogicAlreadyStarted { get; private set; }
        public static string StartLogicError { get; private set; }
        public static string LogicStopped { get; private set; }
        public static string UnableToStopLogic { get; private set; }
        public static string StopLogicError { get; private set; }
        public static string LogicCycleError { get; private set; }
        public static string ThreadFatalError { get; private set; }
        public static string WriteInfoError { get; private set; }
        public static string ConnectionNotFound { get; private set; }
        public static string ConnOptionsNotFound { get; private set; }
        public static string DatabaseNotSupported { get; private set; }
        public static string OperationNotSupported { get; private set; }
        public static string CommandSent { get; private set; }
        public static string SendCommandError { get; private set; }
        public static string AgentDisabled { get; private set; }

        // Scada.ConfigBase
        public static string UndefinedTable { get; private set; }
        public static string ArchiveTable { get; private set; }
        public static string CnlTable { get; private set; }
        public static string CnlStatusTable { get; private set; }
        public static string CnlTypeTable { get; private set; }
        public static string CommLineTable { get; private set; }
        public static string DataTypeTable { get; private set; }
        public static string DeviceTable { get; private set; }
        public static string DevTypeTable { get; private set; }
        public static string FormatTable { get; private set; }
        public static string LimTable { get; private set; }
        public static string ObjTable { get; private set; }
        public static string ObjRightTable { get; private set; }
        public static string QuantityTable { get; private set; }
        public static string RoleTable { get; private set; }
        public static string RoleRefTable { get; private set; }
        public static string ScriptTable { get; private set; }
        public static string UnitTable { get; private set; }
        public static string UserTable { get; private set; }
        public static string ViewTable { get; private set; }
        public static string ViewTypeTable { get; private set; }
        public static string IndexNotFound { get; private set; }
        public static string EntityCaption { get; private set; }

        // Scada.Files
        public static string FileNotFound { get; private set; }
        public static string NamedFileNotFound { get; private set; }
        public static string DirectoryNotExists { get; private set; }
        public static string PathNotSupported { get; private set; }
        public static string InvalidFileFormat { get; private set; }
        public static string LoadConfigError { get; private set; }
        public static string SaveConfigError { get; private set; }
        public static string SaveConfigConfirm { get; private set; }
        public static string LoadViewError { get; private set; }
        public static string SaveViewError { get; private set; }

        // Scada.Format
        public static string IntegerRequired { get; private set; }
        public static string IntegerInRangeRequired { get; private set; }
        public static string RealRequired { get; private set; }
        public static string NonemptyRequired { get; private set; }
        public static string ValidUrlRequired { get; private set; }
        public static string ValidRangeRequired { get; private set; }
        public static string DateTimeRequired { get; private set; }
        public static string NotNumber { get; private set; }
        public static string NotHexadecimal { get; private set; }
        public static string InvalidParamVal { get; private set; }
        public static string InvalidSecretKey { get; private set; }
        public static string UndefinedSign { get; private set; }
        public static string CriticalSeverity { get; private set; }
        public static string MajorSeverity { get; private set; }
        public static string MinorSeverity { get; private set; }
        public static string InfoSeverity { get; private set; }

        // Scada.Forms
        public static string InfoCaption { get; private set; }
        public static string QuestionCaption { get; private set; }
        public static string ErrorCaption { get; private set; }
        public static string WarningCaption { get; private set; }
        public static string NoData { get; private set; }
        public static string EmptyData { get; private set; }
        public static string CorrectErrors { get; private set; }
        public static string HiddenPassword { get; private set; }
        public static string NewConnection { get; private set; }
        public static string UnnamedConnection { get; private set; }
        public static string XmlFileFilter { get; private set; }

        // Scada.ComponentModel
        public static string TrueValue { get; private set; }
        public static string FalseValue { get; private set; }
        public static string EmptyValue { get; private set; }
        public static string CollectionValue { get; private set; }

        // Scada.CnlDataFormatter
        public static string CommandDescrPrefix { get; private set; }
        public static string StatusFormat { get; private set; }

        static CommonPhrases()
        {
            // the phrases below may be required before loading dictionaries
            ProductName = "Rapid SCADA";

            if (Locale.IsRussian)
            {
                UnhandledException = "Необработанное исключение";
                ExecutionImpossible = "Нормальная работа невозможна";
                NamedFileNotFound = "Файл {0} не найден.";
            }
            else
            {
                UnhandledException = "Unhandled exception";
                ExecutionImpossible = "Normal execution is impossible";
                NamedFileNotFound = "File {0} not found.";
            }
        }

        public static void Init()
        {
            LocaleDict dict = Locale.GetDictionary("Scada.Application");
            ProductName = dict["ProductName"];
            ServerAppName = dict["ServerAppName"];
            CommAppName = dict["CommAppName"];
            WebAppName = dict["WebAppName"];
            WebsiteUrl = dict["WebsiteUrl"];
            UnhandledException = dict["UnhandledException"];
            ExecutionImpossible = dict["ExecutionImpossible"];
            StartLogic = dict["StartLogic"];
            LogicAlreadyStarted = dict["LogicAlreadyStarted"];
            StartLogicError = dict["StartLogicError"];
            LogicStopped = dict["LogicStopped"];
            UnableToStopLogic = dict["UnableToStopLogic"];
            StopLogicError = dict["StopLogicError"];
            LogicCycleError = dict["LogicCycleError"];
            ThreadFatalError = dict["ThreadFatalError"];
            WriteInfoError = dict["WriteInfoError"];
            ConnectionNotFound = dict["ConnectionNotFound"];
            ConnOptionsNotFound = dict["ConnOptionsNotFound"];
            DatabaseNotSupported = dict["DatabaseNotSupported"];
            OperationNotSupported = dict["OperationNotSupported"];
            CommandSent = dict["CommandSent"];
            SendCommandError = dict["SendCommandError"];
            AgentDisabled = dict["AgentDisabled"];

            dict = Locale.GetDictionary("Scada.ConfigBase");
            UndefinedTable = dict["UndefinedTable"];
            ArchiveTable = dict["ArchiveTable"];
            CnlTable = dict["CnlTable"];
            CnlStatusTable = dict["CnlStatusTable"];
            CnlTypeTable = dict["CnlTypeTable"];
            CommLineTable = dict["CommLineTable"];
            DataTypeTable = dict["DataTypeTable"];
            DeviceTable = dict["DeviceTable"];
            DevTypeTable = dict["DevTypeTable"];
            FormatTable = dict["FormatTable"];
            LimTable = dict["LimTable"];
            ObjTable = dict["ObjTable"];
            ObjRightTable = dict["ObjRightTable"];
            QuantityTable = dict["QuantityTable"];
            RoleTable = dict["RoleTable"];
            RoleRefTable = dict["RoleRefTable"];
            ScriptTable = dict["ScriptTable"];
            UnitTable = dict["UnitTable"];
            UserTable = dict["UserTable"];
            ViewTable = dict["ViewTable"];
            ViewTypeTable = dict["ViewTypeTable"];
            IndexNotFound = dict["IndexNotFound"];
            EntityCaption = dict["EntityCaption"];

            dict = Locale.GetDictionary("Scada.Files");
            FileNotFound = dict["FileNotFound"];
            NamedFileNotFound = dict["NamedFileNotFound"];
            DirectoryNotExists = dict["DirectoryNotExists"];
            PathNotSupported = dict["PathNotSupported"];
            InvalidFileFormat = dict["InvalidFileFormat"];
            LoadConfigError = dict["LoadConfigError"];
            SaveConfigError = dict["SaveConfigError"];
            SaveConfigConfirm = dict["SaveConfigConfirm"];
            LoadViewError = dict["LoadViewError"];
            SaveViewError = dict["SaveViewError"];

            dict = Locale.GetDictionary("Scada.Format");
            IntegerRequired = dict["IntegerRequired"];
            IntegerInRangeRequired = dict["IntegerInRangeRequired"];
            RealRequired = dict["RealRequired"];
            NonemptyRequired = dict["NonemptyRequired"];
            ValidUrlRequired = dict["ValidUrlRequired"];
            ValidRangeRequired = dict["ValidRangeRequired"];
            DateTimeRequired = dict["DateTimeRequired"];
            NotNumber = dict["NotNumber"];
            NotHexadecimal = dict["NotHexadecimal"];
            InvalidParamVal = dict["InvalidParamVal"];
            InvalidSecretKey = dict["InvalidSecretKey"];
            UndefinedSign = dict["UndefinedSign"];
            CriticalSeverity = dict["CriticalSeverity"];
            MajorSeverity = dict["MajorSeverity"];
            MinorSeverity = dict["MinorSeverity"];
            InfoSeverity = dict["InfoSeverity"];

            dict = Locale.GetDictionary("Scada.Forms");
            InfoCaption = dict["InfoCaption"];
            QuestionCaption = dict["QuestionCaption"];
            ErrorCaption = dict["ErrorCaption"];
            WarningCaption = dict["WarningCaption"];
            NoData = dict["NoData"];
            EmptyData = dict["EmptyData"];
            CorrectErrors = dict["CorrectErrors"];
            HiddenPassword = dict["HiddenPassword"];
            NewConnection = dict["NewConnection"];
            UnnamedConnection = dict["UnnamedConnection"];
            XmlFileFilter = dict["XmlFileFilter"];

            dict = Locale.GetDictionary("Scada.ComponentModel");
            TrueValue = dict["TrueValue"];
            FalseValue = dict["FalseValue"];
            EmptyValue = dict["EmptyValue"];
            CollectionValue = dict["CollectionValue"];

            dict = Locale.GetDictionary("Scada.CnlDataFormatter");
            CommandDescrPrefix = dict["CommandDescrPrefix"];
            StatusFormat = dict["StatusFormat"];
        }
    }
}
