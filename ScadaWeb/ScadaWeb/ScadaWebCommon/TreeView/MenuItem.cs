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
 * Module   : ScadaWebCommon
 * Summary  : Represents a menu item
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2021
 */

using Scada.Web.Lang;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada.Web.TreeView
{
    /// <summary>
    /// Represents a menu item.
    /// <para>Представляет пункт меню.</para>
    /// </summary>
    public class MenuItem : IWebTreeNode, IComparable<MenuItem>
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public MenuItem()
        {
            Parent = null;
            Text = "";
            Url = "";
            Level = -1;

            SortOrder = MenuItemSortOrder.First;
            Subitems = new List<MenuItem>();
        }


        #region IWebTreeNode
        /// <summary>
        /// Gets or sets the parent tree node.
        /// </summary>
        public IWebTreeNode Parent { get; set; }

        /// <summary>
        /// Gets the child tree nodes.
        /// </summary>
        public IList Children => Subitems;

        /// <summary>
        /// Gets or sets a value indicating whether the node is hidden.
        /// </summary>
        public bool IsHidden => false;

        /// <summary>
        /// Gets the icon URL.
        /// </summary>
        public string IconUrl => null;

        /// <summary>
        /// Gets or sets the node text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets the URL to open when the node is clicked.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets the script to execute when the node is clicked.
        /// </summary>
        /// <remarks>
        /// The script has a higher priority than the URL.
        /// It allows to open a page in a new tab using the browser context menu.
        /// </remarks>
        public string Script => null;

        /// <summary>
        /// Gets or sets the nesting level.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Gets the data attributes as key/value pairs.
        /// </summary>
        public IDictionary<string, string> DataAttrs => null;

        /// <summary>
        /// Determines that the node represents the specified object.
        /// </summary>
        public bool Represents(object obj)
        {
            return obj != null && string.Equals(obj.ToString(), Url, StringComparison.OrdinalIgnoreCase);
        }
        #endregion


        /// <summary>
        /// Gets or sets the sort order.
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// Gets the menu subitems.
        /// </summary>
        public List<MenuItem> Subitems { get; }


        /// <summary>
        /// Compares the current instance with another object of the same type.
        /// </summary>
        public int CompareTo(MenuItem other)
        {
            if (other == null)
            {
                return 1;
            }
            else
            {
                int comp1 = SortOrder.CompareTo(other.SortOrder);

                if (comp1 == 0)
                {
                    int comp2 = string.Compare(Text, other.Text, StringComparison.OrdinalIgnoreCase);
                    return comp2 == 0 
                        ? string.Compare(Url, other.Url, StringComparison.OrdinalIgnoreCase) 
                        : comp2;
                }
                else
                {
                    return comp1;
                }
            }
        }
        
        /// <summary>
        /// Creates a menu item according to the known menu item.
        /// </summary>
        public static MenuItem FromKnownMenuItem(KnownMenuItem knownMenuItem)
        {
            const int OrderCoef = 100;

            return knownMenuItem switch
            {
                KnownMenuItem.Reports => new MenuItem
                {
                    Text = WebPhrases.ReportsMenuItem,
                    SortOrder = (int)KnownMenuItem.Reports * OrderCoef
                },

                KnownMenuItem.Administration => new MenuItem
                {
                    Text = WebPhrases.AdministrationMenuItem,
                    SortOrder = (int)KnownMenuItem.Administration * OrderCoef
                },

                KnownMenuItem.Configuration => new MenuItem
                {
                    Text = WebPhrases.ConfigurationMenuItem,
                    SortOrder = (int)KnownMenuItem.Configuration * OrderCoef
                },

                KnownMenuItem.Registration => new MenuItem
                {
                    Text = WebPhrases.RegistrationMenuItem,
                    SortOrder = (int)KnownMenuItem.Registration * OrderCoef
                },

                KnownMenuItem.Plugins => new MenuItem
                {
                    Text = WebPhrases.PluginsMenuItem,
                    SortOrder = (int)KnownMenuItem.Plugins * OrderCoef
                },

                KnownMenuItem.About => new MenuItem
                {
                    Text = WebPhrases.AboutMenuItem,
                    Url = WebUrl.AboutPage,
                    SortOrder = MenuItemSortOrder.Last
                },

                _ => new MenuItem()
            };
        }
    }
}
