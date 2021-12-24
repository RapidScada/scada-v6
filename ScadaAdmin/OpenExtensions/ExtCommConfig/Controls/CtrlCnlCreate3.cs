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

namespace Scada.Admin.Extensions.ExtCommConfig.Controls
{
    /// <summary>
    /// Represents a control for selecting channel numbers when creating channels.
    /// <para>Представляет элемент управления для выбора номеров каналов при создании каналов.</para>
    /// </summary>
    public partial class CtrlCnlCreate3 : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlCnlCreate3()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Gets or sets the selected device name.
        /// </summary>
        public string DeviceName
        {
            get
            {
                return txtDevice.Text;
            }
            set
            {
                txtDevice.Text = value ?? "";
            }
        }


        /// <summary>
        /// Initializes the control.
        /// </summary>
        public void Init()
        {
        }

        /// <summary>
        /// Sets the input focus.
        /// </summary>
        public void SetFocus()
        {
            numStartCnlNum.Select();
        }
        
        /// <summary>
        /// Sets the channel numbers by default.
        /// </summary>
        public void ResetCnlNums(int cnlCnt)
        {

        }


        private void numStartCnlNum_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnMap_Click(object sender, EventArgs e)
        {

        }

        private void btnReset_Click(object sender, EventArgs e)
        {

        }
    }
}
