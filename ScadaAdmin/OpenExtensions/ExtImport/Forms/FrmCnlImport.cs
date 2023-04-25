using Scada.Admin.Config;
using Scada.Admin.Extensions.ExtImport.Code;
using Scada.Admin.Extensions.ExtImport.Controls;
using Scada.Admin.Project;
using Scada.Comm.Devices;
using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Forms;
using Scada.Lang;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Scada.Admin.Extensions.ExtImport.Forms
{
	public partial class FrmCnlImport : Form
	{
		private readonly IAdminContext adminContext; // the Administrator context
		private readonly ScadaProject project;       // the project under development
		private readonly FrmCnlCreateLogic formLogic;


		public FrmCnlImport(IAdminContext adminContext, ScadaProject project)
		{
			InitializeComponent();
			this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
			this.project = project ?? throw new ArgumentNullException(nameof(project));
			formLogic = new FrmCnlCreateLogic(project.ConfigDatabase);
			ctrlCnlImport1.Init(adminContext, project);
			ctrlCnlImport2.Init(adminContext, project);
			ctrlCnlImport3.Init(adminContext, project);
		}


		private void btnCreate_Click(object sender, EventArgs e)
		{
			// validate the channels
			List<int> errors = new List<int>();

			if (!ctrlCnlImport1.ValidateChannels(errors) ||
				!ctrlCnlImport2.ValidateChannels(errors) ||
				!ctrlCnlImport3.ValidateChannels(errors))
			{
				ScadaUiUtils.ShowError(Language.GetItem("Common", "errFillForm"));
				return;
			}

			// create the channels
			if (formLogic.CreateChannels(adminContext, ctrlCnlCreate1.Channels, ctrlCnlCreate2.Channels, ctrlCnlCreate3.Channels))
			{
				DialogResult = DialogResult.OK;
			}
			else
			{
				ScadaUiUtils.ShowError(formLogic.ErrorMessage);
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}

		private void ctrlCnlImport1_Load(object sender, EventArgs e)
		{

		}
	}
}
