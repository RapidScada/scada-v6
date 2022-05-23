/*
 * Copyright 2022 Rapid Software LLC
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * 
 * Product  : Rapid SCADA
 * Module   : Administrator
 * Summary  : Represents a form for creating a limit
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2022
 * Modified : 2022
 */

using Scada.Admin.App.Code;
using Scada.Admin.Project;
using Scada.Data.Entities;
using Scada.Forms;
using Scada.Lang;
using System;
using System.Text;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms.Tables
{
    /// <summary>
    /// Represents a form for creating a limit.
    /// <para>Представляет форму для создания границы.</para>
    /// </summary>
    public partial class FrmLimCreate : Form
    {
        private readonly ConfigDatabase configDatabase;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmLimCreate()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmLimCreate(ConfigDatabase configDatabase)
            : this()
        {
            this.configDatabase = configDatabase ?? throw new ArgumentNullException(nameof(configDatabase));

            CnlNum = 0;
            LimEntity = null;

            numLimID.Value = configDatabase.LimTable.GetNextPk();
            numLimID.Maximum = ConfigDatabase.MaxID;
        }


        /// <summary>
        /// Gets or sets the channel number for which a limit is created.
        /// </summary>
        public int CnlNum { get; set; }

        /// <summary>
        /// Gets the limit.
        /// </summary>
        public Lim LimEntity { get; private set; }


        /// <summary>
        /// Validates the form controls.
        /// </summary>
        private bool ValidateControls()
        {
            StringBuilder sbError = new();

            if (string.IsNullOrWhiteSpace(txtName.Text))
                sbError.AppendError(lblName, CommonPhrases.NonemptyRequired);

            if (txtLoLo.Text != "" && !ScadaUtils.TryParseDouble(txtLoLo.Text, out _))
                sbError.AppendError(lblLoLo, CommonPhrases.RealRequired);

            if (txtLow.Text != "" && !ScadaUtils.TryParseDouble(txtLow.Text, out _))
                sbError.AppendError(lblLow, CommonPhrases.RealRequired);

            if (txtHigh.Text != "" && !ScadaUtils.TryParseDouble(txtHigh.Text, out _))
                sbError.AppendError(lblHigh, CommonPhrases.RealRequired);

            if (txtHiHi.Text != "" && !ScadaUtils.TryParseDouble(txtHiHi.Text, out _))
                sbError.AppendError(lblHiHi, CommonPhrases.RealRequired);

            if (txtDeadband.Text != "" && !ScadaUtils.TryParseDouble(txtDeadband.Text, out _))
                sbError.AppendError(lblDeadband, CommonPhrases.RealRequired);

            if (sbError.Length > 0)
            {
                ScadaUiUtils.ShowError(CommonPhrases.CorrectErrors + Environment.NewLine + sbError);
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Checks feasibility of adding a device.
        /// </summary>
        private bool CheckFeasibility()
        {
            if (configDatabase.LimTable.PkExists(Convert.ToInt32(numLimID.Value)))
            {
                ScadaUiUtils.ShowError(AppPhrases.LimExistsInConfigDatabase);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Sets the entity properties according to the controls.
        /// </summary>
        private void ControlsToEntity()
        {
            LimEntity = new Lim
            {
                LimID = Convert.ToInt32(numLimID.Value),
                Name = txtName.Text,
                IsBoundToCnl = chkIsBoundToCnl.Checked,
                IsShared = chkIsShared.Checked,
                LoLo = txtLoLo.Text == "" ? null : ScadaUtils.ParseDouble(txtLoLo.Text),
                Low = txtLow.Text == "" ? null : ScadaUtils.ParseDouble(txtLow.Text),
                High = txtHigh.Text == "" ? null : ScadaUtils.ParseDouble(txtHigh.Text),
                HiHi = txtHiHi.Text == "" ? null : ScadaUtils.ParseDouble(txtHiHi.Text),
                Deadband = txtDeadband.Text == "" ? null : ScadaUtils.ParseDouble(txtDeadband.Text)
            };
        }


        private void FrmLimCreate_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);

            if (CnlNum > 0)
                txtName.Text = string.Format(AppPhrases.DefaultLimName, CnlNum);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidateControls() && CheckFeasibility())
            {
                ControlsToEntity();
                DialogResult = DialogResult.OK;
            }
        }
    }
}
