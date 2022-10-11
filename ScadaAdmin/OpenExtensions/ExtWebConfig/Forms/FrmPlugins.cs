// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Project;
using Scada.Forms;
using Scada.Web.Config;
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

namespace Scada.Admin.Extensions.ExtWebConfig.Forms
{
    /// <summary>
    /// Represents a form for editing a web config.
    /// <para>Представляет форму для редактирования настроек web-интерфейса.</para>
    /// </summary>
    public partial class FrmPlugins : Form, IChildForm
    {
        private readonly IAdminContext adminContext; // the Administrator context
        private readonly WebApp webApp;              // the web application in a project


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmPlugins()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmPlugins(IAdminContext adminContext, WebApp webApp)
            : this()
        {
            this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
            this.webApp = webApp ?? throw new ArgumentNullException(nameof(webApp));           
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
    }
}
