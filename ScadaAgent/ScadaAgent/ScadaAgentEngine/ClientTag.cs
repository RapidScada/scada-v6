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
 * Module   : ScadaAgentEngine
 * Summary  : Represents an object associated with a connected client
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Scada.Lang;

namespace Scada.Agent.Engine
{
    /// <summary>
    /// Represents an object associated with a connected client.
    /// <para>Представляет объект, связанный с подключенным клиентом.</para>
    /// </summary>
    internal class ClientTag
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ClientTag(bool isReverse)
        {
            IsReverse = isReverse;
        }


        /// <summary>
        /// Gets a value indicating whether the current tag belongs to a reverse client.
        /// </summary>
        public bool IsReverse { get; }

        /// <summary>
        /// Gets or sets the instance.
        /// </summary>
        public ScadaInstance Instance { get; set; } = null;


        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            if (IsReverse)
            {
                return Locale.IsRussian
                    ? (Instance?.Name ?? "Экземпляр не найден") + "; Обратный клиент"
                    : (Instance?.Name ?? "Instanse not found") + "; Reverse client";
            }
            else
            {
                return Instance?.Name ?? "";
            }
        }
    }
}
