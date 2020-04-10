/*
 * Copyright 2020 Mikhail Shiryaev
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
 * Module   : ScadaData
 * Summary  : The common phrases for the entire software package
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2020
 */

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

namespace Scada
{
    /// <summary>
    /// The common phrases for the entire software package.
    /// <para>Общие фразы для всего программного комплекса.</para>
    /// </summary>
    public static class CommonPhrases
    {
        // Scada.Format
        public static string NotNumber { get; private set; }
        public static string NotHexadecimal { get; private set; }

        // Scada.Xml
        public static string IncorrectXmlNodeVal { get; private set; }
        public static string IncorrectXmlAttrVal { get; private set; }
        public static string IncorrectXmlParamVal { get; private set; }

        public static void Init()
        {
            LocaleDict dict = Locale.GetDictionary("Scada.Format");
            NotNumber = dict.GetPhrase("NotNumber");
            NotHexadecimal = dict.GetPhrase("NotHexadecimal");

            dict = Locale.GetDictionary("Scada.Xml");
            IncorrectXmlNodeVal = dict.GetPhrase("IncorrectXmlNodeVal");
            IncorrectXmlAttrVal = dict.GetPhrase("IncorrectXmlAttrVal");
            IncorrectXmlParamVal = dict.GetPhrase("IncorrectXmlParamVal");
        }
    }
}
