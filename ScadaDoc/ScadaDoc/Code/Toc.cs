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
        /// Gets the items.
        /// </summary>
        public List<MenuItem> Items { get; } = new();


        /// <summary>
        /// Loads the item and subitems recursively.
        /// </summary>
        private static void LoadItem(MenuItem item, XmlNode itemNode)
        {
            item.Text = itemNode.GetChildAsString("Text");
            item.Url = itemNode.GetChildAsString("Url");

            if (itemNode.SelectSingleNode("Subitems") is XmlNode subitemsNode)
            {
                foreach (XmlNode subitemNode in subitemsNode.SelectNodes("Item"))
                {
                    MenuItem subitem = new();
                    LoadItem(subitem, subitemNode);
                    item.Subitems.Add(subitem);
                }
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
                MenuItem item = new();
                LoadItem(item, itemNode);
                Items.Add(item);
            }
        }

        /// <summary>
        /// Enumerates all items recursively.
        /// </summary>
        public IEnumerable<MenuItem> EnumerateAllItems()
        {
            static IEnumerable<MenuItem> EnumerateItems(IEnumerable<MenuItem> items)
            {
                foreach (MenuItem item in items)
                {
                    yield return item;

                    foreach (MenuItem subitem in EnumerateItems(item.Subitems))
                    {
                        yield return subitem;
                    }
                }
            }

            return EnumerateItems(Items);
        }
    }
}
