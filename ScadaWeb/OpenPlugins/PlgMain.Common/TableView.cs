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
 * Summary  : Represents a table view
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2011
 * Modified : 2021
 */

using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Lang;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Scada.Web.Plugins.PlgMain
{
    /// <summary>
    /// Represents a table view.
    /// <para>Представляет табличное представление.</para>
    /// </summary>
    public class TableView : BaseView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TableView(View viewEntity)
            : base(viewEntity)
        {
            Items = new List<TableItem>();
            VisibleItems = new List<TableItem>();
        }


        /// <summary>
        /// Gets the table items.
        /// </summary>
        public List<TableItem> Items { get; }

        /// <summary>
        /// Gets the items that are not hidden.
        /// </summary>
        public List<TableItem> VisibleItems { get; }


        /// <summary>
        /// Loads the view from the specified stream.
        /// </summary>
        public override void LoadView(Stream stream)
        {
            XmlDocument xmlDoc = new();
            xmlDoc.Load(stream);
            XmlElement rootElem = xmlDoc.DocumentElement;

            void CreateItem(XmlElement xmlElem)
            {
                TableItem item = new();
                item.LoadFromXml(xmlElem);

                Items.Add(item);
                AddCnlNum(item.CnlNum);
                AddOutCnlNum(item.OutCnlNum);

                if (!item.Hidden)
                    VisibleItems.Add(item);
            }

            foreach (XmlElement itemElem in rootElem.SelectNodes("Item"))
            {
                CreateItem(itemElem);
            }

            // load old format
            foreach (XmlElement paramElem in rootElem.SelectNodes("Param"))
            {
                CreateItem(paramElem);
            }
        }

        /// <summary>
        /// Binds the view to the configuration database.
        /// </summary>
        public override void Bind(BaseDataSet baseDataSet)
        {
            foreach (TableItem item in Items)
            {
                if (item.CnlNum > 0 && baseDataSet.InCnlTable.GetItem(item.CnlNum) is InCnl inCnl)
                    item.InCnl = inCnl;

                if (item.OutCnlNum > 0 && baseDataSet.OutCnlTable.GetItem(item.OutCnlNum) is OutCnl outCnl)
                    item.OutCnl = outCnl;
            }
        }

        /// <summary>
        /// Loads the view from the specified file.
        /// </summary>
        public bool LoadFromFile(string fileName, out string errMsg)
        {
            try
            {
                using FileStream fileStream = new(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                LoadView(fileStream);
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ScadaUtils.BuildErrorMessage(ex, CommonPhrases.LoadViewError);
                return false;
            }
        }

        /// <summary>
        /// Saves the view to the specified file.
        /// </summary>
        public bool SaveToFile(string fileName, out string errMsg)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                xmlDoc.AppendChild(xmlDecl);

                XmlElement rootElem = xmlDoc.CreateElement("TableView");
                xmlDoc.AppendChild(rootElem);

                foreach (TableItem item in Items)
                {
                    XmlElement itemElem = rootElem.AppendElem("Item");
                    item.SaveToXml(itemElem);
                }

                xmlDoc.Save(fileName);
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ScadaUtils.BuildErrorMessage(ex, CommonPhrases.SaveViewError);
                return false;
            }
        }
    }
}
