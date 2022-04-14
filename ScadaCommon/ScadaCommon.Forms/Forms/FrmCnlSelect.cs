// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#pragma warning disable CA1806 // Do not ignore method results

using Scada.Data.Entities;
using Scada.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Scada.Forms.Forms
{
    /// <summary>
    /// Represents a form for selecting channels of the configuration database.
    /// <para>Представляет форму для выбора каналов базы конфигурации.</para>
    /// </summary>
    public partial class FrmCnlSelect : Form
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
            public int CnlNum { get; set; }
            public string Name { get; set; }
            public int ObjNum { get; set; }
            public int DeviceNum { get; set; }

            public event PropertyChangedEventHandler PropertyChanged;
        }

        private readonly BaseDataSet baseDataSet;              // the configuration database cache
        private BindingList<SelectableItem> items;             // the items to select
        private Dictionary<int, SelectableItem> selectedItems; // the selected items


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmCnlSelect()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmCnlSelect(BaseDataSet baseDataSet)
            : this()
        {
            this.baseDataSet = baseDataSet ?? throw new ArgumentNullException(nameof(baseDataSet));
            items = null;
            selectedItems = null;

            MultiSelect = true;
            SelectedCnlNums = null;
            SelectedCnlNum = 0;
        }


        /// <summary>
        /// Gets or sets a value indicating whether multiple channels can be selected.
        /// </summary>
        public bool MultiSelect { get; set; }

        /// <summary>
        /// Gets or sets the selected channel numbers.
        /// </summary>
        public ICollection<int> SelectedCnlNums { get; set; }

        /// <summary>
        /// Gets or sets the single selected channel number.
        /// </summary>
        public int SelectedCnlNum { get; set; }


        /// <summary>
        /// Creates and shows table items.
        /// </summary>
        private void FillTable()
        {
            // get selected channel numbers
            HashSet<int> selectedIdSet = MultiSelect && SelectedCnlNums != null
                ? new HashSet<int>(SelectedCnlNums)
                : new HashSet<int>();

            if (!MultiSelect && SelectedCnlNum > 0)
                selectedIdSet.Add(SelectedCnlNum);

            // prepare table data
            items = new BindingList<SelectableItem>();
            selectedItems = new Dictionary<int, SelectableItem>();

            foreach (Cnl srcItem in baseDataSet.CnlTable.EnumerateItems())
            {
                SelectableItem item = new() 
                {
                    Selected = selectedIdSet.Contains(srcItem.CnlNum),
                    CnlNum = srcItem.CnlNum,
                    Name = srcItem.Name,
                    ObjNum = srcItem.ObjNum ?? 0,
                    DeviceNum = srcItem.DeviceNum ?? 0
                };

                item.PropertyChanged += Item_PropertyChanged;
                items.Add(item);

                if (item.Selected)
                    selectedItems[item.CnlNum] = item;
            }

            // display data
            dataGridView.AutoGenerateColumns = false;
            dataGridView.DataSource = items;
            dataGridView.AutoSizeColumns();
        }

        /// <summary>
        /// Prepares the filter combo boxes.
        /// </summary>
        private void PrepareFilter()
        {
            // object filter
            List<Obj> objs = new(baseDataSet.ObjTable.ItemCount + 1);
            objs.Add(new Obj { ObjNum = 0, Name = " " });
            objs.AddRange(baseDataSet.ObjTable.Enumerate().OrderBy(obj => obj.Name));

            cbObj.ValueMember = "ObjNum";
            cbObj.DisplayMember = "Name";
            cbObj.DataSource = objs;
            cbObj.SelectedIndexChanged += btnApplyFilter_Click;

            // device filter
            List<Device> devices = new(baseDataSet.DeviceTable.ItemCount + 1);
            devices.Add(new Device { DeviceNum = 0, Name = " " });
            devices.AddRange(baseDataSet.DeviceTable.Enumerate().OrderBy(device => device.Name));

            cbDevice.ValueMember = "DeviceNum";
            cbDevice.DisplayMember = "Name";
            cbDevice.DataSource = devices;
            cbDevice.SelectedIndexChanged += btnApplyFilter_Click;
        }

        /// <summary>
        /// Applies the filter.
        /// </summary>
        private void ApplyFilter()
        {
            string filterText = txtFilter.Text.Trim();
            int objNum = (int)cbObj.SelectedValue;
            int deviceNum = (int)cbDevice.SelectedValue;
            bool onlySelected = chkOnlySelected.Checked;

            if (filterText != "" || objNum > 0 || deviceNum > 0)
            {
                int.TryParse(filterText, out int cnlNum);
                IEnumerable<SelectableItem> filteredItems = items.Where(item =>
                    (objNum <= 0 || item.ObjNum == objNum) &&
                    (deviceNum <= 0 || item.DeviceNum == deviceNum) &&
                    (!onlySelected || item.Selected) &&
                    (item.CnlNum == cnlNum || StringContains(item.Name, filterText)));
                dataGridView.DataSource = new BindingList<SelectableItem>(filteredItems.ToList());
            }
            else if (onlySelected)
            {
                dataGridView.DataSource = new BindingList<SelectableItem>(selectedItems.Values.ToList());
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


        private void FrmCnlSelect_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            PrepareFilter();
            FillTable();
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is SelectableItem changedItem)
            {
                // update selected channels
                if (changedItem.Selected)
                    selectedItems[changedItem.CnlNum] = changedItem;
                else
                    selectedItems.Remove(changedItem.CnlNum);

                // deselect other channels
                if (!MultiSelect && changedItem.Selected)
                {
                    foreach (SelectableItem item in selectedItems.Values.ToList()) // make copy of values
                    {
                        if (item.CnlNum != changedItem.CnlNum)
                            item.Selected = false;
                    }
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

        private void btnSelect_Click(object sender, EventArgs e)
        {
            // get the selected channel numbers
            SelectedCnlNums = selectedItems.Keys.ToList();
            SelectedCnlNum = SelectedCnlNums.FirstOrDefault();
            DialogResult = DialogResult.OK;
        }
    }
}
