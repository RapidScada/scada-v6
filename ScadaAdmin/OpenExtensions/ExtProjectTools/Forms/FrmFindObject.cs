// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Forms;

namespace Scada.Admin.Extensions.ExtProjectTools.Forms
{
    /// <summary>
    /// Represents a form for finding objects.
    /// <para>Представляет форму для поиска объектов.</para>
    /// </summary>
    public partial class FrmFindObject : Form
    {
        private readonly TreeView treeView;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmFindObject()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmFindObject(TreeView treeView)
            : this()
        {
            this.treeView = treeView ?? throw new ArgumentNullException(nameof(treeView));
            FormTranslator.Translate(this, GetType().FullName);
        }


        private void FrmFindObject_Load(object sender, EventArgs e)
        {

        }

        private void txtFind_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnFindNext_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
