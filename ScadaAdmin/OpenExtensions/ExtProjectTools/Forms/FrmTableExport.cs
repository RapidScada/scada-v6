// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using CsvHelper;
using Scada.Admin.Extensions.ExtProjectTools.Code;
using Scada.Admin.Project;
using Scada.Data.Adapters;
using Scada.Data.Tables;
using Scada.Forms;
using Scada.Lang;
using Scada.Log;
using System;
using System.IO;
using System.Windows.Forms;

namespace Scada.Admin.Extensions.ExtProjectTools.Forms
{
    /// <summary>
    /// Represents a form for exporting a configuration database table.
    /// <para>Представляет форму экспорта таблицы базы конфигурации.</para>
    /// </summary>
    public partial class FrmTableExport : Form
    {
        private readonly ILog log;                      // the application log
        private readonly ConfigDatabase configDatabase; // the configuration database


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmTableExport()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmTableExport(ILog log, ConfigDatabase configDatabase)
            : this()
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            this.configDatabase = configDatabase ?? throw new ArgumentNullException(nameof(configDatabase));
            SelectedItemType = null;
        }


        /// <summary>
        /// Gets the start ID.
        /// </summary>
        private int StartID
        {
            get
            {
                return chkStartID.Checked ? Convert.ToInt32(numStartID.Value) : 0;
            }
        }

        /// <summary>
        /// Gets the end ID.
        /// </summary>
        private int EndID
        {
            get
            {
                return chkEndID.Checked ? Convert.ToInt32(numEndID.Value) : ConfigDatabase.MaxID;
            }
        }

        /// <summary>
        /// Gets or sets the item type of the selected table.
        /// </summary>
        public Type SelectedItemType { get; set; }


        /// <summary>
        /// Gets the output file name and the selected table format.
        /// </summary>
        private string GetOutputFileName(IBaseTable baseTable, out BaseTableFormat format)
        {
            switch (cbFormat.SelectedIndex)
            {
                case 0:
                    format = BaseTableFormat.DAT;
                    return baseTable.Name.ToLowerInvariant() + ".dat";
                case 1:
                    format = BaseTableFormat.XML;
                    return baseTable.Name + ".xml";
                default:
                    format = BaseTableFormat.CSV;
                    return baseTable.Name + ".csv";
            }
        }

        /// <summary>
        /// Exports the table.
        /// </summary>
        private bool ExportTable(string fileName, IBaseTable baseTable, BaseTableFormat format, int startID, int endID)
        {
            try
            {
                // filter table
                IBaseTable filteredTable;

                if (0 < startID || endID < ConfigDatabase.MaxID)
                {
                    filteredTable = BaseTableFactory.GetBaseTable(baseTable);

                    if (startID <= endID)
                    {
                        foreach (object item in baseTable.EnumerateItems())
                        {
                            int itemID = baseTable.GetPkValue(item);

                            if (startID <= itemID && itemID <= endID)
                                filteredTable.AddObject(item);
                            else if (itemID > endID)
                                break;
                        }
                    }
                }
                else
                {
                    filteredTable = baseTable;
                }

                // save table
                switch (format)
                {
                    case BaseTableFormat.DAT:
                        new BaseTableAdapter { FileName = fileName }.Update(filteredTable);
                        break;

                    case BaseTableFormat.XML:
                        filteredTable.Save(fileName);
                        break;

                    case BaseTableFormat.CSV:
                        using (StreamWriter writer = new(fileName))
                        {
                            using CsvWriter csvWriter = new(writer, Locale.Culture);
                            csvWriter.WriteRecords(filteredTable.EnumerateItems());
                        }
                        break;
                }

                return true;
            }
            catch (Exception ex)
            {
                log.HandleError(ex, ExtensionPhrases.ExportTableError);
                return false;
            }
        }


        private void FrmTableExport_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            saveFileDialog.SetFilter(ExtensionPhrases.ExportTableFilter);
            ExtensionUtils.FillTableList(cbTable, configDatabase, SelectedItemType);
            cbFormat.SelectedIndex = 0;
        }

        private void chkStartID_CheckedChanged(object sender, EventArgs e)
        {
            numStartID.Enabled = chkStartID.Checked;
        }

        private void chkEndID_CheckedChanged(object sender, EventArgs e)
        {
            numEndID.Enabled = chkEndID.Checked;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (cbTable.SelectedItem is BaseTableItem item)
            {
                SelectedItemType = item.BaseTable.ItemType;
                saveFileDialog.FileName = GetOutputFileName(item.BaseTable, out BaseTableFormat format);

                if (saveFileDialog.ShowDialog() == DialogResult.OK &&
                    ExportTable(saveFileDialog.FileName, item.BaseTable, format, StartID, EndID))
                {
                    DialogResult = DialogResult.OK;
                }
            }
        }
    }
}
