// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scada.Forms
{
    /// <summary>
    /// Represents a form for selecting entities of the configuration database.
    /// <para>Представляет форму для выбора сущностей базы конфигурации.</para>
    /// </summary>
    public partial class FrmEntitySelect : Form
    {
        private readonly IBaseTable baseTable; // the table containing entities to select


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmEntitySelect()
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
            IDs = null;
        }


        /// <summary>
        /// Gets or sets the IDs of the selected entities.
        /// </summary>
        public ICollection<int> IDs { get; set; }


        private void FrmEntitySelect_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
        }
    }
}
