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
 * Module   : ScadaCommon.Forms
 * Summary  : The class provides extension methods for a ListView control
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using System;
using System.Windows.Forms;

namespace Scada.Forms
{
    /// <summary>
    /// The class provides extension methods for a ListView control.
    /// <para>Класс, предоставляющий методы расширения для элемента управления ListView.</para>
    /// </summary>
    public static class ListViewExtensions
    {
        /// <summary>
        /// Inserts the specified list item after the selected item.
        /// </summary>
        public static void InsertItem(this ListView listView, ListViewItem item, bool updateOrder = false)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            try
            {
                listView.BeginUpdate();
                int index = listView.SelectedIndices.Count > 0 
                    ? listView.SelectedIndices[0] + 1 
                    : listView.Items.Count;
                listView.Items.Insert(index, item).Selected = true;

                if (updateOrder)
                {
                    for (int i = index, cnt = listView.Items.Count; i < cnt; i++)
                    {
                        listView.Items[i].Text = (i + 1).ToString();
                    }
                }
            }
            finally
            {
                listView.EndUpdate();
                listView.Focus();
            }
        }

        /// <summary>
        /// Moves up the selected list item.
        /// </summary>
        public static bool MoveUpSelectedItem(this ListView listView, bool updateOrder = false)
        {
            if (listView.SelectedIndices.Count > 0)
            {
                int index = listView.SelectedIndices[0];

                if (index > 0)
                {
                    try
                    {
                        listView.BeginUpdate();
                        ListViewItem item = listView.Items[index];
                        ListViewItem prevItem = listView.Items[index - 1];

                        listView.Items.RemoveAt(index);
                        listView.Items.Insert(index - 1, item);

                        if (updateOrder)
                        {
                            item.Text = index.ToString();
                            prevItem.Text = (index + 1).ToString();
                        }
                    }
                    finally
                    {
                        listView.EndUpdate();
                        listView.Focus();
                    }

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Moves down the selected list item.
        /// </summary>
        public static bool MoveDownSelectedItem(this ListView listView, bool updateOrder = false)
        {
            if (listView.SelectedIndices.Count > 0)
            {
                int index = listView.SelectedIndices[0];

                if (index < listView.Items.Count - 1)
                {
                    try
                    {
                        listView.BeginUpdate();
                        ListViewItem item = listView.Items[index];
                        ListViewItem nextItem = listView.Items[index + 1];

                        listView.Items.RemoveAt(index);
                        listView.Items.Insert(index + 1, item);

                        if (updateOrder)
                        {
                            item.Text = (index + 2).ToString();
                            nextItem.Text = (index + 1).ToString();
                        }
                    }
                    finally
                    {
                        listView.EndUpdate();
                        listView.Focus();
                    }

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Removes the selected list item.
        /// </summary>
        public static bool RemoveSelectedItem(this ListView listView, bool updateOrder = false)
        {
            if (listView.SelectedIndices.Count > 0)
            {
                try
                {
                    listView.BeginUpdate();
                    int index = listView.SelectedIndices[0];
                    listView.Items.RemoveAt(index);

                    if (listView.Items.Count > 0)
                    {
                        // select an item
                        if (index >= listView.Items.Count)
                            index = listView.Items.Count - 1;
                        listView.Items[index].Selected = true;

                        // update item numbers
                        if (updateOrder)
                        {
                            for (int i = index, cnt = listView.Items.Count; i < cnt; i++)
                            {
                                listView.Items[i].Text = (i + 1).ToString();
                            }
                        }
                    }
                }
                finally
                {
                    listView.EndUpdate();
                    listView.Focus();
                }

                return true;
            }

            return false;
        }
    }
}
