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
 * Summary  : The common phrases for the entire software package
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2021
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

        // Scada.ConfigBase
        public static string ArchiveTable { get; private set; }
        public static string CmdTypeTable { get; private set; }
        public static string CmdValTable { get; private set; }
        public static string CnlStatusTable { get; private set; }
        public static string CnlTypeTable { get; private set; }
        public static string CommLineTable { get; private set; }
        public static string DataTypeTable { get; private set; }
        public static string DeviceTable { get; private set; }
        public static string DevTypeTable { get; private set; }
        public static string FormatTable { get; private set; }
        public static string InCnlTable { get; private set; }
        public static string LimTable { get; private set; }
        public static string ObjTable { get; private set; }
        public static string ObjRightTable { get; private set; }
        public static string OutCnlTable { get; private set; }
        public static string QuantityTable { get; private set; }
        public static string RoleTable { get; private set; }
        public static string RoleRefTable { get; private set; }
        public static string ScriptTable { get; private set; }
        public static string UnitTable { get; private set; }
        public static string UserTable { get; private set; }
        public static string ViewTable { get; private set; }
        public static string ViewTypeTable { get; private set; }

        // Scada.Files
        public static string FileNotFound { get; private set; }
        public static string NamedFileNotFound { get; private set; }
        public static string DirectoryNotExists { get; private set; }
        public static string LoadAppConfigError { get; private set; }
        public static string SaveAppConfigError { get; private set; }
        public static string LoadViewError { get; private set; }
        public static string SaveViewError { get; private set; }

        // Scada.Format
        public static string IntegerRequired { get; private set; }
        public static string IntegerInRangeRequired { get; private set; }
        public static string RealRequired { get; private set; }
        public static string NonemptyRequired { get; private set; }
        public static string DateTimeRequired { get; private set; }
        public static string NotNumber { get; private set; }
        public static string NotHexadecimal { get; private set; }
        public static string InvalidParamVal { get; private set; }
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

        static CommonPhrases()
        {
            // the phrases below may be required before loading dictionaries
            ProductName = "Rapid SCADA";

            if (Locale.IsRussian)
            {
                UnhandledException = "Необработанное исключение";
                ExecutionImpossible = "Нормальная работа программы невозможна";
                NamedFileNotFound = "Файл {0} не найден.";
            }
            else
            {
                UnhandledException = "Unhandled exception";
                ExecutionImpossible = "Normal program execution is impossible";
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

            dict = Locale.GetDictionary("Scada.ConfigBase");
            ArchiveTable = dict["ArchiveTable"];
            CmdTypeTable = dict["CmdTypeTable"];
            CmdValTable = dict["CmdValTable"];
            CnlStatusTable = dict["CnlStatusTable"];
            CnlTypeTable = dict["CnlTypeTable"];
            CommLineTable = dict["CommLineTable"];
            DataTypeTable = dict["DataTypeTable"];
            DeviceTable = dict["DeviceTable"];
            DevTypeTable = dict["DevTypeTable"];
            FormatTable = dict["FormatTable"];
            InCnlTable = dict["InCnlTable"];
            LimTable = dict["LimTable"];
            ObjTable = dict["ObjTable"];
            ObjRightTable = dict["ObjRightTable"];
            OutCnlTable = dict["OutCnlTable"];
            QuantityTable = dict["QuantityTable"];
            RoleTable = dict["RoleTable"];
            RoleRefTable = dict["RoleRefTable"];
            ScriptTable = dict["ScriptTable"];
            UnitTable = dict["UnitTable"];
            UserTable = dict["UserTable"];
            ViewTable = dict["ViewTable"];
            ViewTypeTable = dict["ViewTypeTable"];

            dict = Locale.GetDictionary("Scada.Files");
            FileNotFound = dict["FileNotFound"];
            NamedFileNotFound = dict["NamedFileNotFound"];
            DirectoryNotExists = dict["DirectoryNotExists"];
            LoadAppConfigError = dict["LoadAppConfigError"];
            SaveAppConfigError = dict["SaveAppConfigError"];
            LoadViewError = dict["LoadViewError"];
            SaveViewError = dict["SaveViewError"];

            dict = Locale.GetDictionary("Scada.Format");
            IntegerRequired = dict["IntegerRequired"];
            IntegerInRangeRequired = dict["IntegerInRangeRequired"];
            RealRequired = dict["RealRequired"];
            NonemptyRequired = dict["NonemptyRequired"];
            DateTimeRequired = dict["DateTimeRequired"];
            NotNumber = dict["NotNumber"];
            NotHexadecimal = dict["NotHexadecimal"];
            InvalidParamVal = dict["InvalidParamVal"];
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
        }
    }
}
