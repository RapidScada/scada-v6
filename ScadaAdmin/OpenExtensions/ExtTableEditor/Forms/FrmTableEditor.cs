// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinControl;

namespace Scada.Admin.Extensions.ExtTableEditor.Forms
{
    /// <summary>
    /// Represents a form for editing a table view.
    /// <para>Представляет форму для редактирования табличного представления.</para>
    /// </summary>
    public partial class FrmTableEditor : Form, IChildForm
    {
        private readonly IAdminContext adminContext; // the Administrator context
        private string fileName;                     // the full name of the edited file


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmTableEditor()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmTableEditor(IAdminContext adminContext, string fileName)
            : this()
        {
            this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
            this.fileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
            Text = Path.GetFileName(fileName);
        }


        /// <summary>
        /// Gets or sets the object associated with the form.
        /// </summary>
        public ChildFormTag ChildFormTag { get; set; }


        /// <summary>
        /// Saves the file.
        /// </summary>
        public void Save()
        {
        }


        private void FrmTableEditor_Load(object sender, EventArgs e)
        {
            ChildFormTag.MessageToChildForm += ChildFormTag_MessageToChildForm;
        }

        private void ChildFormTag_MessageToChildForm(object sender, FormMessageEventArgs e)
        {
            // update file name in case of renaming a file or its parent directory
            if (e.Message == AdminMessage.UpdateFileName &&
                e.GetArgument("FileName") is string newFileName && newFileName != "")
            {
                fileName = newFileName;
                Text = Path.GetFileName(fileName);
            }
        }

        private void btnRefreshBase_Click(object sender, EventArgs e)
        {

        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {

        }

        private void btnAddEmptyItem_Click(object sender, EventArgs e)
        {

        }

        private void btnMoveUpItem_Click(object sender, EventArgs e)
        {

        }

        private void btnMoveDownItem_Click(object sender, EventArgs e)
        {

        }

        private void btnDeleteItem_Click(object sender, EventArgs e)
        {

        }
    }
}
