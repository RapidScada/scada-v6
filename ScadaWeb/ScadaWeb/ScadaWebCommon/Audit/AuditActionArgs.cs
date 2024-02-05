﻿/*
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
 * Summary  : Provides helper methods for representing action arguments
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2023
 * Modified : 2023
 */

namespace Scada.Web.Audit
{
    /// <summary>
    /// Provides helper methods for representing action arguments.
    /// <para>Предоставляет вспомогательные методы для представления аргументов действия.</para>
    /// </summary>
    public static class AuditActionArgs
    {
        /// <summary>
        /// Converts the specified object containing action arguments to a string.
        /// </summary>
        public static string FromObject(object args)
        {
            return args?.ToString();
        }
    }
}
