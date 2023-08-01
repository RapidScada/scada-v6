// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtImport.Code;
using Scada.Admin.Extensions.ExtImport.Controls;
using Scada.Admin.Project;
using Scada.Comm.Devices;
using Scada.Data.Entities;
using Scada.Forms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Scada.Admin.Extensions.ExtImport.Forms
{
	/// <summary>
	/// Represents a form for creating channels.
	/// <para>Представляет форму для создания каналов.</para>
	/// </summary>
	public partial class FrmImport : Form
	{
		private readonly IAdminContext adminContext;      // the Administrator context
		private readonly ScadaProject project;            // the project under development
		private readonly RecentSelection recentSelection; // the recently selected objects
		private int step;                                 // the current step of the wizard


		/// <summary>
		/// Initializes a new instance of the class.
		/// </summary>
		private FrmImport()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Initializes a new instance of the class.
		/// </summary>
		public FrmImport(IAdminContext adminContext, ScadaProject project, RecentSelection recentSelection)
			: this()
		{
			this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
			this.project = project ?? throw new ArgumentNullException(nameof(project));
			this.recentSelection = recentSelection ?? throw new ArgumentNullException(nameof(recentSelection));
			step = 1;
		}


		/// <summary>
		/// Applies the wizard step.
		/// </summary>
		private void ApplyStep(int offset)
		{
			step += offset;

			if (step < 1)
				step = 1;
			else if (step > 3)
				step = 3;

			ctrlImport1.Visible = false;
			ctrlImport2.Visible = false;
			ctrlImport3.Visible = false;
			btnBack.Visible = false;
			btnNext.Visible = false;
			btnCreate.Visible = false;

			switch (step)
			{
				case 1:
					lblStep.Text = ExtensionPhrases.ImportStep1;
					ctrlImport1.Visible = true;
					btnNext.Visible = true;

					ctrlImport1.SetFocus();
					btnNext.Enabled = ctrlImport1.StatusOK;
					break;
				case 2:
					lblStep.Text = ExtensionPhrases.ImportStep2;
					ctrlImport2.Visible = true;
					btnBack.Visible = true;
					btnNext.Visible = true;

					ctrlImport2.DeviceName = ctrlImport1.SelectedDevice?.Name;
					ctrlImport2.SetFocus();
					break;
				case 3:
					lblStep.Text = ExtensionPhrases.ImportStep3;
					ctrlImport3.Visible = true;
					btnBack.Visible = true;
					btnCreate.Visible = true;
					if (ctrlImport1.StatusOK)
					{
						ctrlImport3.ResetCnlNums(ctrlImport1.CnlPrototypes.Count);
						if (ctrlImport3.lastCheckState2)
							btnCreate.Enabled = true;
						else if (ctrlImport3.lastCheckState)
							if (!ctrlImport3.FileSelected)
								btnCreate.Enabled = false;
							else
								btnCreate.Enabled = true;
						else
							btnCreate.Enabled = false;
					}
					else
					{
						btnCreate.Enabled = false;
					}

					ctrlImport3.DeviceName = ctrlImport1.SelectedDevice?.Name;
					ctrlImport3.SetFocus();
					break;
			}
		}

		private void CtrlImport3_SelectedDeviceChanged(object sender, EventArgs e)
		{
			if (ctrlImport3.lastCheckState2)
				btnCreate.Enabled = true;
			else
				if (ctrlImport3.FileSelected)
				btnCreate.Enabled = true;
			else { btnCreate.Enabled = false; }

		}
		private void CtrlCnlCnl3_rdbCheckStateChanged(object sender, EventArgs e)
		{

			if (ctrlImport3.lastCheckState2)
				btnCreate.Enabled = true;
			else
				if (ctrlImport3.FileSelected)
				btnCreate.Enabled = true;
			else { btnCreate.Enabled = false; }

		}

		private void FrmCnlCreate_Load(object sender, EventArgs e)
		{
			FormTranslator.Translate(this, GetType().FullName);
			FormTranslator.Translate(ctrlImport1, ctrlImport1.GetType().FullName);
			FormTranslator.Translate(ctrlImport2, ctrlImport2.GetType().FullName);
			FormTranslator.Translate(ctrlImport3, ctrlImport3.GetType().FullName);

			ctrlImport1.Init(adminContext, project, recentSelection);
			ctrlImport2.Init(project, recentSelection);
			ctrlImport3.Init(adminContext, project);
			ApplyStep(0);
		}

		private void ctrlCnlCreate1_SelectedDeviceChanged(object sender, EventArgs e)
		{
			if (step == 1)
				btnNext.Enabled = ctrlImport1.StatusOK;
		}

		private void btnBack_Click(object sender, EventArgs e)
		{
			ApplyStep(-1);
		}

		private void btnNext_Click(object sender, EventArgs e)
		{
			ApplyStep(1);
		}

		private void btnCreate_Click(object sender, EventArgs e)
		{
			if (ctrlImport1.StatusOK)
			{

				if (ctrlImport3.lastCheckState2)
				{
					if (new FrmCnlsMerge(project, ctrlImport1, ctrlImport2, ctrlImport3).ShowDialog() == DialogResult.OK)
					{
						DialogResult = DialogResult.OK;
					}
				}
				else
				{
					if (ctrlImport3._dictio.Count > 0 && new FrmVariablesMerge(project, ctrlImport1, ctrlImport2, ctrlImport3).ShowDialog() == DialogResult.OK)
					{
						DialogResult = DialogResult.OK;
					}
                    else
                    {
						if (ctrlImport3._dictio.Count == 0)
							ScadaUiUtils.ShowWarning(ExtensionPhrases.FileWarning);
					}
                }

			}

		}
	}
}
