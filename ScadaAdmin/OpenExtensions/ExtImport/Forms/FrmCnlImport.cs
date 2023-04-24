using Scada.Admin.Extensions.ExtImport.Code;
using Scada.Admin.Project;
using Scada.Comm.Devices;
using Scada.Data.Entities;
using Scada.Forms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

using System.Windows.Forms;

namespace Scada.Admin.Extensions.ExtImport.Forms
{
	public partial class FrmCnlImport : Form
	{
		/// <summary>
		/// Represents a form for Import channels.
		/// </summary>
		private readonly IAdminContext adminContext;      // the Administrator context
		private readonly ScadaProject project;            // the project under development
		private readonly RecentSelection recentSelection; // the recently selected objects
		private int step;
		public FrmCnlImport()
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
			ctrlCnlImport3.Visible = false;
			chkPreview.Visible = false;
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
					ctrlCnlImport3.Visible = true;
					chkPreview.Visible = true;
					btnBack.Visible = true;
					btnCreate.Visible = true;

					if (ctrlCnlImport1.StatusOK)
					{
						// ctrlCnlImport3.ResetCnlNums(ctrlCnlImport1.CnlPrototypes.Count);
						btnCreate.Enabled = true;
					}
					else
					{
						btnCreate.Enabled = false;
					}

					// ctrlCnlImport3.DeviceName = ctrlCnlImport1.SelectedDevice?.Name;
					// ctrlCnlImport3.SetFocus();
					break;
			}
		}

		/// <summary>
		/// Creates channels based on the channel prototypes with parameters from file.
		/// </summary>
		//private List<Cnl> CreateChannels()
		//{ 
		//}

		/// <summary>
		/// Adds the specified channels into the configuration database.
		/// </summary>
		private void AddChannels(List<Cnl> cnls, bool silent)
		{
		}

		private void FrmCnlImport_Load(object sender, EventArgs e)
		{
			FormTranslator.Translate(this, GetType().FullName);
			FormTranslator.Translate(ctrlCnlImport1, ctrlCnlImport1.GetType().FullName);
			FormTranslator.Translate(ctrlCnlImport2, ctrlCnlImport2.GetType().FullName);
			FormTranslator.Translate(ctrlCnlImport3, ctrlCnlImport3.GetType().FullName);

			ctrlCnlImport1.Init(adminContext, project, recentSelection);
			ctrlCnlImport2.Init(project, recentSelection);
			ctrlCnlImport3.Init(adminContext, project);
			ApplyStep(0);
		}

		private void ctrlCnlImport1_SelectedDeviceChanged(object sender, EventArgs e)
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
				//List<Cnl> cnls = CreateChannels();

				if (!chkPreview.Checked)
				//||new FrmCnlPreview(cnls).ShowDialog() == DialogResult.OK)
				{
					//AddChannels(cnls, chkPreview.Checked);
					DialogResult = DialogResult.OK;
				}
			}
		}

		private void ctrlCnlImport3_Load(object sender, EventArgs e)
		{

		}
	}
}
