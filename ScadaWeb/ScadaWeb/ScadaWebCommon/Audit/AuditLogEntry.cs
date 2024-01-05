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
 * Summary  : Represents a single record in the audit log
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2023
 * Modified : 2023
 */

using Scada.Data.Entities;

namespace Scada.Web.Audit
{
    /// <summary>
    /// Represents a single record in the audit log.
    /// <para>Представляет одну запись в журнале аудита.</para>
    /// </summary>
    public class AuditLogEntry
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public AuditLogEntry()
        {
            ActionTime = DateTime.MinValue;
            Username = null;
            ActionType = null;
            ActionArgs = null;
            ActionResult = null;
            Severity = null;
            Message = null;
        }
        
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public AuditLogEntry(User userEntity)
            : this()
        {
            ActionTime = DateTime.UtcNow;
            Username = userEntity?.Name;
        }


        /// <summary>
        /// Gets the action timestamp (UTC).
        /// </summary>
        public DateTime ActionTime { get; init; }

        /// <summary>
        /// Gets the username who performed the action.
        /// </summary>
        public string Username { get; init; }

        /// <summary>
        /// Gets the action type.
        /// </summary>
        public string ActionType { get; init; }

        /// <summary>
        /// Gets the action arguments.
        /// </summary>
        public string ActionArgs { get; init; }

        /// <summary>
        /// Gets the action result.
        /// </summary>
        public string ActionResult { get; init; }

        /// <summary>
        /// Gets the severity.
        /// </summary>
        public int? Severity { get; init; }

        /// <summary>
        /// Gets the auxiliary message.
        /// </summary>
        public string Message { get; init; }
    }
}
