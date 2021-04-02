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
 * Modified : 2020
 */

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

namespace Scada
{
    /// <summary>
    /// The common phrases for the entire software package.
    /// <para>Общие фразы для всего программного комплекса.</para>
    /// </summary>
    public static class CommonPhrases
    {
        // Scada.Application
        public static string ServerAppName { get; private set; }
        public static string CommAppName { get; private set; }
        public static string WebAppName { get; private set; }
        public static string UnhandledException { get; private set; }
        public static string ExecutionImpossible { get; private set; }
        public static string StartLogic { get; private set; }
        public static string LogicIsAlreadyStarted { get; private set; }
        public static string StartLogicError { get; private set; }
        public static string LogicIsStopped { get; private set; }
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
        public static string LoadAppConfigError { get; private set; }
        public static string SaveAppConfigError { get; private set; }

        // Scada.Format
        public static string UndefinedSign { get; private set; }
        public static string NotNumber { get; private set; }
        public static string NotHexadecimal { get; private set; }
        public static string InvalidParamVal { get; private set; }

        static CommonPhrases()
        {
            // the phrases below may be required before loading dictionaries
            if (Locale.IsRussian)
            {
                UnhandledException = "Unhandled exception";
                ExecutionImpossible = "Normal program execution is impossible";
            }
            else
            {
                UnhandledException = "Необработанное исключение";
                ExecutionImpossible = "Нормальная работа программы невозможна";
            }
        }

        public static void Init()
        {
            LocaleDict dict = Locale.GetDictionary("Scada.Application");
            ServerAppName = dict.GetPhrase("ServerAppName");
            CommAppName = dict.GetPhrase("CommAppName");
            WebAppName = dict.GetPhrase("WebAppName");
            UnhandledException = dict.GetPhrase("UnhandledException");
            ExecutionImpossible = dict.GetPhrase("ExecutionImpossible");
            StartLogic = dict.GetPhrase("StartLogic");
            LogicIsAlreadyStarted = dict.GetPhrase("LogicIsAlreadyStarted");
            StartLogicError = dict.GetPhrase("StartLogicError");
            LogicIsStopped = dict.GetPhrase("LogicIsStopped");
            UnableToStopLogic = dict.GetPhrase("UnableToStopLogic");
            StopLogicError = dict.GetPhrase("StopLogicError");
            LogicCycleError = dict.GetPhrase("LogicCycleError");
            ThreadFatalError = dict.GetPhrase("ThreadFatalError");
            WriteInfoError = dict.GetPhrase("WriteInfoError");
            ConnectionNotFound = dict.GetPhrase("ConnectionNotFound");

            dict = Locale.GetDictionary("Scada.ConfigBase");
            ArchiveTable = dict.GetPhrase("ArchiveTable");
            CmdTypeTable = dict.GetPhrase("CmdTypeTable");
            CmdValTable = dict.GetPhrase("CmdValTable");
            CnlStatusTable = dict.GetPhrase("CnlStatusTable");
            CnlTypeTable = dict.GetPhrase("CnlTypeTable");
            CommLineTable = dict.GetPhrase("CommLineTable");
            DataTypeTable = dict.GetPhrase("DataTypeTable");
            DeviceTable = dict.GetPhrase("DeviceTable");
            DevTypeTable = dict.GetPhrase("DevTypeTable");
            FormatTable = dict.GetPhrase("FormatTable");
            InCnlTable = dict.GetPhrase("InCnlTable");
            LimTable = dict.GetPhrase("LimTable");
            ObjTable = dict.GetPhrase("ObjTable");
            ObjRightTable = dict.GetPhrase("ObjRightTable");
            OutCnlTable = dict.GetPhrase("OutCnlTable");
            QuantityTable = dict.GetPhrase("QuantityTable");
            RoleTable = dict.GetPhrase("RoleTable");
            RoleRefTable = dict.GetPhrase("RoleRefTable");
            ScriptTable = dict.GetPhrase("ScriptTable");
            UnitTable = dict.GetPhrase("UnitTable");
            UserTable = dict.GetPhrase("UserTable");
            ViewTable = dict.GetPhrase("ViewTable");
            ViewTypeTable = dict.GetPhrase("ViewTypeTable");

            dict = Locale.GetDictionary("Scada.Files");
            FileNotFound = dict.GetPhrase("FileNotFound");
            NamedFileNotFound = dict.GetPhrase("NamedFileNotFound");
            LoadAppConfigError = dict.GetPhrase("LoadAppConfigError");
            SaveAppConfigError = dict.GetPhrase("SaveAppConfigError");

            dict = Locale.GetDictionary("Scada.Format");
            UndefinedSign = dict.GetPhrase("UndefinedSign");
            NotNumber = dict.GetPhrase("NotNumber");
            NotHexadecimal = dict.GetPhrase("NotHexadecimal");
            InvalidParamVal = dict.GetPhrase("InvalidParamVal");
        }
    }
}
