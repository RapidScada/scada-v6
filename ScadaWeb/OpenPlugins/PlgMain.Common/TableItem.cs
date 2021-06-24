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
 * Module   : PlgMain.Common
 * Summary  : Represents a table view item
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Scada.Data.Entities;
using System;
using System.Xml;

namespace Scada.Web.Plugins.PlgMain
{
    /// <summary>
    /// Represents a table view item.
    /// <para>Представляет элемент табличного представления.</para>
    /// </summary>
    public class TableItem
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TableItem()
        {
            CnlNum = 0;
            OutCnlNum = 0;
            Text = "";
            Hidden = false;
            InCnl = null;
            OutCnl = null;
        }


        /// <summary>
        /// Gets or sets the input channel number.
        /// </summary>
        public int CnlNum { get; set; }

        /// <summary>
        /// Gets or sets the output channel number.
        /// </summary>
        public int OutCnlNum { get; set; }

        /// <summary>
        /// Gets or sets the displayed text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item is hidden, but affects event filtering.
        /// </summary>
        public bool Hidden { get; set; }

        /// <summary>
        /// Gets or sets the input channel properties.
        /// </summary>
        public InCnl InCnl { get; set; }

        /// <summary>
        /// Gets or sets the output channel properties.
        /// </summary>
        public OutCnl OutCnl { get; set; }


        /// <summary>
        /// Loads the item from the XML node.
        /// </summary>
        public void LoadFromXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException(nameof(xmlElem));

            CnlNum = xmlElem.GetAttrAsInt("cnlNum");
            OutCnlNum = xmlElem.GetAttrAsInt("outCnlNum");
            Text = xmlElem.InnerText;
            Hidden = xmlElem.GetAttrAsBool("hidden");

            // old format
            if (OutCnlNum == 0)
                OutCnlNum = xmlElem.GetAttrAsInt("ctrlCnlNum");
        }

        /// <summary>
        /// Saves the item into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException(nameof(xmlElem));

            xmlElem.SetAttribute("cnlNum", CnlNum);
            xmlElem.SetAttribute("outCnlNum", OutCnlNum);
            xmlElem.InnerText = Text;
            xmlElem.SetAttribute("hidden", Hidden);
        }
    }
}
