// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#pragma warning disable CA1806 // Do not ignore method results

using Scada.Data.Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Scada.Forms
{
    /// <summary>
    /// Represents a form for selecting entities of the configuration database.
    /// <para>Представляет форму для выбора сущностей базы конфигурации.</para>
    /// </summary>
    public partial class FrmEntitySelect : Form
    {
        /// <summary>
        /// Represents an item that can be selected.
        /// <para>Представляет элемент, который может быть выбран.</para>
        /// </summary>
        private class SelectableItem : INotifyPropertyChanged
        {
            private bool selected;
            public bool Selected
            { 
                get 
                { 
                    return selected; 
                } 
                set 
                { 
                    selected = value; 
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Selected))); 
                } 
            }
            public int ID { get; set; }
            public string Name { get; set; }
            public string Code { get; set; }
            public string Descr { get; set; }

            public event PropertyChangedEventHandler PropertyChanged;
        }

        /// <summary>
        /// Contains item property descriptors.
        /// <para>Содержит дескрипторы свойств элемента.</para>
        /// </summary>
        private struct ItemProps
        {
            public ItemProps(Type itemType)
            {
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(itemType);
                NameProp = props.Find("Name", false);
                CodeProp = props.Find("Code", false);
                DescrProp = props.Find("Descr", false);
            }
            public PropertyDescriptor NameProp { get; }
            public PropertyDescriptor CodeProp { get; }
            public PropertyDescriptor DescrProp { get; }
        }

        private readonly IBaseTable baseTable;     // the table containing entities to select
        private BindingList<SelectableItem> items; // the items to select


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmEntitySelect()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmEntitySelect(IBaseTable baseTable)
            : this()
        {
            this.baseTable = baseTable ?? throw new ArgumentNullException(nameof(baseTable));
            items = null;
            MultiSelect = true;
            SelectedIDs = null;
            SelectedID = 0;
        }


        /// <summary>
        /// Gets or sets a value indicating whether multiple items can be selected.
        /// </summary>
        public bool MultiSelect { get; set; }

        /// <summary>
        /// Gets or sets the IDs of the selected entities.
        /// </summary>
        public ICollection<int> SelectedIDs { get; set; }

        /// <summary>
        /// Gets or sets the ID of the single selected entity.
        /// </summary>
        public int SelectedID { get; set; }


        /// <summary>
        /// Creates and shows table items.
        /// </summary>
        private void FillTable()
        {
            // prepare table data
            HashSet<int> idSet = MultiSelect && SelectedIDs != null
                ? new HashSet<int>(SelectedIDs)
                : new HashSet<int>();

            if (!MultiSelect && SelectedID > 0)
                idSet.Add(SelectedID);

            ItemProps srcProps = new(baseTable.ItemType);
            ItemProps destProps = new(typeof(SelectableItem));
            bool nameExists = srcProps.NameProp != null;
            bool codeExists = srcProps.CodeProp != null;
            bool descrExists = srcProps.DescrProp != null;
            items = new BindingList<SelectableItem>();

            foreach (object srcItem in baseTable.EnumerateItems())
            {
                SelectableItem item = new() { ID = baseTable.GetPkValue(srcItem) };
                item.Selected = idSet.Contains(item.ID);

                if (nameExists)
                    destProps.NameProp.SetValue(item, srcProps.NameProp.GetValue(srcItem));

                if (codeExists)
                    destProps.CodeProp.SetValue(item, srcProps.CodeProp.GetValue(srcItem));

                if (descrExists)
                    destProps.DescrProp.SetValue(item, srcProps.DescrProp.GetValue(srcItem));

                if (!MultiSelect)
                    item.PropertyChanged += Item_PropertyChanged;

                items.Add(item);
            }

            // display data
            colName.Visible = nameExists;
            colCode.Visible = codeExists;
            colDescr.Visible = descrExists;
            dataGridView.DataSource = items;
            dataGridView.AutoSizeColumns();
        }

        /// <summary>
        /// Applies the object filter.
        /// </summary>
        private void ApplyFilter()
        {
            string filterText = txtFilter.Text.Trim();
            bool onlySelected = chkOnlySelected.Checked;

            if (filterText != "")
            {
                int.TryParse(filterText, out int id);
                IEnumerable<SelectableItem> filteredItems = items.Where(item =>
                    (item.ID == id ||
                    StringContains(item.Name, filterText) ||
                    StringContains(item.Code, filterText) ||
                    StringContains(item.Descr, filterText)) &&
                    (!onlySelected || item.Selected));
                dataGridView.DataSource = new BindingList<SelectableItem>(filteredItems.ToList());
            }
            else if (onlySelected)
            {
                IEnumerable<SelectableItem> filteredItems = items.Where(item => item.Selected);
                dataGridView.DataSource = new BindingList<SelectableItem>(filteredItems.ToList());
            }
            else
            {
                dataGridView.DataSource = items;
            }
        }

        /// <summary>
        /// Determines whether the string contains the specified text.
        /// </summary>
        private static bool StringContains(string s, string text)
        {
            return !string.IsNullOrEmpty(s) && s.Contains(text, StringComparison.OrdinalIgnoreCase);
        }


        private void FrmEntitySelect_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            Text = string.Format(Text, baseTable.Name);
            FillTable();
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // deselect other items
            if (sender is SelectableItem changedItem && changedItem.Selected && !MultiSelect)
            {
                foreach (SelectableItem item in items)
                {
                    if (item.Selected && item.ID != changedItem.ID)
                        item.Selected = false;
                }
            }
        }

        private void dataGridView_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            dataGridView.EndEdit();
        }

        private void txtFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                ApplyFilter();
        }

        private void btnApplyFilter_Click(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void chkOnlySelected_CheckedChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            // get IDs of the selected items
            SelectedIDs = (from item in items 
                   where item.Selected
                   select item.ID).ToArray();

            SelectedID = SelectedIDs.FirstOrDefault();
            DialogResult = DialogResult.OK;
        }
    }
}
