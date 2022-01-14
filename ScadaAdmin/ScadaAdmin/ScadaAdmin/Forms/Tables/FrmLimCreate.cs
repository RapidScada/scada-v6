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

using Scada.Admin.Project;
using Scada.Data.Entities;
using Scada.Forms;
using System;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms.Tables
{
    /// <summary>
    /// Represents a form for creating a limit.
    /// <para>Представляет форму для создания границы.</para>
    /// </summary>
    public partial class FrmLimCreate : Form
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmLimCreate()
        {
            InitializeComponent();

            numLimID.Maximum = ConfigBase.MaxID;
            LimEntity = new Lim();
        }


        /// <summary>
        /// Gets the limit.
        /// </summary>
        public Lim LimEntity { get; }


        private void FrmLimCreate_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
