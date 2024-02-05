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
 * Summary  : Defines functionality to write to the audit log
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2023
 * Modified : 2023
 */

using Scada.Web.Audit;

namespace Scada.Web.Services
{
    /// <summary>
    /// Defines functionality to write to the audit log.
    /// <para>Определяет функциональность для записи в журнал аудита.</para>
    /// </summary>
    public interface IAuditLog
    {
        /// <summary>
        /// Writes the entry to the audit log.
        /// </summary>
        void Write(AuditLogEntry entry);
    }
}
