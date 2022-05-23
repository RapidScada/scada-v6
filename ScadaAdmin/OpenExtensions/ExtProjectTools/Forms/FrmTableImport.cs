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
    /// Represents a form for importing a configuration database table.
    /// <para>Представляет форму импорта таблицы базы конфигурации.</para>
    /// </summary>
    public partial class FrmTableImport : Form
    {
        private readonly ILog log;                      // the application log
        private readonly ConfigDatabase configDatabase; // the configuration database


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmTableImport()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmTableImport(ILog log, ConfigDatabase configDatabase)
            : this()
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            this.configDatabase = configDatabase ?? throw new ArgumentNullException(nameof(configDatabase));
            SelectedItemType = null;
        }


        /// <summary>
        /// Gets the source start ID.
        /// </summary>
        private int SrcStartID
        {
            get
            {
                return chkSrcStartID.Checked ? Convert.ToInt32(numSrcStartID.Value) : 0;
            }
        }

        /// <summary>
        /// Gets the source end ID.
        /// </summary>
        private int SrcEndID
        {
            get
            {
                return chkSrcEndID.Checked ? Convert.ToInt32(numSrcEndID.Value) : ConfigDatabase.MaxID;
            }
        }

        /// <summary>
        /// Gets the destination start ID.
        /// </summary>
        private int DestStartID
        {
            get
            {
                return chkDestStartID.Checked ? Convert.ToInt32(numDestStartID.Value) : SrcStartID;
            }
        }

        /// <summary>
        /// Gets or sets the item type of the selected table.
        /// </summary>
        public Type SelectedItemType { get; set; }


        /// <summary>
        /// Calculates the end destination ID.
        /// </summary>
        private void CalcDestEndID()
        {
            numDestEndID.SetValue(chkSrcEndID.Checked 
                ? chkDestStartID.Checked 
                    ? numSrcEndID.Value - numSrcStartID.Value + numDestStartID.Value 
                    : numSrcEndID.Value 
                : 0);
        }

        /// <summary>
        /// Imports the table.
        /// </summary>
        private bool ImportTable(string fileName, IBaseTable baseTable, BaseTableFormat format,
            int srcStartID, int srcEndID, int destStartID)
        {
            if (!File.Exists(fileName))
            {
                ScadaUiUtils.ShowError(CommonPhrases.FileNotFound);
                return false;
            }

            try
            {
                // open source table
                IBaseTable srcTable = BaseTableFactory.GetBaseTable(baseTable);

                switch (format)
                {
                    case BaseTableFormat.DAT:
                        new BaseTableAdapter { FileName = fileName }.Fill(srcTable);
                        break;

                    case BaseTableFormat.XML:
                        srcTable.Load(fileName);
                        break;

                    case BaseTableFormat.CSV:
                        using (StreamReader reader = new(fileName))
                        {
                            using CsvReader csvReader = new(reader, Locale.Culture);
                            foreach (object record in csvReader.GetRecords(srcTable.ItemType))
                            {
                                srcTable.AddObject(record);
                            }
                        }
                        break;
                }

                // copy data from source table to destination
                ExtensionUtils.NormalizeIdRange(0, ConfigDatabase.MaxID,
                    ref srcStartID, ref srcEndID, destStartID, out int idOffset);
                int affectedRows = 0;

                foreach (object item in srcTable.EnumerateItems())
                {
                    int itemID = srcTable.GetPkValue(item);

                    if (itemID < srcStartID)
                    {
                        continue;
                    }
                    else if (itemID > srcEndID)
                    {
                        break;
                    }
                    else
                    {
                        if (idOffset != 0)
                            srcTable.SetPkValue(item, itemID + idOffset);

                        baseTable.AddObject(item);
                        affectedRows++;
                    }
                }

                if (affectedRows > 0)
                    baseTable.Modified = true;

                ScadaUiUtils.ShowInfo(string.Format(ExtensionPhrases.ImportTableCompleted, affectedRows));
                return true;
            }
            catch (Exception ex)
            {
                log.HandleError(ex, ExtensionPhrases.ImportTableError);
                return false;
            }
        }


        private void FrmTableImport_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            openFileDialog.SetFilter(ExtensionPhrases.ImportTableFilter);
            ExtensionUtils.FillTableList(cbTable, configDatabase, SelectedItemType);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (File.Exists(txtSrcFile.Text))
            {
                openFileDialog.InitialDirectory = Path.GetDirectoryName(txtSrcFile.Text);
                openFileDialog.FileName = Path.GetFileName(txtSrcFile.Text);
            }

            if (openFileDialog.ShowDialog() == DialogResult.OK)
                txtSrcFile.Text = openFileDialog.FileName;
        }

        private void chkSrcStartID_CheckedChanged(object sender, EventArgs e)
        {
            numSrcStartID.Enabled = chkSrcStartID.Checked;
            CalcDestEndID();
        }

        private void chkSrcEndID_CheckedChanged(object sender, EventArgs e)
        {
            numSrcEndID.Enabled = chkSrcEndID.Checked;
            CalcDestEndID();
        }

        private void chkDestStartID_CheckedChanged(object sender, EventArgs e)
        {
            numDestStartID.Enabled = chkDestStartID.Checked;
            CalcDestEndID();
        }

        private void num_ValueChanged(object sender, EventArgs e)
        {
            CalcDestEndID();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (cbTable.SelectedItem is BaseTableItem item)
            {
                SelectedItemType = item.BaseTable.ItemType;
                BaseTableFormat format = Path.GetExtension(txtSrcFile.Text).ToLowerInvariant() switch
                {
                    ".xml" => BaseTableFormat.XML,
                    ".csv" => BaseTableFormat.CSV,
                    _ => BaseTableFormat.DAT
                };

                if (ImportTable(txtSrcFile.Text, item.BaseTable, format, SrcStartID, SrcEndID, DestStartID))
                    DialogResult = DialogResult.OK;
            }
        }
    }
}
