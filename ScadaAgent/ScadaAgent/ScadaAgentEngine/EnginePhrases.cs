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
 * Summary  : The phrases used by this library
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Scada.Lang;

namespace Scada.Agent.Engine
{
    /// <summary>
    /// The phrases used by this library.
    /// <para>Фразы, используемые данной библиотекой.</para>
    /// </summary>
    internal static class EnginePhrases
    {
        public static string InstanceLocked { get; private set; }

        public static void Init()
        {
            // set phrases that are used in the bilingual service logic, depending on the locale
            if (Locale.IsRussian)
            {
                InstanceLocked = "Невозможно выполнить операцию {0}, потому что экземпляр {1} заблокирован";
            }
            else
            {
                InstanceLocked = "Unable to perform the {0} operation because the {1} instance is locked";
            }
        }
    }
}
