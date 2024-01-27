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
 * Summary  : Represents a list of custom options
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2021
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Scada.Config
{
    /// <summary>
    /// Represents a list of custom options.
    /// <para>Представляет список пользовательских параметров.</para>
    /// </summary>
    [Serializable]
    public class OptionList : SortedList<string, string>
    {
        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            if (xmlNode == null)
                throw new ArgumentNullException(nameof(xmlNode));

            foreach (XmlElement optionElem in xmlNode.SelectNodes("Option"))
            {
                this[optionElem.GetAttrAsString("name")] = optionElem.GetAttrAsString("value");
            }
        }

        /// <summary>
        /// Saves the options into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException(nameof(xmlElem));

            foreach (KeyValuePair<string, string> pair in this)
            {
                xmlElem.AppendOptionElem(pair.Key, pair.Value);
            }
        }

        /// <summary>
        /// Copies the options from the current list to the specified list.
        /// </summary>
        public void CopyTo(OptionList list)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            foreach (KeyValuePair<string, string> pair in this)
            {
                list[pair.Key] = pair.Value;
            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            bool addLine = false;

            foreach (KeyValuePair<string, string> pair in this)
            {
                if (addLine)
                    stringBuilder.AppendLine();
                else
                    addLine = true;

                stringBuilder
                    .Append(pair.Key)
                    .Append(" = ")
                    .Append(pair.Value);
            }

            return stringBuilder.ToString();
        }
    }
}
