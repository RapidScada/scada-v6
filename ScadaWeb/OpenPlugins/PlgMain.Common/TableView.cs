// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Data.Tables;
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
    public class TableView : ViewBase
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
        public List<TableItem> Items { get; protected set; }

        /// <summary>
        /// Gets the items that are not hidden.
        /// </summary>
        public List<TableItem> VisibleItems { get; }


        /// <summary>
        /// Adds the item to the lists.
        /// </summary>
        protected void AddItem(TableItem item)
        {
            Items.Add(item);
            AddCnlNum(item.CnlNum);

            if (!item.Hidden)
                VisibleItems.Add(item);
        }

        /// <summary>
        /// Removes the device name from the beginning of the channel name.
        /// </summary>
        protected static string TrimDeviceName(string deviceName, string cnlName)
        {
            return !string.IsNullOrEmpty(deviceName) && !string.IsNullOrEmpty(cnlName) && 
                cnlName.StartsWith(deviceName, StringComparison.Ordinal)
                ? cnlName[deviceName.Length..].TrimStart('-', '.', ' ')
                : cnlName;
        }


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
        public override void Bind(ConfigDataset configDataset)
        {
            List<TableItem> initialItems = Items;
            Items = new List<TableItem>();

            foreach (TableItem item in initialItems)
            {
                if (item.CnlNum > 0)
                {
                    // update item according to channel
                    Cnl cnl = configDataset.CnlTable.GetItem(item.CnlNum);

                    if (cnl != null)
                    {
                        item.Cnl = cnl;

                        if (item.AutoText)
                            item.Text = cnl.Name;

                        if (cnl.IsNumericArray())
                            item.Text += "[0]";
                    }

                    // add item
                    AddItem(item);

                    // add items for array or string
                    if (cnl != null && cnl.IsArray())
                    {
                        bool hidden = cnl.IsString();

                        for (int i = 1, len = cnl.DataLen.Value; i < len; i++)
                        {
                            int newCnlNum = cnl.CnlNum + i;
                            AddItem(new TableItem
                            {
                                CnlNum = newCnlNum,
                                Hidden = hidden,
                                AutoText = item.AutoText,
                                Text = item.Text + "[" + i + "]",
                                Cnl = configDataset.CnlTable.GetItem(newCnlNum)
                            });
                        }
                    }
                }
                else if (item.DeviceNum <= 0)
                {
                    AddItem(item);
                }
                else if (configDataset.DeviceTable.GetItem(item.DeviceNum) is Device device)
                {
                    // create item for device title
                    AddItem(new TableItem
                    {
                        Hidden = item.Hidden,
                        AutoText = item.AutoText,
                        Text = item.AutoText ? device.Name : item.Text
                    });

                    // create items for device channels
                    // note that Webstation duplicates channels for arrays and strings
                    int hiddenCnlNum = 0;

                    foreach (Cnl cnl in 
                        configDataset.CnlTable.Select(new TableFilter("DeviceNum", item.DeviceNum), true))
                    {
                        AddItem(new TableItem
                        {
                            CnlNum = cnl.CnlNum,
                            Hidden = item.Hidden || cnl.CnlNum <= hiddenCnlNum,
                            AutoText = true,
                            Text = TrimDeviceName(device.Name, cnl.Name),
                            Cnl = cnl
                        });

                        if (cnl.IsArray() && cnl.IsString())
                            hiddenCnlNum = cnl.CnlNum + cnl.DataLen.Value - 1;
                    }
                }
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
                errMsg = ex.BuildErrorMessage(CommonPhrases.LoadViewError);
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
                XmlDocument xmlDoc = new();
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
                errMsg = ex.BuildErrorMessage(CommonPhrases.SaveViewError);
                return false;
            }
        }
    }
}
