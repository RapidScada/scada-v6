/*
 * Copyright 2024 Rapid Software LLC
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
 * Module   : ScadaWebCommon
 * Summary  : Specifies the types of actions in the audit log
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2023
 * Modified : 2023
 */

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

using Scada.Lang;

namespace Scada.Web.Audit
{
    /// <summary>
    /// Specifies the types of actions in the audit log.
    /// <para>Задаёт типы действий в журнале аудита.</para>
    /// </summary>
    public static class AuditActionType
    {
        static AuditActionType()
        {
            if (Locale.IsRussian)
            {
                Login = "Вход в систему";
                Logout = "Выход из системы";
                OpenView = "Просмотр представления";
                OpenChart = "Просмотр графика";
                GenerateReport = "Формирование отчёта";
                SendCommand = "Отправка команды";
            }
            else
            {
                Login = "Login";
                Logout = "Logout";
                OpenView = "Open view";
                OpenChart = "Open chart";
                GenerateReport = "Generate report";
                SendCommand = "Send command";
            }
        }

        public static string Login { get; private set; }
        public static string Logout { get; private set; }
        public static string OpenView { get; private set; }
        public static string OpenChart { get; private set; }
        public static string GenerateReport { get; private set; }
        public static string SendCommand { get; private set; }
    }
}
