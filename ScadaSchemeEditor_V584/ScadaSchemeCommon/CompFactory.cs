﻿/*
 * Copyright 2017 Mikhail Shiryaev
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
 * Module   : ScadaSchemeCommon
 * Summary  : The base class for creating scheme components
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

using Scada.Scheme.Model;
using System;

namespace Scada.Scheme
{
    /// <summary>
    /// The base class for creating scheme components
    /// <para>Базовый класс для создания компонентов схемы</para>
    /// </summary>
    public abstract class CompFactory
    {
        /// <summary>
        /// Определить, что имена типов равны
        /// </summary>
        protected bool NameEquals(string expectedShortName, string expectedFullName, 
            string actualName, bool nameIsShort)
        {
            return string.Equals(nameIsShort ? expectedShortName : expectedFullName, 
                actualName, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Создать компонент схемы
        /// </summary>
        public abstract BaseComponent CreateComponent(string typeName, bool nameIsShort);
    }
}
