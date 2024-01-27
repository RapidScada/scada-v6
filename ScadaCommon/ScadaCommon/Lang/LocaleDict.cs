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
 * Module   : ScadaCommon
 * Summary  : Represents a dictionary that contains phrases in a certain language
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2021
 */

using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Scada.Lang
{
    /// <summary>
    /// Represents a dictionary that contains phrases in a certain language.
    /// <para>Представляет словарь, который содержит фразы на определённом языке.</para>
    /// </summary>
    public class LocaleDict : DynamicObject
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public LocaleDict(string key)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
            Phrases = new Dictionary<string, string>();
        }


        /// <summary>
        /// Gets the dictionary key.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Gets the phrase associated with the specified key or an empty phrase if the key is not found.
        /// </summary>
        public string this[string key]
        {
            get
            {
                return GetPhrase(key);
            }
        }

        /// <summary>
        /// Gets the phrases contained in the dictionary.
        /// </summary>
        public Dictionary<string, string> Phrases { get; }


        /// <summary>
        /// Gets the phrase associated with the specified key or an empty phrase if the key is not found.
        /// </summary>
        public string GetPhrase(string key)
        {
            return Phrases.TryGetValue(key, out string phrase) ? phrase : "[" + key + "]";
        }

        /// <summary>
        /// Provides the implementation for operations that get member values.
        /// </summary>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = GetPhrase(binder.Name);
            return true;
        }

        /// <summary>
        /// Provides the implementation for operations that set member values.
        /// </summary>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            Phrases[binder.Name] = value?.ToString() ?? "";
            return true;
        }
    }
}
