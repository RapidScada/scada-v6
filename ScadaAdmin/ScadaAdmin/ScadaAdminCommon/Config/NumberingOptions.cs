/*
 * Copyright 2023 Rapid Software LLC
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
 * Module   : ScadaAdminCommon
 * Summary  : Represents record numbering options in a configuration base table
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2023
 * Modified : 2023
 */

using System;
using System.Xml;

namespace Scada.Admin.Config
{
    /// <summary>
    /// Represents record numbering options in a configuration base table.
    /// <para>Представляет параметры нумерации записей в таблице базы конфигурации.</para>
    /// </summary>
    public class NumberingOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public NumberingOptions()
        {
            Multiplicity = 100;
            Shift = 1;
            Gap = 10;
        }


        /// <summary>
        /// Gets or sets the multiplicity of the first record of a group.
        /// </summary>
        public int Multiplicity { get; set; }

        /// <summary>
        /// Gets or sets the shift of the first record of a group.
        /// </summary>
        public int Shift { get; set; }

        /// <summary>
        /// Gets or sets the gap between record IDs of different groups.
        /// </summary>
        public int Gap { get; set; }


        /// <summary>
        /// Adjusts the next record ID according to the numbering options.
        /// </summary>
        public int AdjustID(int nextID)
        {
            return AdjustID(nextID, true);
        }

        /// <summary>
        /// Adjusts the next record ID according to the numbering options.
        /// </summary>
        public int AdjustID(int nextID, bool keepGap)
        {
            int adjustedID = keepGap && Gap > 0 ? nextID + Gap : nextID;

            if (Multiplicity > 1)
            {
                int remainder = (adjustedID - Shift) % Multiplicity;

                if (remainder > 0)
                    adjustedID = adjustedID - remainder + Multiplicity;
            }

            return adjustedID;
        }

        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public virtual void LoadFromXml(XmlNode xmlNode)
        {
            ArgumentNullException.ThrowIfNull(xmlNode, nameof(xmlNode));
            Multiplicity = xmlNode.GetChildAsInt("Multiplicity", Multiplicity);
            Shift = xmlNode.GetChildAsInt("Shift", Shift);
            Gap = xmlNode.GetChildAsInt("Gap", Gap);
        }

        /// <summary>
        /// Saves the options into the XML node.
        /// </summary>
        public virtual void SaveToXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            xmlElem.AppendElem("Multiplicity", Multiplicity);
            xmlElem.AppendElem("Shift", Shift);
            xmlElem.AppendElem("Gap", Gap);
        }
    }
}
