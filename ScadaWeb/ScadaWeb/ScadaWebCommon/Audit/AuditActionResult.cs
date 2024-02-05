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
 * Summary  : Specifies the results of actions in the audit log
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
    /// Specifies the results of actions in the audit log.
    /// <para>Задаёт результаты действий в журнале аудита.</para>
    /// </summary>
    public static class AuditActionResult
    {
        static AuditActionResult()
        {
            if (Locale.IsRussian)
            {
                Success = "Успех";
                Failure = "Отказ";
            }
            else
            {
                Success = "Success";
                Failure = "Failure";
            }
        }

        public static string Success { get; private set; }
        public static string Failure { get; private set; }

        public static string FromBool(bool value)
        {
            return value ? Success : Failure;
        }
    }
}
