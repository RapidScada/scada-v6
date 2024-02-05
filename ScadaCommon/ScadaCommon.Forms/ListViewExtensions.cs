// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
            ArgumentNullException.ThrowIfNull(item, nameof(item));

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
        /// Gets the first selected list item.
        /// </summary>
        public static ListViewItem GetSelectedItem(this ListView listView)
        {
            return listView.SelectedItems.Count > 0 ? listView.SelectedItems[0] : null;
        }

        /// <summary>
        /// Gets an object associated with the first selected list item.
        /// </summary>
        public static object GetSelectedObject(this ListView listView)
        {
            return listView.GetSelectedItem()?.Tag;
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
                        listView.Focus(); // to avoid NullReferenceException on .NET 6
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
                        listView.Focus(); // to avoid NullReferenceException on .NET 6
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
                    listView.Focus(); // to avoid NullReferenceException on .NET 6
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
