using Scada.Admin.Config;
using Scada.Admin.Extensions.ExtImport.Code;
using Scada.Admin.Extensions.ExtImport.Controls;
using Scada.Admin.Project;
using Scada.Comm.Devices;
using Scada.Data.Entities;
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
		private readonly IAdminContext adminContext;
		private readonly ScadaProject project;
		private readonly RecentSelection recentSelection;
		private int step;

		private FrmCnlImport()
		{
			InitializeComponent();
		}

		public FrmCnlImport(IAdminContext adminContext, ScadaProject project, RecentSelection recentSelection)
			: this()
		{
			this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
			this.project = project ?? throw new ArgumentNullException(nameof(project));
			this.recentSelection = recentSelection ?? throw new ArgumentNullException(nameof(recentSelection));
			step = 1;
		}

		
		private void ApplyStep(int offset)
		{
			step += offset;

			if (step < 1)
				step = 1;
			else if (step > 3)
				step = 3;

			CtrlCnlImport1.Visible = false;
			CtrlCnlImport2.Visible = false;
			CtrlCnlImport3.Visible = false;
			btnBack.Visible = false;
			btnNext.Visible = false;
			btnCreate.Visible = false;

			switch (step)
			{
				case 1:
					lblStep.Text = ExtensionPhrases.CreateCnlsStep1;
					CtrlCnlImport1.Visible = true;
					btnNext.Visible = true;

					CtrlCnlImport1.SetFocus();
					btnNext.Enabled = CtrlCnlImport1.StatusOK;
					break;
				case 2:
					lblStep.Text = ExtensionPhrases.CreateCnlsStep2;
					CtrlCnlImport2.Visible = true;
					btnBack.Visible = true;
					btnNext.Visible = true;

					CtrlCnlImport2.DeviceName = CtrlCnlImport1.SelectedDevice?.Name;
					CtrlCnlImport2.SetFocus();
					
					break;
				case 3:
					lblStep.Text = ExtensionPhrases.CreateCnlsStep3;
					CtrlCnlImport3.Visible = true;
					btnCreate.Visible = true;
					//btnCreate.Enabled = CtrlCnlImport3.FileSelected;
					btnBack.Visible = true;
					
					// ctrlCnlImport3.DeviceName = ctrlCnlImport1.SelectedDevice?.Name;
					// ctrlCnlImport3.SetFocus();
					break;
			}
		}
		private List<Cnl> CreateChannels()
		{
			List<Cnl> cnls = new();
			//int cnlNum = CtrlCnlImport3.StartCnlNum;
			string namePrefix = adminContext.AppConfig.ChannelNumberingOptions.PrependDeviceName ?
				CtrlCnlImport1.SelectedDevice.Name + " - " : "";
			int? objNum = CtrlCnlImport2.ObjNum;
			int deviceNum = CtrlCnlImport1.SelectedDevice.DeviceNum;

			foreach (CnlPrototype cnlPrototype in CtrlCnlImport1.CnlPrototypes)
			{
				cnls.Add(new Cnl
				{
					//CnlNum = cnlNum,
					Active = cnlPrototype.Active,
					Name = namePrefix + cnlPrototype.Name,
					DataTypeID = cnlPrototype.DataTypeID,
					DataLen = cnlPrototype.DataLen,
					CnlTypeID = cnlPrototype.CnlTypeID,
					ObjNum = objNum,
					DeviceNum = deviceNum,
					TagNum = cnlPrototype.TagNum,
					TagCode = cnlPrototype.TagCode,
					FormulaEnabled = cnlPrototype.FormulaEnabled,
					InFormula = cnlPrototype.InFormula,
					OutFormula = cnlPrototype.OutFormula,
					FormatID = project.ConfigDatabase.GetFormatByCode(cnlPrototype.FormatCode)?.FormatID,
					QuantityID = project.ConfigDatabase.GetQuantityByCode(cnlPrototype.QuantityCode)?.QuantityID,
					UnitID = project.ConfigDatabase.GetUnitByCode(cnlPrototype.UnitCode)?.UnitID,
					LimID = null,
					ArchiveMask = cnlPrototype.ArchiveMask,
					EventMask = cnlPrototype.EventMask
				});

				int dataLength = cnlPrototype.GetDataLength();
				//if (cnlNum > ConfigDatabase.MaxID - dataLength)
				//	break;
				//cnlNum += dataLength;
			}

			return cnls;
		}

		//private void AddChannels(List<Cnl> cnls, bool silent)
		private void AddChannels(List<Cnl> cnls)
		{
			if (cnls.Count > 0)
			{
				cnls.ForEach(cnl => project.ConfigDatabase.CnlTable.AddItem(cnl));
				project.ConfigDatabase.CnlTable.Modified = true;
			}

			//if (!silent)
			//	ScadaUiUtils.ShowInfo(ExtensionPhrases.CreateCnlsCompleted, cnls.Count);
		}

		private void FrmCnlCreate_Load(object sender, EventArgs e)
		{
			FormTranslator.Translate(this, GetType().FullName);
			FormTranslator.Translate(CtrlCnlImport1, CtrlCnlImport1.GetType().FullName);
			FormTranslator.Translate(CtrlCnlImport2, CtrlCnlImport2.GetType().FullName);
			FormTranslator.Translate(CtrlCnlImport3, CtrlCnlImport3.GetType().FullName);
		
			CtrlCnlImport1.Init(adminContext, project, recentSelection);
			CtrlCnlImport2.Init(project, recentSelection);
			CtrlCnlImport3.Init(adminContext, project);
			
			ApplyStep(0);
		}

		private void ctrlCnlCreate1_SelectedDeviceChanged(object sender, EventArgs e)
		{
			if (step == 1)
				btnNext.Enabled = CtrlCnlImport1.StatusOK;
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
			if (CtrlCnlImport1.StatusOK)
			{
				List<Cnl> cnls = CreateChannels();

				if (new FrmCnlMerge(cnls, CtrlCnlImport3._dictio).ShowDialog() == DialogResult.OK)
				//new FrmCnlMerge().ShowDialog() == DialogResult.OK)
				{
					//AddChannels(cnls, chkPreview.Checked);
					AddChannels(cnls);
					DialogResult = DialogResult.OK;
				}
			}
		}
	}
}


