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
 * Summary  : Represents a dictionary of phrases with dynamic run-time behavior
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using System;
using System.Dynamic;

namespace Scada.Lang
{
    /// <summary>
    /// Represents a dictionary of phrases with dynamic run-time behavior.
    /// <para>Представляет словарь фраз с динамическим поведением во время выполнения.</para>
    /// </summary>
    public class DynamicLocaleDict : DynamicObject
    {
        private readonly LocaleDict localeDict;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DynamicLocaleDict(LocaleDict localeDict)
        {
            this.localeDict = localeDict ?? throw new ArgumentNullException(nameof(localeDict));
        }


        /// <summary>
        /// Provides the implementation for operations that get member values.
        /// </summary>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = localeDict[binder.Name];
            return true;
        }

        /// <summary>
        /// Provides the implementation for operations that set member values.
        /// </summary>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            localeDict.Phrases[binder.Name] = value?.ToString() ?? "";
            return true;
        }
    }
}
