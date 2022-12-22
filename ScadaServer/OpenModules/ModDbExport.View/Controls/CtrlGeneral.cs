// Copyright (c) Rapid Software LLC. All rights reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Scada.Data.Models;
using Scada.Forms;
using Scada.Server.Modules.ModDbExport.Config;

namespace Scada.Server.Modules.ModDbExport.View.Controls
{
    /// <summary>
    /// Represents a control for editing a general options.
    /// <para>Представляет элемент управления для редактирования общих настроек.</para>
    /// </summary>
    public partial class CtrlGeneral : UserControl
    {
        private GeneralOptions generalOptions;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlGeneral()
        {
            InitializeComponent();

            generalOptions = null;
            ConfigDataset = null;
        }


        /// <summary>
        /// Gets or sets the general options for editing.
        /// </summary>
        internal GeneralOptions GeneralOptions
        {
            get
            {
                return generalOptions;
            }
            set
            {
                generalOptions = null;
                ShowOptions(value);
                generalOptions = value;
            }
        }

        /// <summary>
        /// Gets or sets the configuration database.
        /// </summary>
        public ConfigDataset ConfigDataset { get; set; }


        /// <summary>
        /// Shows the options.
        /// </summary>
        private void ShowOptions(GeneralOptions options)
        {
            if (options == null)
            {
                chkActive.Checked = false;
                txtTargetID.Text = "";
                txtName.Text = "";
                txtCmdCode.Text = "";
                numStatusCnlNum.Value = numStatusCnlNum.Minimum;
                numMaxQueueSize.Value = numMaxQueueSize.Minimum;                
                numDataLifetime.Value = numDataLifetime.Minimum;
            }
            else
            {
                chkActive.Checked = options.Active;
                txtTargetID.Text = options.ID.ToString();
                txtName.Text = options.Name;
                txtCmdCode.Text = options.CmdCode;
                numStatusCnlNum.Value = options.StatusCnlNum;
                numMaxQueueSize.Value = options.MaxQueueSize;
                numDataLifetime.Value = options.DataLifetime;
            }
        }

        /// <summary>
        /// Raises an ObjectChanged event.
        /// </summary>
        private void OnObjectChanged(object changeArgument)
        {
            ObjectChanged?.Invoke(this, new ObjectChangedEventArgs(generalOptions, changeArgument));
        }

        /// <summary>
        /// Sets input focus to the control.
        /// </summary>
        public void SetFocus()
        {
            txtName.Select();
        }


        /// <summary>
        /// Occurs when the edited object changes.
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler<ObjectChangedEventArgs> ObjectChanged;
    }
}
