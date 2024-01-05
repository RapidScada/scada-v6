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
 * Module   : Webstation Application
 * Summary  : Implements writing to the audit log
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2023
 * Modified : 2023
 */

using Scada.Web.Audit;
using Scada.Web.Services;
using System;

namespace Scada.Web.Code
{
    /// <summary>
    /// Implements writing to the audit log.
    /// <para>Реализует запись в журнал аудита.</para>
    /// </summary>
    internal class AuditLog : IAuditLog
    {
        private readonly IWebContext webContext;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public AuditLog(IWebContext webContext)
        {
            this.webContext = webContext ?? throw new ArgumentNullException(nameof(webContext));
        }

        /// <summary>
        /// Writes the entry to the audit log.
        /// </summary>
        public void Write(AuditLogEntry entry)
        {
            ArgumentNullException.ThrowIfNull(entry, nameof(entry));
            webContext.PluginHolder.WriteToAuditLog(entry);
        }
    }
}
