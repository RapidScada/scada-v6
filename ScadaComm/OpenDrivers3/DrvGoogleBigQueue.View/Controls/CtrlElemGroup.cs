// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvGoogleBigQueue.Config;
using Scada.Comm.Drivers.DrvGoogleBigQueue.Protocol;
using Scada.Forms;
using Scada.Lang;
using System.ComponentModel;
using System.Data;

namespace Scada.Comm.Drivers.DrvGoogleBigQueue.View.Controls
{
    /// <summary>
    /// Represents a control for editing an element group.
    /// <para>Представляет элемент управления для редактирования группы элементов.</para>
    /// </summary>
    public partial class CtrlElemGroup : UserControl
    {
        private ElemGroupConfig elemGroup; // the element group configuration


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlElemGroup()
        {
            InitializeComponent();
            elemGroup = null;
            TemplateOptions = null;
        }

        /// <summary>
        /// Gets or sets a reference to the device template options.
        /// </summary>
        public DeviceTemplateOptions TemplateOptions { get; set; }

        /// <summary>
        /// Gets or sets the element group for editing.
        /// </summary>
        public ElemGroupConfig ElemGroup
        {
            get
            {
                return elemGroup;
            }
            set
            {
                elemGroup = null; // to avoid ObjectChanged event
                ShowElemGroupConfig(value);
                elemGroup = value;
            }
        }


        /// <summary>
        /// Shows the element group configuration.
        /// </summary>
        private void ShowElemGroupConfig(ElemGroupConfig elemGroup)
        {
            if (elemGroup == null)
            {
                chkGrActive.Checked = false;
                txtGrName.Text = "";
                txtGrCode.Text = "";
                txtQuerySql.Text = "";
                txtProjectId.Text = "";
                numGrElemCnt.Value = 1;
                gbElemGroup.Enabled = false;
            }
            else
            {
                chkGrActive.Checked = elemGroup.Active;
                txtGrName.Text = elemGroup.Name;
                txtGrCode.Text = elemGroup.Code;
                txtProjectId.Text = elemGroup.ProjectId;
                txtQuerySql.Text = elemGroup.QuerySql;
                numGrElemCnt.Maximum = int.MaxValue;
                numGrElemCnt.SetValue(elemGroup.Elems.Count);
                gbElemGroup.Enabled = true;
            }
        }


        /// <summary>
        /// Raises an ObjectChanged event.
        /// </summary>
        private void OnObjectChanged(object changeArgument)
        {
            ObjectChanged?.Invoke(this, new ObjectChangedEventArgs(elemGroup, changeArgument));
        }

        /// <summary>
        /// Sets input focus to the control.
        /// </summary>
        public void SetFocus()
        {
            txtGrName.Select();
        }


        /// <summary>
        /// Occurs when the edited object changes.
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler<ObjectChangedEventArgs> ObjectChanged;


        private void chkGrActive_CheckedChanged(object sender, EventArgs e)
        {
            // update group activity
            if (elemGroup != null)
            {
                elemGroup.Active = chkGrActive.Checked;
                OnObjectChanged(TreeUpdateTypes.CurrentNode);
            }
        }

        private void txtGrName_TextChanged(object sender, EventArgs e)
        {
            // update group name
            if (elemGroup != null)
            {
                elemGroup.Name = txtGrName.Text;
                OnObjectChanged(TreeUpdateTypes.CurrentNode);
            }
        }

        private void txtGrCode_TextChanged(object sender, EventArgs e)
        {
            // update group name
            if (elemGroup != null)
            {
                elemGroup.Code = txtGrCode.Text;
                OnObjectChanged(TreeUpdateTypes.CurrentNode);
            }
        }

        private void numGrElemCnt_ValueChanged(object sender, EventArgs e)
        {
            // update number of elements
            if (elemGroup != null)
            {
                int oldElemCnt = elemGroup.Elems.Count;
                int newElemCnt = (int)numGrElemCnt.Value;

                if (oldElemCnt < newElemCnt)
                {
                    // add new elements
                    for (int elemInd = oldElemCnt; elemInd < newElemCnt; elemInd++)
                    {
                        ElemConfig elem = elemGroup.CreateElemConfig();
                        elemGroup.Elems.Add(elem);
                    }
                }
                else if (oldElemCnt > newElemCnt)
                {
                    // remove redundant elements
                    for (int i = newElemCnt; i < oldElemCnt; i++)
                    {
                        elemGroup.Elems.RemoveAt(newElemCnt);
                    }
                }

                OnObjectChanged(TreeUpdateTypes.ChildCount);
            }
        }

        private void txtProjectId_TextChanged(object sender, EventArgs e)
        {
            if (elemGroup != null)
            {
                elemGroup.ProjectId = txtProjectId.Text;
                OnObjectChanged(TreeUpdateTypes.CurrentNode);
            }
        }

        private void txtQuerySql_TextChanged(object sender, EventArgs e)
        {
            if (elemGroup != null)
            {
                elemGroup.QuerySql = txtQuerySql.Text;
                OnObjectChanged(TreeUpdateTypes.CurrentNode);
            }
        }

        private void btnViewParameters_Click(object sender, EventArgs e)
        {
            new FrmQueryParametrs { }.ShowDialog();
        }
    }
}
