// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml;

namespace Scada.Doc.Code
{
    /// <summary>
    /// Represents a table of contents.
    /// <para>Представляет оглавление.</para>
    /// </summary>
    public class Toc
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Toc()
        {
            Items = new List<TocItem>();
        }


        /// <summary>
        /// Gets the items.
        /// </summary>
        public List<TocItem> Items { get; }


        /// <summary>
        /// Loads the item and subitems recursively.
        /// </summary>
        private void LoadItem(TocItem item, XmlNode itemNode)
        {
            item.Text = itemNode.GetChildAsString("Text");
            item.Url = itemNode.GetChildAsString("Url");

            foreach (XmlNode subitemNode in itemNode.SelectNodes("Item"))
            {
                TocItem subitem = new();
                LoadItem(subitem, subitemNode);
                item.Subitems.Add(subitem);
            }
        }

        /// <summary>
        /// Loads the table of contents from the specified file.
        /// </summary>
        public void LoadFromFile(string fileName)
        {
            Items.Clear();
            XmlDocument xmlDoc = new();
            xmlDoc.Load(fileName);

            foreach (XmlNode itemNode in xmlDoc.DocumentElement.SelectNodes("Item"))
            {
                TocItem item = new();
                LoadItem(item, itemNode);
                Items.Add(item);
            }
        }
    }
}
