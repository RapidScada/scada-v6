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
	public partial class FrmCnlImport : Form
	{
		private readonly IAdminContext adminContext;      // the Administrator context
		private readonly ScadaProject project;            // the project under development
		private readonly RecentSelection recentSelection; // the recently selected objects
		private int step;                                 // the current step of the wizard


		/// <summary>
		/// Initializes a new instance of the class.
		/// </summary>
		private FrmCnlImport()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Initializes a new instance of the class.
		/// </summary>
		public FrmCnlImport(IAdminContext adminContext, ScadaProject project, RecentSelection recentSelection)
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

			ctrlCnlImport1.Visible = false;
			ctrlCnlImport2.Visible = false;
			ctrlCnlmport3.Visible = false;
			btnBack.Visible = false;
			btnNext.Visible = false;
			btnCreate.Visible = false;

			switch (step)
			{
				case 1:
					lblStep.Text = ExtensionPhrases.CreateCnlsStep1;
					ctrlCnlImport1.Visible = true;
					btnNext.Visible = true;

					ctrlCnlImport1.SetFocus();
					btnNext.Enabled = ctrlCnlImport1.StatusOK;
					break;
				case 2:
					lblStep.Text = ExtensionPhrases.CreateCnlsStep2;
					ctrlCnlImport2.Visible = true;
					btnBack.Visible = true;
					btnNext.Visible = true;

					ctrlCnlImport2.DeviceName = ctrlCnlImport1.SelectedDevice?.Name;
					ctrlCnlImport2.SetFocus();
					break;
				case 3:
					lblStep.Text = ExtensionPhrases.CreateCnlsStep3;
					ctrlCnlmport3.Visible = true;
					btnBack.Visible = true;
					btnCreate.Visible = true;
					if (ctrlCnlImport1.StatusOK)
					{
						ctrlCnlmport3.ResetCnlNums(ctrlCnlImport1.CnlPrototypes.Count);
						if (ctrlCnlmport3.lastCheckState2)
							btnCreate.Enabled = true;
						else if (ctrlCnlmport3.lastCheckState)
							if (!ctrlCnlmport3.FileSelected)
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

					ctrlCnlmport3.DeviceName = ctrlCnlImport1.SelectedDevice?.Name;
					ctrlCnlmport3.SetFocus();
					break;
			}
		}

		private void CtrlCnlImport3_SelectedDeviceChanged(object sender, EventArgs e)
		{
			if (ctrlCnlmport3.lastCheckState2)
				btnCreate.Enabled = true;
			else
				if (ctrlCnlmport3.FileSelected)
				btnCreate.Enabled = true;
			else { btnCreate.Enabled = false; }

		}
		private void CtrlCnlCnl3_rdbCheckStateChanged(object sender, EventArgs e)
		{

			if (ctrlCnlmport3.lastCheckState2)
				btnCreate.Enabled = true;
			else
				if (ctrlCnlmport3.FileSelected)
				btnCreate.Enabled = true;
			else { btnCreate.Enabled = false; }

		}

		private void FrmCnlCreate_Load(object sender, EventArgs e)
		{
			FormTranslator.Translate(this, GetType().FullName);
			FormTranslator.Translate(ctrlCnlImport1, ctrlCnlImport1.GetType().FullName);
			FormTranslator.Translate(ctrlCnlImport2, ctrlCnlImport2.GetType().FullName);
			FormTranslator.Translate(ctrlCnlmport3, ctrlCnlmport3.GetType().FullName);

			ctrlCnlImport1.Init(adminContext, project, recentSelection);
			ctrlCnlImport2.Init(project, recentSelection);
			ctrlCnlmport3.Init(adminContext, project);
			ApplyStep(0);
		}

		private void ctrlCnlCreate1_SelectedDeviceChanged(object sender, EventArgs e)
		{
			if (step == 1)
				btnNext.Enabled = ctrlCnlImport1.StatusOK;
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
			if (ctrlCnlImport1.StatusOK)
			{

				if (ctrlCnlmport3.lastCheckState2)
				{
					if (new FrmCnlsMerge(project, ctrlCnlImport1, ctrlCnlImport2, ctrlCnlmport3).ShowDialog() == DialogResult.OK)
					{
						DialogResult = DialogResult.OK;
					}
				}
				else
				{
					if (ctrlCnlmport3._dictio.Count > 0 && new FrmVariableMerge(project, ctrlCnlImport1, ctrlCnlImport2, ctrlCnlmport3).ShowDialog() == DialogResult.OK)
					{
						DialogResult = DialogResult.OK;
					}
                    else
                    {
						ScadaUiUtils.ShowWarning("Le fichier selectioné est probablement vide");
					}
                }

			}

		}
	}
}
